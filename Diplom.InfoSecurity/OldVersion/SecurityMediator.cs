using System;
using System.Threading.Tasks;
using DiplomInfo.DataBase;
using DiplomInfo.DataBase.Models;


namespace Diplom.InfoSecurity
{
    public class SecurityMediator
    {
        private readonly FileRepository _fileRepository;
        private readonly WorkFile _workFile;
        private readonly SecurityService _securityService;
        public SecurityMediator(FileRepository fileRepository, string pathUploads, string pathWrite, string pathTempStorage)
        {
            _workFile = new WorkFile(pathUploads, pathWrite, pathTempStorage);
            _securityService = new SecurityService();
            _fileRepository = fileRepository;
        }



        public async Task<Guid> SaveFilesToRepositroty(string firstName, string secondName, string thirdName, string text)
        {
            // получение всех путей файлов в хранилище
            var files = _workFile.GetPathFiles();
            // задаем новый идентификатор для загружаемого файла
            var newId = Guid.NewGuid();
            foreach (var file in files)
            {
                // проверка фальсификации
                var fileName = _workFile.GetNameFile(file);
                if (_fileRepository.IsExsistsFileByTitleAndHash (fileName, text))
                    continue;

                #region Сохранение данных о файле
                
                // создание модели для хранения данных о файле
                var dataItem = new FileDTO()
                {
                    Id = newId,
                    Name = fileName,
                    DateTime = DateTime.UtcNow,
                    Hash = _securityService.CalculateMD5Hash(text),
                    AsymetricCode = _securityService.Encrypt(text, out byte[] encBytes, newId),
                    EncBytes = encBytes,
                    User = new User()
                    {
                        Id = Guid.NewGuid(),
                        FirstName = firstName,
                        SecondName = secondName,
                        ThirdName = thirdName,
                    }
                };
                // запись данных о файле
                _fileRepository.AddFile(dataItem);
                #endregion

                // создание файла с зашифрованным текстом
                _workFile.CreateFile(file, dataItem.AsymetricCode);
            }

            return newId;
        }


        public async Task<string> GetDecryptText(Guid fileId)
        {
            // получение данных о файле
            var model = _fileRepository.GetFileById(fileId);
            // Расшифрование текста зашифрованного файла
            var text = _securityService.Decyrpt(model.EncBytes, fileId);
            return text;
        }

        public void AddInRepositoryExiststFiles()
        {
            var files = _workFile.GetPathFiles();

            foreach (var file in files)
            {
                var fileName = _workFile.GetNameFile(file);
                if (_fileRepository.IsExsistsFileByTitle(fileName))
                    continue;

                #region Сохранение в базу
                var text = _workFile.GetTextFromFile(file);
                var newId = Guid.NewGuid();
                var dataItem = new FileDTO()
                {
                    Id = newId,
                    Name = fileName,
                    DateTime = DateTime.UtcNow,
                    Hash = _securityService.CalculateMD5Hash(text),
                    AsymetricCode = _securityService.Encrypt(text, out byte[] encBytes, newId),
                    EncBytes = encBytes,
                };
                _fileRepository.AddFile(dataItem);
                #endregion

                // сразу создается файл под с зашифрованными данными
                _workFile.CreateFile(file, dataItem.AsymetricCode);
            }
        }


        public bool IsExsistsFileByTitleAndText(string fileName, string text)
        {
            // получение хеша текста
            var hash = _securityService.CalculateMD5Hash(text);

            // проверка есть ли такой файл с таким название и содержимым по хешу
            return _fileRepository.IsExsistsFileByTitleAndHash(fileName, hash);
        }
    }
}

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
        public SecurityMediator(FileRepository fileRepository, string pathUploads, string pathWrite)
        {
            _workFile = new WorkFile(pathUploads, pathWrite);
            _securityService = new SecurityService();
            _fileRepository = fileRepository;
        }


        public async Task SaveFilesToRepositroty()
        {
            var files = _workFile.GetPathFiles();

            foreach (var file in files)
            {
                var fileName = _workFile.GetNameFile(file);
                if (_fileRepository.IsExsistsFileByTitle(fileName))
                    continue;

                #region Сохранение в базу
                var text = await _workFile.GetTextFromFile(file);
                var newId = Guid.NewGuid();
                var dataItem = new FileDTO()
                {
                    Id = newId,
                    Name = fileName,
                    Text = text,
                    DateTime = DateTime.UtcNow,
                    Hash = _securityService.CalculateMD5Hash(text),
                    AsymetricCode = _securityService.Encrypt(text, out byte[] encBytes, newId),
                    EncBytes = encBytes,
                };
                _fileRepository.AddFile(dataItem);
                #endregion

                // сразу создается файл под с зашифрованными данными
                await _workFile.CreateFile(file, dataItem.AsymetricCode);
            }
        }


        public async Task CreateNotExistsFile(string fileName, string text, Guid fileId)
        {
            var encryptText = _securityService.Encrypt(text, out byte[] encBytes, fileId);
            var listFiles = _workFile.GetPathFiles();
            if(!listFiles.Contains(fileName))
                await _workFile.CreateFile(fileName, encryptText);
        }



        public async Task<string> GetDecryptText(Guid fileId)
        {
            var model = _fileRepository.GetFileById(fileId);
            var text = _securityService.Decyrpt(model.EncBytes, fileId);
            return text;
        }

        public async Task AddInRepositoryExiststFiles()
        {
            var files = _workFile.GetPathFiles();

            foreach (var file in files)
            {
                var fileName = _workFile.GetNameFile(file);
                if (_fileRepository.IsExsistsFileByTitle(fileName))
                    continue;

                #region Сохранение в базу
                var text = await _workFile.GetTextFromFile(file);
                var newId = Guid.NewGuid();
                var dataItem = new FileDTO()
                {
                    Id = newId,
                    Name = fileName,
                    Text = text,
                    DateTime = DateTime.UtcNow,
                    Hash = _securityService.CalculateMD5Hash(text),
                    AsymetricCode = _securityService.Encrypt(text, out byte[] encBytes, newId),
                    EncBytes = encBytes,
                };
                _fileRepository.AddFile(dataItem);
                #endregion

                // сразу создается файл под с зашифрованными данными
                await _workFile.CreateFile(file, dataItem.AsymetricCode);
            }
        }
    }
}

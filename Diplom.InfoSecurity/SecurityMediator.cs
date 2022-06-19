using System;
using Diplom.InfoSecurity.Utils;
using DiplomInfo.DataBase.InterfaceRepository;
using DiplomInfo.DataBase.Models;


namespace Diplom.InfoSecurity
{
    public class SecurityMediator
    {
        private readonly IFileRepository _fileRepository;
        private readonly FileWorker _workFile;
        private readonly SecurityService _securityService;
        public SecurityMediator(IFileRepository fileRepository)
        {
            _workFile = new FileWorker();
            _securityService = new SecurityService();
            _fileRepository = fileRepository;
        }

        public string GetDecryptText(in Guid fileId)
        {
            var model = _fileRepository.GetById(fileId);
            var text = _securityService.Decyrpt(model.EncBytes, fileId);
            return text;
        }
        public bool IsExsistsFileByTitleAndText(string fileName, string text)
        {
            var hash = UtilSecurity.CalculateMD5Hash(text);
            return _fileRepository.IsExsistsByTitleAndHash(fileName, hash);
        }

        // объединить в модель
        public Guid SaveFilesToRepositroty(in string firstName, in string secondName, in string thirdName, in string text, in string pathDirectory)
        {
            var files = _workFile.GetPathFileFromDirectory(pathDirectory);
            var fileId = Guid.NewGuid();
            foreach (var file in files)
            {
                var fileName = _workFile.GetNameFile(file);
                if (_fileRepository.IsExsistsByTitleAndHash(fileName, text))
                    continue;

                var dataItem = new FileEntity()
                {
                    Id = fileId,
                    Name = fileName,
                    DateTime = DateTime.UtcNow,
                    Hash = UtilSecurity.CalculateMD5Hash(text),
                    AsymetricCode = _securityService.Encrypt(text, out byte[] encBytes, fileId),
                    EncBytes = encBytes,
                    User = new UserEntity()
                    {
                        Id = Guid.NewGuid(),
                        Name = firstName,
                        Surname = secondName,
                        Patronymic = thirdName,
                    }
                };

                _fileRepository.Add(dataItem);
;
                _workFile.CreateFile(file, dataItem.AsymetricCode, fileName);
            }

            return fileId;
        }
       
    }
}

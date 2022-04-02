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
        public SecurityMediator(FileRepository fileRepository)
        {
            string pathRead = Environment.CurrentDirectory + @"\uploads";
            string pathWrite = Environment.CurrentDirectory + @"\encryptfiles";

            _workFile = new WorkFile(pathRead, pathWrite);
            _securityService = new SecurityService();
            _fileRepository = fileRepository;
        }
        public async Task SaveFilesToRepositroty()
        {
            var files = _workFile.GetFiles();

            foreach (var file in files)
            {
                var text = await _workFile.ReadTextFromFile(file);
                var dataItem = new FileDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = _workFile.GetNameFile(file),
                    Text = text,
                    DateTime = DateTime.UtcNow,
                    Hash = _securityService.CalculateMD5Hash(text),
                    KeyDecrypt = "",
                    KeyEncrypt ="",
                    AsymetricCode = _securityService.Encrypt(text, out byte[] encBytes, out string keyEncrypt),
                };

                _fileRepository.AddFile(dataItem);

                // сразу создается файл под с зашифрованными данными
                await _workFile.CreateFile(file, dataItem.AsymetricCode);
            }
        }


        public async Task CreateNotExistsFile(string fileName, string text)
        {
            var encryptText = _securityService.Encrypt(text, out byte[] encBytes, out string encKey);
            await _workFile.CreateFile(text, encryptText);
        }
    }
}

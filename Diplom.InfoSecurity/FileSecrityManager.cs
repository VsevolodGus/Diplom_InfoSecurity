using System;
using System.Threading.Tasks;
using DiplomInfo.DataBase;
using DiplomInfo.DataBase.Models;
using Diplom.Utils;

namespace Diplom.InfoSecurity
{
    public class FileSecrityManager
    {
        private readonly FileRepository _fileRepository;
        public FileSecrityManager(FileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
        public async Task SaveFilesToRepositroty()
        {
            var files = FilesUtils.GetFilePathsFromUploads();

            foreach (var file in files)
            {
                var text = await FilesUtils.ReadTextFromFile(file);
                var dataItem = new FileDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = FilesUtils.GetNameFile(file),
                    Text = text,
                    DateTime = DateTime.UtcNow,
                    Hash = SecurityUtils.CalculateMD5Hash(text),
                    KeyDecrypt = "",
                    KeyEncrypt ="",
                    AsymetricCode = SecurityUtils.Encrypt(text, out byte[] encBytes, out string keyEncrypt),
                };

                _fileRepository.Add(dataItem);

                // сразу создается файл под с зашифрованными данными
                await FilesUtils.CreateFile(file, dataItem.AsymetricCode);
            }
        }


        public async Task CreateNotExistsFile(string fileName, string text)
        {
            var encryptText = SecurityUtils.Encrypt(text, out byte[] encBytes, out string encKey);
            await FilesUtils.CreateFile(fileName, encryptText);
        }
    }
}

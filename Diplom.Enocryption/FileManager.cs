using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Diplom.DataBase.Models;
using Diplom.DataBase.InterfaceRepository;

namespace Diplom.Enocryption
{
    public class FileManager
    {
        private readonly IFileRepository fileRepository;
        private readonly EncryptService encryptService;

        public FileManager(IFileRepository fileRepository)
        {
            this.fileRepository = fileRepository;
            this.encryptService = new EncryptService();
        }

        public async Task<bool> SetFile(IFormFile upload)
        {
            var file = new FileModel()
            {
                Id = Guid.NewGuid(),
            };
            try
            {
                file.FileContent = this.GetBLOBFile(upload);
                file.HashFile = this.CalculateSHA256Hash(file.FileContent, file.Id);
                file.AssimentCode = encryptService.AssimetricCode(file.AssimentCode, /*file.Key*/"");
                await fileRepository.AddFile(file);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// получение хеша файла
        /// </summary>
        /// <param name="input">blob файла</param>
        /// <returns></returns>
        private string CalculateSHA256Hash(string input, Guid fileId)
        {
            // step 1, calculate MD5 hash from input
            SHA256 md5 = SHA256.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input + fileId.ToString());
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// загрузка и получение даннных файла
        /// </summary>
        /// <param name="upload"> данные загружаемого файла</param>
        /// <returns></returns>
        private string GetBLOBFile(IFormFile upload)
        {
            using (var binaryReader = new BinaryReader(upload.OpenReadStream()))
            {
                var fileData = binaryReader.ReadBytes((int)upload.Length);

                return Encoding.UTF8.GetString(fileData);
            }
        }

    }
}

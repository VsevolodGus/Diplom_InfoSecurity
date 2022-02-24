using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Diplom.DataBase.Models;
using Diplom.DataBase;

namespace Diplom.Enocryption
{
    public class FileManager
    {
        private readonly FileRepository fileRepository;

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
                file.AssimentCode = this.AssimetricCode(file.AssimentCode, /*file.Key*/"");
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

        #region Получение ассиметричного кода        
        public string AssimetricCode(string input, string key)
        {
            var rsa = new RSACryptoServiceProvider();
            
            var publicKey = rsa.ExportParameters(false);

            var byteConverter = new UnicodeEncoding();

            byte[] encBytes = RSAEncrypt(byteConverter.GetBytes(input), publicKey, false);

            //var privateKey = asd.ExportParameters(true);
            //byte[] decBytes = RSADecrypt(encBytes, privateKey, false);
            return Encoding.UTF8.GetString(encBytes);
        }


        static public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            //Create a new instance of RSACryptoServiceProvider.
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            //Import the RSA Key information. This only needs
            //toinclude the public key information.
            RSA.ImportParameters(RSAKeyInfo);

            //Encrypt the passed byte array and specify OAEP padding.  
            //OAEP padding is only available on Microsoft Windows XP or
            //later.  
            return RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
        }

        static public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            //Create a new instance of RSACryptoServiceProvider.
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            //Import the RSA Key information. This needs
            //to include the private key information.
            RSA.ImportParameters(RSAKeyInfo);

            //Decrypt the passed byte array and specify OAEP padding.  
            //OAEP padding is only available on Microsoft Windows XP or
            //later.  
            return RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
        }
        #endregion
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Diplom.InfoSecurity
{
    public class SecurityMediator
    {
        private readonly RSACryptoServiceProvider _RSA;
        private readonly UnicodeEncoding _byteConverter;
        private readonly RSAParameters privateKey;
        private readonly RSAParameters publicKey;

        public SecurityMediator()
        {
            this._RSA = new RSACryptoServiceProvider();
            publicKey = _RSA.ExportParameters(true);
            privateKey = _RSA.ExportParameters(false);
            this._byteConverter = new UnicodeEncoding();
        }

        public string Encrypt(string input, out byte[] encBytes, out string encKey)
        {
            encKey = _byteConverter.GetString(publicKey.P);
            encBytes = SecurityService.RSAEncrypt(_byteConverter.GetBytes(input), publicKey, false);
            var encrypt = _byteConverter.GetString(encBytes);
            return encrypt;
        }


        public string Decyrpt(byte[] encBytes, out string decKey)
        {
            decKey = _byteConverter.GetString(publicKey.Modulus);
            byte[] decBytes = SecurityService.RSADecrypt(encBytes, publicKey, false);
            var decrypt = _byteConverter.GetString(decBytes);
            return decrypt;
        }

        public string CalculateSHA256Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            SHA256 md5 = SHA256.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public string GetBLOBFile(IFormFile upload)
        {
            using (var binaryReader = new BinaryReader(upload.OpenReadStream()))
            {
                var fileData = binaryReader.ReadBytes((int)upload.Length);

                return Encoding.UTF8.GetString(fileData);
            }
        }
    }
}

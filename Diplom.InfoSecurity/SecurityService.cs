using System.Text;
using System.Security.Cryptography;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Diplom.InfoSecurity
{
    internal class SecurityService
    {

        public string AssimetricCode(string input)
        {
            var rsa = new RSACryptoServiceProvider();

            var publicKey = rsa.ExportParameters(false);

            var byteConverter = new UnicodeEncoding();

            byte[] encBytes = RSAEncrypt(byteConverter.GetBytes(input), publicKey, false);

            //var privateKey = asd.ExportParameters(true);
            //byte[] decBytes = RSADecrypt(encBytes, privateKey, false);
            return Encoding.UTF8.GetString(encBytes);
        }


        public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            //Create a new instance of RSACryptoServiceProvider.
            var RSA = new RSACryptoServiceProvider();

            //Import the RSA Key information. This only needs
            //toinclude the public key information.
            RSA.ImportParameters(RSAKeyInfo);

            //Encrypt the passed byte array and specify OAEP padding.  
            //OAEP padding is only available on Microsoft Windows XP or
            //later.  
            return RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
        }

        public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            //Create a new instance of RSACryptoServiceProvider.
            var RSA = new RSACryptoServiceProvider();

            //Import the RSA Key information. This needs
            //to include the private key information.
            RSA.ImportParameters(RSAKeyInfo);

            //Decrypt the passed byte array and specify OAEP padding.  
            //OAEP padding is only available on Microsoft Windows XP or
            //later.  
            return RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
        }

        public string CalculateSHA256Hash(string input, Guid fileId)
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

using System.Text;
using System.Security.Cryptography;
using System;
using System.Linq;

namespace Diplom.InfoSecurity
{
    internal class SecurityService
    {
        private readonly RSACryptoServiceProvider _RSA;
        private readonly UnicodeEncoding _byteConverter;

        public SecurityService()
        {
            this._RSA = new RSACryptoServiceProvider();
            this._byteConverter = new UnicodeEncoding();
        }




        #region Шифрование дешифрование
        
        #region Static 
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
        #endregion

        public string Encrypt(string input, out byte[] encBytes, Guid fileId)
        {
            var publicKey = _RSA.ExportParameters(true);
            encBytes = RSAEncrypt(_byteConverter.GetBytes(input), publicKey, false);
            var encrypt = _byteConverter.GetString(encBytes);

            Utils.DataFile.Add(new KeyModel()
            {
                FileId = fileId,
                Key = publicKey,
            });

            return encrypt;
        }

        public string Decyrpt(byte[] encBytes, Guid fileId)
        {
            var data = Utils.DataFile.FirstOrDefault(c => c.FileId == fileId);
            byte[] decBytes = RSADecrypt(encBytes, data.Key, false);
            var decrypt = _byteConverter.GetString(decBytes);
            return decrypt;
        }
        #endregion


        #region Хеширование
        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
        #endregion
    }
}

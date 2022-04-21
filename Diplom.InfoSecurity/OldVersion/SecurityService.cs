using System;
using System.Text;
using System.Linq;
using System.Security.Cryptography;

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
            //Создание нового RSACryptoServiceProvider.
            var RSA = new RSACryptoServiceProvider();

            //Импорт ключевой информации RSA.
            RSA.ImportParameters(RSAKeyInfo);

            //Шифрование переданного массива байтов и указание заполнения OAEP.
            return RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
        }

        public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            //Создание нового RSACryptoServiceProvider.
            var RSA = new RSACryptoServiceProvider();

            //Импорт ключевой информации RSA.
            RSA.ImportParameters(RSAKeyInfo);

            //Дешифрование переданного массива байтов и указание заполнения OAEP.  
            return RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
        }
        #endregion

        public string Encrypt(string input, out byte[] encBytes, Guid fileId)
        {
            // создание ключа шифрования
            var privateKey = _RSA.ExportParameters(true);
            
            if (fileId != Guid.Empty)
            {
                // добавление в хранилище ключей для каждого файла
                Utils.DataFile.Add(new KeyModel()
                {
                    FileId = fileId,
                    Key = privateKey,
                });
            }

            // получение зашифрованного текста из файла в байтовом виде
            encBytes = RSAEncrypt(_byteConverter.GetBytes(input), privateKey, false);
            // получения текста из байтово вида
            var encrypt = _byteConverter.GetString(encBytes);

            return encrypt;
        }

        public string Decyrpt(byte[] encBytes, Guid fileId)
        {
            // получение ключа шифрования
            var dataKey = Utils.DataFile.FirstOrDefault(c => c.FileId == fileId);
            // получение расшифрованного текста из зашифрованного файла в байтовом виде
            byte[] decBytes = RSADecrypt(encBytes, dataKey.Key, false);
            // получения текста из байтово вида
            // 
            var decrypt = _byteConverter.GetString(decBytes);
            return decrypt;
        }
        #endregion


        #region Хеширование
        /// <summary>
        /// Функйия хеширования, используется стандарт MD5
        /// </summary>
        /// <param name="input">хешируемый текст</param>
        /// <returns></returns>
        public string CalculateMD5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            // получение текста в байтовом представлении
            var inputBytes = Encoding.ASCII.GetBytes(input);
            // хеширование
            var hash = md5.ComputeHash(inputBytes);

            // сборка захешированного текста
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

using System.Text;
using System.Security.Cryptography;

namespace Diplom.InfoSecurity.Utils
{
    public static class UtilSecurity
    {
        public static byte[] RSAEncrypt(in byte[] DataToEncrypt, in RSAParameters RSAKeyInfo, in bool DoOAEPPadding)
        {
            //Создание нового RSACryptoServiceProvider.
            var RSA = new RSACryptoServiceProvider();

            //Импорт ключевой информации RSA.
            RSA.ImportParameters(RSAKeyInfo);

            //Шифрование переданного массива байтов и указание заполнения OAEP.
            return RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
        }

        public static byte[] RSADecrypt(in byte[] DataToDecrypt, in RSAParameters RSAKeyInfo, in bool DoOAEPPadding)
        {
            //Создание нового RSACryptoServiceProvider.
            var RSA = new RSACryptoServiceProvider();

            //Импорт ключевой информации RSA.
            RSA.ImportParameters(RSAKeyInfo);

            //Дешифрование переданного массива байтов и указание заполнения OAEP.  
            return RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
        }

        public static string CalculateMD5Hash(in string input)
        {
            MD5 md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}

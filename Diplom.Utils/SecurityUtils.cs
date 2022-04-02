using System.Text;
using System.Security.Cryptography;

namespace Diplom.Utils
{
    public static class SecurityUtils
    {
        private static readonly RSACryptoServiceProvider _RSA = new RSACryptoServiceProvider();
        private static readonly UnicodeEncoding _byteConverter = new UnicodeEncoding();
        //private static readonly RSAParameters privateKey;
        //private static readonly RSAParameters publicKey;

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

        public static string Encrypt(string input, out byte[] encBytes, out string encKey)
        {
            var publicKey = _RSA.ExportParameters(true);

            encKey = _byteConverter.GetString(publicKey.P);
            encBytes = SecurityUtils.RSAEncrypt(_byteConverter.GetBytes(input), publicKey, false);
            var encrypt = _byteConverter.GetString(encBytes);
            return encrypt;
        }

        public static string Decyrpt(byte[] encBytes, out string decKey)
        {
            var publicKey = _RSA.ExportParameters(true);
            var privateKey = _RSA.ExportParameters(false);

            decKey = _byteConverter.GetString(publicKey.Modulus);
            byte[] decBytes = SecurityUtils.RSADecrypt(encBytes, publicKey, false);
            var decrypt = _byteConverter.GetString(decBytes);
            return decrypt;
        }

        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
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
    }
}

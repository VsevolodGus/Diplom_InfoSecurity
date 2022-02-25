using System.Text;
using System.Security.Cryptography;

namespace Diplom.Enocryption
{
    internal class EncryptService
    {
      
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


        private static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
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

        private static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
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
    }
}

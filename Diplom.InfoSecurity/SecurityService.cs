using System;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using Diplom.InfoSecurity.Utils;

namespace Diplom.InfoSecurity
{
    internal class SecurityService
    {
        private readonly RSACryptoServiceProvider _RSA;
        private readonly UnicodeEncoding _byteConverter;

        public SecurityService()
        {
            _RSA = new RSACryptoServiceProvider();
            _byteConverter = new UnicodeEncoding();
        }

        public string Encrypt(in string input, out byte[] encBytes, in Guid fileId)
        {
            var privateKey = _RSA.ExportParameters(true);
            
            if (fileId != Guid.Empty)
            {
                Utilsasdads.DataFile.Add(new KeyRSAModel()
                {
                    FileId = fileId,
                    Key = privateKey,
                });
            }

            encBytes = UtilSecurity.RSAEncrypt(_byteConverter.GetBytes(input), privateKey, false);
            var encrypt = _byteConverter.GetString(encBytes);

            return encrypt;
        }

        public string Decyrpt(in byte[] encBytes, Guid fileId)
        {
            var dataKey = Utilsasdads.DataFile.FirstOrDefault(c => c.FileId == fileId);
            byte[] decBytes = UtilSecurity.RSADecrypt(encBytes, dataKey.Key, false);

            var decrypt = _byteConverter.GetString(decBytes);
            return decrypt;
        }

    }
}

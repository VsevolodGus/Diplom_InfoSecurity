using System;
using System.Security.Cryptography;

namespace Diplom.InfoSecurity.Utils
{
    internal class KeyRSAModel
    {
        public Guid FileId { get; set; }
        public RSAParameters Key { get; set; }
    }
}

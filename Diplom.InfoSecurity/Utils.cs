using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Diplom.InfoSecurity
{
    public class KeyModel
    {
        public Guid FileId { get; set; }
        public RSAParameters Key { get; set; }
    }

    
    public static class Utils
    {
        public static readonly List<KeyModel> DataFile = new List<KeyModel>();

        public static readonly IReadOnlyList<string> FileExtentions = new List<string>()
        {
            ".txt",
            ".xlsx",
            ".csv",
            ".bin",
            ".dox",
            ".jpg",
        };
    }
}

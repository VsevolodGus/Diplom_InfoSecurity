using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Diplom.Utils
{
    public static class FilesUtils
    {
        public static readonly string DirectoryUploads = Environment.CurrentDirectory + @"\uploads";
        public static readonly string DirectoryDownload = Environment.CurrentDirectory + @"\encryptfiles";

        private static readonly IReadOnlyList<string> fileExtentions = new List<string>()
        {
            ".txt",
            ".xlsx",
            ".csv",
            ".bin",
            ".dox",
            ".jpg",
        };

        public static List<string> GetFilePathsFromUploads()
        {
            return new List<string>(Directory.GetFiles(DirectoryUploads));
        }

        public static string GetNameFile(string path)
        {
            var fileName = path.Split(@"\", System.StringSplitOptions.RemoveEmptyEntries).Last();

            foreach (var item in fileExtentions)
            {
                fileName = fileName.Replace(item, "");
            }

            return fileName;
        }

        public static  async Task<string> ReadTextFromFile(string path)
        {
            string result;
            using (var readre = new StreamReader(path))
            {
                result = await readre.ReadToEndAsync();
            }

            return result;
        }

        public static async Task CreateFile(string path, string encrypttext)
        {
            var fileName = GetNameFile(path);
            var filePath = DirectoryDownload + @"\" +fileName + ".txt";
            if (!File.Exists(filePath))
                File.Create(filePath);

            using (var writer = new StreamWriter(filePath))
            {
                await writer.WriteLineAsync(encrypttext);
            }
        }

    }
}

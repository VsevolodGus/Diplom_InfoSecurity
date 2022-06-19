using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Diplom.InfoSecurity.Utils
{
    public static class FileUtils
    {
        // опасно делать static async метод, наверное нужно вывести в отдельный класс
        public static async Task<MemoryStream> GetFilledStreamFromFile(string path, string fileName)
        {
            var memory = new MemoryStream();
            var filePath = path + @"\" + fileName;
            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return memory;
        }

        public static string GetErrorMessageCheckFile(IFormFile file)
        {
            if (file == null)
                return "Not found";

            var maxSizeFile = 1000;
            if (file.Length > maxSizeFile)
                return "FileIsBig";

            return string.Empty;
        }

        public static async Task<string> CreateAndGetPathFile(IFormFile file, string path)
        {
            string pathFile = path + @"\" + file.Name;
            DeleteFile(pathFile);
            using (var fileStream = new FileStream(pathFile, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return pathFile;
        }
        private static void DeleteFile(in string pathFile)
        {
            if (File.Exists(pathFile))
                File.Delete(pathFile);
        }

        public static string GetPathDirectoryCheckExistsAndCreate(in string nameDirectory)
        {
            var path = Environment.CurrentDirectory + @"\" + nameDirectory;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        public static string GetTextFromFile(in string path)
        {
            return File.ReadAllText(path);
        }
            
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System;

namespace Diplom.InfoSecurity
{
    internal class WorkFile
    {   
        private readonly string _pathRead;
        private readonly string _pathWrite;

        public WorkFile(string pathRead, string pathWrite)
        {
            this._pathRead = pathRead;
            this._pathWrite = pathWrite;
        }

        #region Get
        public List<string> GetPathFiles()
        {
            return new List<string>(Directory.GetFiles(this._pathRead));
        }

        public string GetNameFile(string path, bool withExtension = true)
        {
            var fileName = path.Split(@"\", System.StringSplitOptions.RemoveEmptyEntries).Last();

            if (!withExtension)
            {
                foreach (var item in Utils.FileExtentions)
                {
                    fileName = fileName.Replace(item, "");
                }
            }

            return fileName;
        }

        public async Task<string> GetTextFromFile(string path)
        {
            string result;
            using (var readre = new StreamReader(path))
            {
                result = await readre.ReadToEndAsync();
            }

            return result;
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"> путь для сохранения</param>
        /// <param name="encrypttext"> зашифрованный текст </param>
        /// <returns></returns>
        public async Task CreateFile(string path, string encrypttext)
        {
            var fileName = GetNameFile(path);
            var filePath = _pathWrite + @"\" + fileName;
            if (!File.Exists(filePath))
                File.Create(filePath);

            while (!IsLocked(filePath))
                continue;

            using (var writer = new StreamWriter(filePath))
            {
                await writer.WriteLineAsync(encrypttext);
            }
        }

        private static bool IsLocked(string fileName)
        {
            try
            {
                using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    fs.Close();
                    // Здесь вызываем свой метод, работаем с файлом
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2147024894)
                    return false;
            }
            return true;
        }
    }
}

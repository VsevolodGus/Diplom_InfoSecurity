﻿using System.Collections.Generic;
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
        private readonly IReadOnlyList<string> fileExtentions = new List<string>()
        {
            ".txt",
            ".xlsx",
            ".csv",
            ".bin",
            ".dox",
            ".jpg",
        };


        public WorkFile(string pathRead, string pathWrite)
        {
            this._pathRead = pathRead;
            this._pathWrite = pathWrite;
        }

        public List<string> GetFiles()
        {
            return new List<string>(Directory.GetFiles(this._pathRead));
        }

        public string GetNameFile(string path)
        {
            var fileName = path.Split(@"\", System.StringSplitOptions.RemoveEmptyEntries).Last();

            foreach (var item in fileExtentions)
            {
                fileName = fileName.Replace(item, "");
            }

            return fileName;
        }

        public async Task<string> ReadTextFromFile(string path)
        {
            string result;
            using (var readre = new StreamReader(path))
            {
                result = await readre.ReadToEndAsync();
            }

            return result;
        }

        public async Task CreateFile(string path, string encrypttext)
        {
            var fileName = GetNameFile(path);
            var filePath = _pathWrite + @"\" +fileName + ".txt";
            if (!File.Exists(filePath))
                File.Create(filePath);

            while (IsLocked(filePath))
                continue;

            using (var writer = new StreamWriter(filePath))
            {
                await writer.WriteLineAsync(encrypttext);
            }
        }

        public bool IsLocked(string fileName)
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

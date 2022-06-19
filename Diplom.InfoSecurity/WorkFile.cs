using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Diagnostics;

namespace Diplom.InfoSecurity
{
    internal class FileWorker
    {
        public List<string> GetPathFileFromDirectory(in string pathDirectory)
        {
            return new List<string>(Directory.GetFiles(pathDirectory));
        }

        public string GetNameFile(in string path, bool withExtension = true)
        {
            var fileName = path.Split(@"\", StringSplitOptions.RemoveEmptyEntries).Last();

            if (!withExtension)
            {
                fileName = fileName.Split('.', StringSplitOptions.RemoveEmptyEntries).First();
            }

            return fileName;
        }


        public void CreateFile(in string path, in string encrypttext, in string fileWrite)
        {
            var fileName = GetNameFile(path);
            //var filePath = path + @"\" + fileName;
            if (!File.Exists(path))
                File.Create(path);

            // таймер, чтобы не было бесконечного зацикливания
            // выдает ошибку по не поянтной причине
            var timer = new Stopwatch();
            timer.Start();
            while (timer.ElapsedMilliseconds < 1000)
            {
                try
                {
                    using (var writer = new StreamWriter(path))
                    { 
                        writer.WriteLine(encrypttext); 
                    }
                    break;
                }
                catch  { }
            }
            timer.Stop();
        }
    }
}

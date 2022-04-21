using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Diagnostics;

namespace Diplom.InfoSecurity
{
    internal class WorkFile
    {   
        private readonly string _pathRead;
        private readonly string _pathWrite;
        private readonly string _pathTempStorage;


        public WorkFile(string pathRead, string pathWrite, string pathTempStorage)
        {
            this._pathRead = pathRead;
            this._pathWrite = pathWrite;
            this._pathTempStorage = pathTempStorage;
        }

        #region Get
        public List<string> GetPathFiles()
        {
            // получени путей файлов из директории 
            return new List<string>(Directory.GetFiles(this._pathRead));
        }

        public string GetNameFile(string path, bool withExtension = true)
        {
            // получение последней части пути после \
            var fileName = path.Split(@"\", StringSplitOptions.RemoveEmptyEntries).Last();

            // удаляем расширения из названия
            if (!withExtension)
            {
                fileName = fileName.Split('.', StringSplitOptions.RemoveEmptyEntries).First();
            }

            return fileName;
        }

        public string GetTextFromFile(string path)
        {
            string result;
            //  объявления потока для чтения текста из файла
            using (var readre = new StreamReader(path))
            {
                // чтение текста из файла
                result = readre.ReadToEnd();
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
        public void CreateFile(string path, string encrypttext)
        {
            // получение имени файла по по пути
            var fileName = GetNameFile(path);
            // составление пути
            var filePath = _pathWrite + @"\" + fileName;
            // проверка наличия фалйа по заданному пути
            if (!File.Exists(filePath))
                File.Create(filePath);

            // создание таймера
            var time = new Stopwatch();
            // запуск таймера
            time.Start();
            while (time.ElapsedMilliseconds < 1000)
            {
                // попытка записи текста в файл
                try
                {
                    // создание потока для записи в файл
                    using (var writer = new StreamWriter(filePath))
                    {
                        // запись в файл
                        writer.WriteLine(encrypttext); 
                    }
                    //выход из цикла
                    break;
                }
                catch
                { }
            }
            time.Stop(); // остановка таймера
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

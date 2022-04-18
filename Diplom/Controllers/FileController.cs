using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Diplom.InfoSecurity;
using Diplom.Models;
using DiplomInfo.DataBase;
using DiplomInfo.DataBase.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diplom.Controllers
{
    // TODO 
    // уточнить по поводу ключей для шифрования

    
    public class FileController : Controller
    {
        private readonly FileRepository _fileRepository;
        private readonly SecurityMediator _securityMediator;
        private readonly string _pathUploads;
        private readonly string _pathWrite;
        public FileController(FileRepository fileRepository)
        {
            this._fileRepository = fileRepository;
            this._pathUploads = Environment.CurrentDirectory + @"\uploads";
            if (Directory.Exists(_pathUploads))
                Directory.CreateDirectory(_pathUploads);

            this._pathWrite = Environment.CurrentDirectory + @"\encryptfiles";
            if (Directory.Exists(_pathWrite))
                Directory.CreateDirectory(_pathWrite);

            this._securityMediator = new SecurityMediator(_fileRepository, _pathUploads, _pathWrite);
            _securityMediator.AddInRepositoryExiststFiles().Wait();
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Получение данных
        public async Task<IActionResult> GetListDataFiles(string query = "", int skipCount = 0, int count = 10)
        {
            var files = _fileRepository.GetFiles(query, skipCount, count);
            var model = files.Select(c => new FileModelTable
            {
                Id = c.Id,
                Name = c.Name,
                DateTme = c.DateTime,
                User = c.User ?? new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "",
                    SecondName = "",
                    ThirdName = ""
                },
            }).ToList();
            return View("ListFiles", model);
        }

        
        public async Task<IActionResult> GetDataFile(Guid fileId)
        {
            var model = _fileRepository.GetFileById(fileId);
            return View("FileData", model);
        }
        #endregion

        #region Работа с данными
        /// <summary>
        /// првоерка коррекктнотси загружаемого файла и его сохранение
        /// </summary>
        /// <param name="firstName"> имя владельца</param>
        /// <param name="secondName"> фамилия владельца </param>
        /// <param name="thirdName"> отчество владельца </param>
        /// <returns></returns>
        public async Task<IActionResult> Uploads(string firstName, string secondName, string thirdName)
        {
            #region Проверка корректности имени владельца загружаемого файла
            if (firstName is null)
                firstName = "";
            if (secondName is null)
                secondName = "";
            if (thirdName is null)
                thirdName = "";
            #endregion

            

            #region Проверки на корректность
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null)
                return NotFound();

            //ограничение размер файла и проверка существования в базе
            if (file.Length > 1000)
                return View(); // файл слишком большой

            // возможно это фальсифицированный
            if (_fileRepository.IsExsistsFileByTitle(file.Name.Split(".", StringSplitOptions.RemoveEmptyEntries).First()))
                return View();
            #endregion

            // задание пути для сохранения файла откуда будет считваться текст
            string fullPath = _pathUploads + @"\" + file.Name;
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // сохранение в базу
            var fileId = await _securityMediator.SaveFilesToRepositroty(firstName, secondName, thirdName);

            return await GetDataFile(fileId);
        }


        
        [HttpGet, DisableRequestSizeLimit]
        public async Task<IActionResult> DownloadFile(Guid id)
        {
            var model = _fileRepository.GetFileById(id);
            var memory = new MemoryStream();

            // создание выгружаемого файла
            await using (var stream = new FileStream(_pathWrite + @"\" + model.Name, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            // запись корректного типа файла
            // выгрузка и определние расширения
            return File(memory, "application/octet-stream", model.Name.Split('.')[0] + ".sig");
        }
        #endregion

        #region Получение расшифровки
        public async Task<IActionResult> GetDecryptText(Guid id)
        {
            var decryptText = await _securityMediator.GetDecryptText(id);
            return View("DecryptText", decryptText);
        }
        #endregion
    }
}

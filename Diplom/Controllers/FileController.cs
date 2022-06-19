using System;
using System.Linq;
using System.Threading.Tasks;
using Diplom.InfoSecurity;
using Diplom.InfoSecurity.Utils;
using Diplom.Models;
using DiplomInfo.DataBase.InterfaceRepository;
using DiplomInfo.DataBase.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diplom.Controllers
{
    public class FileController : Controller
    {
        // сделать переходный уровень между controller и repository
        private readonly IFileRepository _fileRepository;
        private readonly SecurityMediator _securityMediator;
        private readonly string _pathUploads;
        private readonly string _pathWrite;
        private readonly string _pathTempStorage;
        public FileController(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
            _securityMediator = new SecurityMediator(_fileRepository);

            _pathUploads  = FileUtils.GetPathDirectoryCheckExistsAndCreate("uploads");
            _pathWrite = FileUtils.GetPathDirectoryCheckExistsAndCreate("encryptfiles");
            _pathTempStorage = FileUtils.GetPathDirectoryCheckExistsAndCreate("tempStorage");
        }


        public IActionResult Index()
        {
            return View();
        }

        #region Methods getting data

        // вынести в методы где берутся эти данные
        [HttpGet]
        public IActionResult GetListDataFiles(string query = "")
        {
            var files = _fileRepository.GetListBySearch(query);
            var model = files.Select(c => new FileModelTable
            {
                Id = c.Id,
                Name = c.Name,
                DateTme = c.DateTime,
                User = c.User ?? new UserEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = "",
                    Surname = "",
                    Patronymic = ""
                },
            }).ToList();
            return View("ListFiles", model);
        }
        
        [HttpGet]
        public IActionResult GetDataFile(Guid fileId)
        {
            var model = _fileRepository.GetById(fileId);
            return View("FileData", model);
        }

        [HttpGet]
        public IActionResult GetDecryptText(Guid id)
        {
            var decryptText = _securityMediator.GetDecryptText(id);
            return View("DecryptText", decryptText);
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> Uploads(string firstName, string secondName, string thirdName)
        {
            var file = Request.Form.Files.FirstOrDefault();

            UserUtils.CheckOrDefaultFIO(ref firstName, ref secondName, ref thirdName);
            var errorMessage = FileUtils.GetErrorMessageCheckFile(file);
            if (!string.IsNullOrEmpty(errorMessage))
                return View(errorMessage);


            var pathTempStorage = await FileUtils.CreateAndGetPathFile(file, _pathTempStorage);
            var text = FileUtils.GetTextFromFile(pathTempStorage);
            if (_securityMediator.IsExsistsFileByTitleAndText(file.Name, text))
                return View("FileIsExists");


            // Сохранение в основное хранилище
            await FileUtils.CreateAndGetPathFile(file, _pathUploads);
            var fileId = _securityMediator.SaveFilesToRepositroty(firstName, secondName, thirdName, text, _pathUploads);

            return GetDataFile(fileId);
        }



        [HttpGet, DisableRequestSizeLimit]
        public async Task<IActionResult> DownloadSigFile(Guid id)
        {
            var model = _fileRepository.GetById(id);
            var memoryStream = await FileUtils.GetFilledStreamFromFile(_pathWrite, model.Name);
            return File(memoryStream, "application/octet-stream", model.Name.Split('.')[0] + ".sig");
        }

    }
}

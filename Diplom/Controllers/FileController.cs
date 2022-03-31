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
        public FileController(FileRepository fileRepository, SecurityMediator securityMediator)
        {
            this._fileRepository = fileRepository;
            this._securityMediator = securityMediator;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> GetListDataFiles(string query = "", int skipCount = 0, int count = 10)
        {
            var files = _fileRepository.GetFiles(query, skipCount, count);
            var model = files.Select(c=> new FileModelTable
            {
                Id = c.Id,
                Name = c.Name,
                DateTme = c.DateTime,
            }).ToList();
            return View("ListFiles", model);
        }


        public async Task<IActionResult> GetDataFile(Guid fileId)
        {
            var model = _fileRepository.GetFileById(fileId);
            model.AsymetricCode = _securityMediator.Encrypt(model.Blob, out byte[] encBytes, out string keyEncrypt);
            model.KeyEncrypt = keyEncrypt;
            
            model.Hash = _securityMediator.CalculateSHA256Hash(model.Blob);
            /*string asd = */_securityMediator.Decyrpt(encBytes, out string keyDecrypt);
            model.KeyDecrypt = keyDecrypt;
            return View("FileData", model);
        }

        public async Task<IActionResult> Upload()
        {
            var upload = Request.Form.Files.FirstOrDefault();
            if (upload is null)
            {
                return BadRequest();
            }

            // получаем имя файла
            string filePath = Path.GetFileName(upload.FileName);

            var model = new FileDTO()
            {
                Id = Guid.NewGuid(),
                Name = filePath,
                DateTime = DateTime.Now,
            };
            if (upload.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await upload.CopyToAsync(stream);
                }
            }
            model.Blob = _securityMediator.GetBLOBFile(upload);
            model.Hash = _securityMediator.CalculateSHA256Hash(model.Blob);
            model.AsymetricCode = _securityMediator.Encrypt(model.Blob, out byte[] encBytes, out string keyEncrypt);
            _securityMediator.Decyrpt(encBytes, out string keyDecrypt);
            model.KeyEncrypt = keyEncrypt;
            model.KeyDecrypt = keyDecrypt;

            return RedirectToAction("Index");
        }
    }
}

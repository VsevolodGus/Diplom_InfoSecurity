using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Diplom.Models;
using DiplomInfo.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace Diplom.Controllers
{
    // TODO 
    // уточнить по поводу ключей для шифрования

    
    public class FileController : Controller
    {
        private FileRepository _fileRepository;
        public FileController(FileRepository fileRepository)
        {
            this._fileRepository = fileRepository;
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
                Blob = c.Blob,
            }).ToList();
            return View("ListFiles", model);
        }


        public async Task<IActionResult> GetDataFile(Guid fileId)
        {
            var model = _fileRepository.GetFileById(fileId);
            return View("DataFile", model);
        }

        public async Task<IActionResult> Upload()
        {
            var upload = Request.Form.Files.FirstOrDefault();
            if (upload is null)
            {
                return BadRequest();
            }
            if (upload != null)
            {
                // получаем имя файла
                string filePath = Path.GetFileName(upload.FileName);
                
                if (upload.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await upload.CopyToAsync(stream);
                        
                    }
                }
            }


            return RedirectToAction("Index");
        }
    }
}

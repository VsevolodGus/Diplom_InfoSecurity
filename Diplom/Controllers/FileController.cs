using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Diplom.InfoSecurity;
using Diplom.Models;
using DiplomInfo.DataBase;
using DiplomInfo.DataBase.Models;
using Microsoft.AspNetCore.Mvc;
using Diplom.Utils;

namespace Diplom.Controllers
{
    // TODO 
    // уточнить по поводу ключей для шифрования

    
    public class FileController : Controller
    {
        private readonly FileRepository _fileRepository;
        private readonly FileSecrityManager _securityMediator;
        public FileController(FileRepository fileRepository, FileSecrityManager securityMediator)
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
            return View("FileData", model);
        }


        public async Task<IActionResult> Uploads()
        {
            var files = Request.Form.Files;
            
            if (Directory.Exists(FilesUtils.DirectoryUploads)) 
                Directory.CreateDirectory(FilesUtils.DirectoryUploads);
            

            foreach (var file in files)
            {
                if (file.Length > 1024)
                    continue;
                string fullPath = FilesUtils.DirectoryUploads + @"\" + file.Name;
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            await _securityMediator.SaveFilesToRepositroty();

            return await GetListDataFiles();
        }

        [HttpGet, DisableRequestSizeLimit]
        public async Task<IActionResult> DownloadFile(Guid id)
        {
            var model = _fileRepository.GetFileById(id);
            var memory = new MemoryStream();

            var pathFile = FilesUtils.DirectoryDownload + @"\" + model.Name + ".txt";
            if (!System.IO.File.Exists(pathFile))
                await _securityMediator.CreateNotExistsFile(model.Name, model.Text);


            await using (var stream = new FileStream(FilesUtils.DirectoryDownload + @"\" + model.Name + ".txt", FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            //set correct content type here
            return File(memory, "application/octet-stream", model.Name + ".sig");


        }
    }
}

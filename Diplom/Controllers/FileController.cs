using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Diplom.Controllers
{
    // TODO 
    // уточнить по поводу ключей для шифрования








    
    public class FileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> GetListDataFiles(int skipCount, int count)
        {
            return null;
        }


        public async Task<IActionResult> GetDataFiles(Guid faileId)
        {
            return null;
        }


        public async Task<IActionResult> SetFile(/*file*/)
        {
            return null;
        }




        [HttpPost, Route("file/upload")]
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

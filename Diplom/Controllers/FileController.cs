using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diplom.Controllers
{
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
    }
}

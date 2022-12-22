using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Net.Mime;
using UplodeFileToDatabase.Models;
using UplodeFileToDatabase.Repositories;
using static System.Net.Mime.MediaTypeNames;

namespace UplodeFileToDatabase.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileRepositorie fileRepositorie; 
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger , IFileRepositorie fileRepositorie)
        {
            _logger = logger;
            this.fileRepositorie = fileRepositorie;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IndexVM indexVM = new IndexVM();
            return View(indexVM);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IndexVM indexVM)
        {
           var id = await fileRepositorie.UploadFile(indexVM.fileUpload);

            return Ok(id);
        }

        public async Task<IActionResult> Privacy()
        {
          var file =   await fileRepositorie.DownloadFileById(2);

            switch (file.Type)
            {
                case "text/plain":
                    file.Name = file.Name + ".txt";
                    break;
                case "image/webp":
                    file.Name = file.Name + ".webp";
                    break;
                default:
                    return BadRequest();
                    break;
            }
           
            return File(file.Data, file.Type, file.Name);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
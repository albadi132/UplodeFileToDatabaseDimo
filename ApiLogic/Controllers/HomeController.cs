using DataModel;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiLogic.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly TestDBContext _db;
        public HomeController(TestDBContext testDBContext)
        {
            _db = testDBContext;

        }

        [HttpPost]
        public async Task<string> UploadFile([FromForm] IFormFile file)
        {
            

            try
            {

                Filestorge filestorge = new()
                {
                    Name = file.Name,
                    Type = file.ContentType,
                    Id = 2
                };

                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    filestorge.Data = stream.ToArray();
                }

                //var fileSave = 
                
                var add =await _db.Filestorge.AddAsync(filestorge);
                await _db.SaveChangesAsync();

                return "done";
            }
            catch (Exception)
            {

              
                return null;

            }
        }

        [HttpGet]
        public async Task<FileDto> DownloadFileById(int Id)
        {


            try
            {

                var file = await _db.Filestorge.Where(x => x.Id == Id).FirstOrDefaultAsync();


                FileDto fileDto = new()
                {
                    Name = file.Name,
                    Type = file.Type,
                    Data = file.Data
                };
                return fileDto;

            }
            catch (Exception)
            {


                return null;

            }
        }

        private async Task CopyStream(Stream stream, string downloadPath)
        {
            using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
        }
    }
}

using DTO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace UplodeFileToDatabase.Repositories
{
    public interface IFileRepositorie
    {

        Task<string> UploadFile(IFormFile fileUpload);
        Task<FileDto> DownloadFileById(int id);
    }

    public class FileRepositorie : IFileRepositorie
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        public FileRepositorie(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> UploadFile(IFormFile fileUpload)
        {


            //HttpClientHandler clientHandler = new HttpClientHandler();
            //clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            //using HttpClient client = new(clientHandler);


            using HttpClient client = new();

            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(fileUpload.OpenReadStream());

            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(fileUpload.ContentType);

            content.Add(fileContent, "file", fileUpload.FileName);

            var response = await client.
                PostAsync($"https://localhost:7240/api/Home/UploadFile", content);



            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();


           string businessunits = responseBody;

            return businessunits;
        }

        public async Task<FileDto> DownloadFileById(int id)
        {


            //HttpClientHandler clientHandler = new HttpClientHandler();
            //clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            //using HttpClient client = new(clientHandler);


            using HttpClient client = new();

            
            var response = await client.
                GetAsync($"https://localhost:7240/api/Home/DownloadFileById?Id={id}");


            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();


            FileDto businessunits = JsonConvert.DeserializeObject<FileDto>(responseBody);

            return businessunits;
        }

    }
}

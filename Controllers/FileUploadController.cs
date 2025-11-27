using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace firstRagWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        public readonly IConfiguration _config;
        public FileUploadController(IConfiguration config)
        {
            _config = config;

        }

        [HttpPost]
        public async Task<IActionResult> upload(IFormFile file)
        {
            var blobclient = new BlobContainerClient(_config["Azure:BlobStorageConnectionString"], _config["Azure:BlobContainer"]);
            await blobclient.CreateIfNotExistsAsync();

            var blob = blobclient.GetBlobClient(file.FileName);
            await using var stream = file.OpenReadStream();
            await blob.UploadAsync(stream,overwrite:true);
            return Ok(new {file.FileName});
        }
    }
}

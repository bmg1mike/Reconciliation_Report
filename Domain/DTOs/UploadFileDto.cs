using Microsoft.AspNetCore.Http;

namespace Domain
{
    public class UploadFileDto
    {
        public IFormFile? File { get; set; }
    }
}

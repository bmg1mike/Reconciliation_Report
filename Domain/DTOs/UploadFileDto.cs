using Microsoft.AspNetCore.Http;

namespace ReconciliationReport.DTOs
{
    public class UploadFileDto
    {
        public IFormFile? File { get; set; }
    }
}

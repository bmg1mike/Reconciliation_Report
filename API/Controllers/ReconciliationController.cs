using Domain;
using Microsoft.AspNetCore.Mvc;
using ReconciliationReport.Services.Interface;

namespace ReconciliationReport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReconciliationController : ControllerBase
    {
        private readonly IReconciliationService service;

        public ReconciliationController(IReconciliationService service)
        {
            this.service = service;
        }

        [HttpPost("UploadFileForInward")]
        public async Task<IActionResult> UploadFileForInward([FromForm] UploadFileDto request)
        {
            var report = await service.UploadFile(request,"Inward");
            return Ok(report);
        }
        [HttpPost("UploadFileForOutward")]
        public async Task<IActionResult> UploadFileForOutward([FromForm] UploadFileDto request)
        {
            var report = await service.UploadFile(request,"Outward");
            return Ok(report);
        }
    }
}

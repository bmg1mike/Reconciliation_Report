using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReconciliationReport.DTOs;
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

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadReportFile([FromForm] UploadFileDto request)
        {
            var report = await service.UploadFile(request);
            return Ok(report);
        }
    }
}

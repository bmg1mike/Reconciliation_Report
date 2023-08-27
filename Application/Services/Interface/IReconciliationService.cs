using ReconciliationReport.Common;
using ReconciliationReport.DTOs;

namespace ReconciliationReport.Services.Interface
{
    public interface IReconciliationService
    {
        Task<Result<string>> UploadFile(UploadFileDto request);
        Task CompareOutward();
        Task CompareInward();
    }
}
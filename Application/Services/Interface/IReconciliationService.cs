using ReconciliationReport.Common;
using Domain;

namespace ReconciliationReport.Services.Interface
{
    public interface IReconciliationService
    {
        Task<Result<string>> UploadFile(UploadFileDto request,string fileType);
        Task CompareOutward();
        Task CompareInward();
    }
}
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReconciliationReport.Common;
using ReconciliationReport.Data;
using ReconciliationReport.Entities;
using ReconciliationReport.Services.Interface;
using System.IO;

namespace ReconciliationReport.Services.Implementation
{
    public class ReconciliationService : IReconciliationService
    {
        private readonly ILogger<ReconciliationService> _logger;
        private readonly ReconciliationContext _context;

        public ReconciliationService(ILogger<ReconciliationService> logger, ReconciliationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result<string>> UploadFile(UploadFileDto request, string fileType)
        {
            try
            {
                var validInwardReport = new List<InwardReport>();
                var validOutwardReport = new List<OutwardReport>();
                var inwardBatch = new NipInwardBatch();
                var outwardBatch = new NipOutwardBatch();
                var batchId = DateTime.Now.ToString("yyyy MM dd HH mm ss").Replace(" ", string.Empty);

                var dir = $"{Directory.GetCurrentDirectory()}/BulkFiles";

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                if (request.File == null)
                {
                    _logger.LogInformation("There was no file sent");
                    return Result<string>.Failure("There was no file sent");

                }

                FileInfo fileInfo = new FileInfo(request.File.FileName);


                if (fileInfo.Extension != ".csv")
                {
                    _logger.LogInformation("The File received is not an CSV file");
                    return Result<string>.Failure("The File received is not a CSV file");

                }

                var uniqueFileName = $"{batchId}_{request.File.FileName}";

                var uploadsFolder = dir;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.File.CopyToAsync(stream);
                }


                if (fileType == "Inward")
                {
                    var reports = ReadInwardReport(filePath, batchId); // 63851
                    validInwardReport = reports.Where(x => x.CHANNEL != null).ToList();
                    validInwardReport = validInwardReport.Where(x => x.CHANNEL.Contains("CHANNEL") == false).ToList();
                    inwardBatch.BatchId = batchId;
                    inwardBatch.FileName = uniqueFileName;

                    await _context.NipInwardBatches.AddAsync(inwardBatch);
                    await _context.InwardReports.AddRangeAsync(validInwardReport);
                }
                else
                {
                    var reports = ReadOutwardReport(filePath, batchId);
                    validOutwardReport = reports.Where(x => x.CHANNEL != null).ToList();
                    validOutwardReport = validOutwardReport.Where(x => x.CHANNEL.Contains("CHANNEL") == false).ToList();

                    outwardBatch.BatchId = batchId;
                    outwardBatch.FileName = uniqueFileName;
                    await _context.NipOutwardBatches.AddAsync(outwardBatch);
                    await _context.OutwardReports.AddRangeAsync(validOutwardReport);
                }

                await _context.SaveChangesAsync();

                return Result<string>.Success($"File has been uploaded Successfully. File Id is {batchId}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result<string>.Failure("There was an error with your upload, Please try again later");

            }
        }

        public async Task CompareInward()
        {
            try
            {
                var inwardReports = _context.InwardReports
                                    .Where(x => x.IsProcessed == false)
                                    .AsParallel()
                                    .AsOrdered()
                                    .ToList();
                foreach (var item in inwardReports)
                {
                    var inwardTransaction = await _context.NIPInboundTransactions.Where(x => x.SessionId == item.SESSION_ID).FirstOrDefaultAsync();

                    if (inwardTransaction == null)
                    {
                        item.IsProcessed = true;
                        item.TransactionExist = false;
                        continue;
                    }
                    else if (inwardTransaction is not null)
                    {
                        if (inwardTransaction.TransactionProcessed == 1
                            && inwardTransaction.Requery == "00"
                            && !string.IsNullOrEmpty(inwardTransaction.VtellerPrinRsp)
                            && inwardTransaction.TransactionProcessedDate != null
                            && inwardTransaction.Approvevalue == 1
                            && inwardTransaction.ResponseCode == "00"
                            && (inwardTransaction.InwardType == 1
                                || inwardTransaction.InwardType == 2
                                || inwardTransaction.InwardType == 5))
                        {
                            item.TransactionExist = true;
                            item.IsProcessed = true;
                            item.IsCredited = true;

                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        item.IsProcessed = true;
                        item.TransactionExist = true;
                        item.IsCredited = false;

                        await _context.SaveChangesAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
        public async Task CompareOutward()
        {
            try
            {
                var outwardReports = _context.OutwardReports
                                    .Where(x => x.IsProcessed == false)
                                    .AsParallel()
                                    .AsOrdered()
                                    .ToList();
                foreach (var item in outwardReports)
                {
                    var outwardTransaction = await _context.NIPOutwardTransactions.Where(x => x.SessionID == item.SESSION_ID).FirstOrDefaultAsync();

                    if (outwardTransaction == null)
                    {
                        item.IsProcessed = true;
                        item.TransactionExist = false;
                        continue;
                    }
                    else if (outwardTransaction is not null)
                    {
                        if (outwardTransaction.KafkaStatus == "Processed")
                        {
                            item.TransactionExist = true;
                            item.IsProcessed = true;
                            item.IsDebited = true;

                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        item.IsProcessed = true;
                        item.TransactionExist = true;
                        item.IsDebited = false;

                        await _context.SaveChangesAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
        private List<InwardReport> ReadInwardReport(string path, string batchId)
        {
            var list = File.ReadAllLines(path)
                .Skip(1)
                .Select(x => GetInwardReportFromFile(x, batchId))
                .AsParallel()
                .ToList();
            return list;
        }

        private List<OutwardReport> ReadOutwardReport(string path, string batchId)
        {
            var list = File.ReadAllLines(path)
                .Skip(1)
                .Select(x => GetOutwardReportFromFile(x, batchId))
                .AsParallel()
                .ToList();
            return list;
        }

        private InwardReport GetInwardReportFromFile(string uploadValues, string batchId)
        {
            var report = new InwardReport();
            var values = uploadValues.Split(',');
            if (values.Length != 15)
            {
                return new InwardReport();
            }
            report.BatchId = batchId;
            report.CHANNEL = values[1].Replace("\"", string.Empty);
            report.SESSION_ID = values[2].Replace("\"", string.Empty); ;
            report.TRANSACTION_TYPE = values[3].Replace("\"", string.Empty); ;
            report.RESPONSE = values[4].Replace("\"", string.Empty); ;
            report.Amount = values[5].Replace("\"", string.Empty); ;
            report.TRANSACTION_TIME = values[6].Replace("\"", string.Empty); ;
            report.ORIGINATOR_INSTITUTION = values[7].Replace("\"", string.Empty); ;
            report.ORIGINATOR = values[8].Replace("\"", string.Empty); ;
            report.DESTINATION_INSTITUTION = values[9].Replace("\"", string.Empty); ;
            report.DESTINATION_ACCOUNT_NAME = values[10].Replace("\"", string.Empty); ;
            report.DESTINATION_ACCOUNT_NO = values[11].Replace("\"", string.Empty); ;
            report.NARRATION = values[12].Replace("\"", string.Empty); ;
            report.PAYMENT_REFERENCE = values[13].Replace("\"", string.Empty); ;
            report.LAST_12_DIGITS_OF_SESSION_ID = values[14].Replace("\"", string.Empty); ;
            report.IsProcessed = false;
            report.EntryDate = DateTime.UtcNow;
            report.IsCredited = false;
            report.ProcessedDate = DateTime.UtcNow;
            return report;
        }

        private OutwardReport GetOutwardReportFromFile(string uploadValues, string batchId)
        {
            var report = new OutwardReport();
            var values = uploadValues.Split(",");
            if (values.Length != 15)
            {
                return new OutwardReport();
            }

            report.BatchId = batchId;
            report.CHANNEL = values[1];
            report.SESSION_ID = values[2];
            report.TRANSACTION_TYPE = values[3];
            report.RESPONSE = values[4];
            report.Amount = values[5];
            report.TRANSACTION_TIME = values[6];
            report.ORIGINATOR_INSTITUTION = values[7];
            report.ORIGINATOR = values[8];
            report.DESTINATION_INSTITUTION = values[9];
            report.DESTINATION_ACCOUNT_NAME = values[10];
            report.DESTINATION_ACCOUNT_NO = values[11];
            report.NARRATION = values[12];
            report.PAYMENT_REFERENCE = values[13];
            report.LAST_12_DIGITS_OF_SESSION_ID = values[14];
            report.IsProcessed = false;
            report.EntryDate = DateTime.UtcNow;
            report.IsDebited = false;
            report.ProcessedDate = DateTime.UtcNow;
            return report;
        }
    }
}

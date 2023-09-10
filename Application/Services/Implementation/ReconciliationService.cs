using Domain;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReconciliationReport.Common;
using ReconciliationReport.Data;
using ReconciliationReport.Entities;
using ReconciliationReport.Services.Interface;
using System.IO;
using System.Text;

namespace ReconciliationReport.Services.Implementation;

public class ReconciliationService : IReconciliationService
{
    private readonly ILogger<ReconciliationService> _logger;
    private readonly ReconciliationContext _context;
    private readonly NipContext nipContext;
    private readonly IServiceScopeFactory scopeFactory;
    private readonly IHttpClientFactory clientFactory;
    private readonly IConfiguration config;

    public ReconciliationService(ILogger<ReconciliationService> logger, ReconciliationContext context, IServiceScopeFactory scopeFactory, IHttpClientFactory clientFactory, IConfiguration config, NipContext nipContext)
    {
        _logger = logger;
        _context = context;
        this.scopeFactory = scopeFactory;
        this.clientFactory = clientFactory;
        this.config = config;
        this.nipContext = nipContext;
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
            using var dbContext = scopeFactory.CreateScope();
            var context = dbContext.ServiceProvider.GetRequiredService<ReconciliationContext>();
            var inwardReports = context.InwardReports
                                .Where(x => x.IsProcessed == false)
                                .AsParallel()
                                .AsOrdered()
                                .ToList();

            var parallel = Parallel.ForEach(inwardReports, async item =>
            {
                if (item.SESSION_ID[0].Equals("'"))
                {
                    item.SESSION_ID.Replace("'", string.Empty);
                }

                using var dbContext = scopeFactory.CreateScope();
                var context = dbContext.ServiceProvider.GetRequiredService<NipContext>();
                var inwardTransaction = await context.NIPInboundTransactions.Where(x => x.SessionId == item.SESSION_ID).FirstOrDefaultAsync();
                if (inwardTransaction == null)
                {
                    item.IsProcessed = true;
                    item.TransactionExist = false;
                    return;
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
                        item.Remark = "Transaction not touched";

                        await context.SaveChangesAsync();
                    }

                    else
                    {
                        if (item.RESPONSE == "Approved or Completed Successfully")
                        {
                            // credit transaction
                            var creditCustomer = await CreditTransaction(item);
                            if (creditCustomer)
                            {
                                item.TransactionExist = true;
                                item.IsProcessed = true;
                                item.IsCredited = true;
                                item.Remark = "Transaction was creditted because it was successful from Nibbs";
                            }
                            else
                            {
                                item.TransactionExist = true;
                                item.IsProcessed = false;
                                item.IsCredited = false;
                            }
                        }
                    }
                }
                else
                {
                    item.IsProcessed = true;
                    item.TransactionExist = true;
                    item.IsCredited = false;

                    await context.SaveChangesAsync();
                }

            });

            if (parallel.IsCompleted)
            {
                _logger.LogInformation("Completed all inward transactions for the day");
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
            using var dbContext = scopeFactory.CreateScope();
            var context = dbContext.ServiceProvider.GetRequiredService<ReconciliationContext>();
            var outwardReports = context.OutwardReports
                                .Where(x => x.IsProcessed == false)
                                .AsParallel()
                                .AsOrdered()
                                .ToList();

            var parallel = Parallel.ForEach(outwardReports, async item =>
            {
                try
                {
                    if (item.SESSION_ID[0].Equals("'"))
                    {
                        item.SESSION_ID.Replace("'", string.Empty);
                    }

                    using var dbContext = scopeFactory.CreateScope();
                    var context = dbContext.ServiceProvider.GetRequiredService<NipContext>();

                    var outwardTransaction = await context.NIPOutwardTransactions.Where(x => x.SessionID == item.SESSION_ID).FirstOrDefaultAsync();
                    if (outwardTransaction == null)
                    {
                        item.IsProcessed = true;
                        item.TransactionExist = false;
                        return;
                    }
                    else if (outwardTransaction is not null)
                    {
                        if (outwardTransaction.KafkaStatus == "Processed")
                        {
                            item.TransactionExist = true;
                            item.IsProcessed = true;
                            item.IsDebited = true;
                            item.Remark = "Transaction not touched";

                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            if (item.RESPONSE == "Approved or Completed Successfully")
                            {
                                // debit transaction
                                var debitTransaction = await DebitTransaction(outwardTransaction);
                                if (debitTransaction)
                                {
                                    item.TransactionExist = true;
                                    item.IsProcessed = true;
                                    item.IsDebited = true;
                                    item.Remark = "Transaction was debitted because it was successful from Nibbs";
                                }
                                else
                                {
                                    item.TransactionExist = true;
                                    item.IsProcessed = false;
                                    item.IsDebited = false;
                                }
                            }
                            else
                            {
                                var isReversed = await ReverseTransaction(outwardTransaction);
                                if (isReversed)
                                {
                                    item.TransactionExist = true;
                                    item.IsProcessed = true;
                                    item.IsDebited = false;
                                    item.Remark = $"Transaction not successfully and the money has been Reverseed ";
                                }
                            }
                        }
                    }
                    else
                    {
                        item.IsProcessed = true;
                        item.TransactionExist = true;
                        item.IsDebited = false;

                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return;
                }
            });
            if (parallel.IsCompleted)
            {
                _logger.LogInformation("Completed all outward transactions for the day");
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

    private async Task<bool> ReverseTransaction(NIPOutwardTransaction transaction)
    {
        try
        {
            var url = config["FundTransfer:ReversalUrl"];
            var payload = new ReversalRequest
            {
                BranchCode = transaction.BranchCode,
                FtReference = transaction.PaymentReference,
                ApplicationName = "NIP Reconciler"
            };

            _logger.LogInformation($"Reversal Request : \t{JsonConvert.SerializeObject(payload)}");
            var token = await GetAccessToken();
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var client = clientFactory.CreateClient();
            var response = await client.SendAsync(requestMessage);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var reversalResponse = JsonConvert.DeserializeObject<ReversalResponse>(result);
                if (reversalResponse.Content.ResponseCode == "1" && reversalResponse.IsSuccess)
                {
                    _logger.LogInformation($"Reversal Response : \t {result}");
                    return true;
                }
                else
                {
                    _logger.LogError($"Reversal Response : \t {result}");
                    return false;
                }
            }
            _logger.LogError($"Reversal Response : \t {result}");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }

    }

    private async Task<bool> DebitTransaction(NIPOutwardTransaction transaction)
    {
        try
        {
            var url = config["FundTransfer:SingleCallDebit"];
            var payload = new DebitRequest
            {
                ChannelID = transaction.AppId.Value,
                CreditCurrency = transaction.CurrencyCode,
                DebitCurrency = transaction.CurrencyCode,
                PrincipalDebitAccount = transaction.DebitAccountNumber,//"NGN1250100062001",
                PrincipalCreditAccount = transaction.CreditAccountNumber,
                TransactionNarration = "Reconciliation Reversal",
                FeeAmount = 10,
                PrincipalAmount = transaction.Amount.Value,
                TransactionDebitType = transaction.OutwardTransactionType.Value,
                TransactionReference = transaction.PaymentReference,
                VatAmount = 10,
                VatCreditAccount = transaction.CreditAccountNumber,
                VatDebitAccount = transaction.DebitAccountNumber,
                FtCommissionTypes = "Credit",
                TransactionFeeCode = 10
            };
            _logger.LogInformation($"Debit Transaction Request : \t{JsonConvert.SerializeObject(payload)}");
            var token = await GetAccessToken();
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var client = clientFactory.CreateClient();
            var response = await client.SendAsync(requestMessage);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var reversalResponse = JsonConvert.DeserializeObject<DebitResponse>(result);
                if (reversalResponse?.IsSuccess == true)
                {
                    return true;
                }

            }
            _logger.LogError($"Debit Transaction Response : \t {result}");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, ex.Message);
            return false;
        }

    }
    private async Task<bool> CreditTransaction(InwardReport report)
    {
        try
        {
            var url = config["FundTransfer:SingleCallDebit"];
            var payload = new DebitRequest
            {
                ChannelID = 7,
                CreditCurrency = "NGN",
                DebitCurrency = "NGN",
                PrincipalDebitAccount = "NGN1250100062001", // confifg
                PrincipalCreditAccount = report.DESTINATION_ACCOUNT_NO,
                TransactionNarration = report.NARRATION,
                FeeAmount = 10,
                PrincipalAmount = Convert.ToDecimal(report.Amount),
                TransactionDebitType = 1,
                TransactionReference = report.PAYMENT_REFERENCE,
                VatAmount = 10,
                VatCreditAccount = "NGN1250100062001", // config
                VatDebitAccount = "NGN1250100062001", // config,
                FtCommissionTypes = "Credit",
                TransactionFeeCode = 10
            };
            _logger.LogInformation($"Debit Transaction Request : \t{JsonConvert.SerializeObject(payload)}");
            var token = await GetAccessToken();
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var client = clientFactory.CreateClient();
            var response = await client.SendAsync(requestMessage);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var reversalResponse = JsonConvert.DeserializeObject<DebitResponse>(result);
                if (reversalResponse?.IsSuccess == true)
                {
                    return true;
                }

            }
            _logger.LogError($"Debit Transaction Response : \t {result}");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, ex.Message);
            return false;
        }

    }

    private async Task<string> GetAccessToken()
    {
        try
        {
            var url = config["FundTransfer:TokenUrl"];
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var client = clientFactory.CreateClient();
            var response = await client.SendAsync(requestMessage);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var resultObject = JsonConvert.DeserializeObject<AccessTokenResponse>(result);
                if (resultObject?.Content is null || string.IsNullOrEmpty(resultObject.Content.BearerToken))
                {
                    _logger.LogError(resultObject?.ErrorMessage);
                    return string.Empty;
                }

                return resultObject.Content.BearerToken;
            }

            _logger.LogInformation($"Access token Error Response : \t {result}");
            return string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return string.Empty;
        }
    }
}

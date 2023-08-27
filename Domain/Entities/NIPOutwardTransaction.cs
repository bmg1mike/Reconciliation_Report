using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;

[Table("tbl_NIPOutwardTransactions")]
public class NIPOutwardTransaction
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public long ID { get; set; }
    public string? SessionID { get; set; }
    public string? NameEnquirySessionID { get; set; }
    public string? TransactionCode { get; set; }
    public byte? ChannelCode { get; set; }
    public string? PaymentReference { get; set; }
    public decimal? Amount { get; set; }
    public string? CreditAccountName { get; set; }
    public string? CreditAccountNumber { get; set; }
    public string? OriginatorName { get; set; }
    public string? BranchCode { get; set; }
    public string? CustomerID { get; set; }
    public string? CurrencyCode { get; set; }
    public string? LedgerCode { get; set; }
    public string? SubAccountCode { get; set; }
    public string? FundsTransferResponse { get; set; }
    public DateTime? DateAdded { get; set; }
    public string? DebitRequeryStatus { get; set; }
    public string? NIBSSRequeryStatus { get; set; }
    public string? NameEnquiryResponse { get; set; }
    public DateTime? LastUpdate { get; set; }
    public string? ReversalStatus { get; set; }
    public byte? DebitResponse { get; set; }
    public int? FTAdvice { get; set; }
    public DateTime? FTAdviceDate { get; set; }
    public string? DebitAccountNumber { get; set; }
    public string? BeneficiaryBankCode { get; set; }
    public string? PrincipalResponse { get; set; }
    public string? FeeResponse { get; set; }
    public string? VatResponse { get; set; }
    public string? AccountStatus { get; set; }
    public string? Restriction { get; set; }
    public int? StatusFlag { get; set; }
    public string? FraudResponse { get; set; }
    public string? FraudRequeryResponse { get; set; }
    public string? FraudScore { get; set; }
    public string? OriginatorBVN { get; set; }
    public string? OriginatorEmail { get; set; }
    public string? BeneficiaryBVN { get; set; }
    public string? BeneficiaryKYCLevel { get; set; }
    public string? OriginatorKYCLevel { get; set; }
    public string? TransactionLocation { get; set; }
    public int? AppId { get; set; }
    public DateTime? DebitServiceRequestTime { get; set; }
    public DateTime? DebitServiceResponseTime { get; set; }
    public string? KafkaStatus { get; set; }
    public int? PriorityLevel { get; set; }
    public string? NIBSSResponse { get; set; }
    public int? OutwardTransactionType { get; set; }
    public int? AppsTransactionType { get; set; }
    public bool IsWalletTransaction { get; set; }
    public string? WalletAccountNumber { get; set; }
    public bool IsImalTransaction { get; set; }
    public int? DebitRequeryCounter { get; set; }
    public int? PrincipalResponseReversed { get; set; }
    public int? FeeResponseReversed { get; set; }
    public int? VatResponseReversed { get; set; }
    public string? FraudAnalyticsReferenceId { get; set; }
}


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("NIPInboundTransactionTb")]
    public class NIPInboundTransaction
    {
        [Key]
        public long RefId { get; set; }
        [Column("Transnature", TypeName = "tinyint")]
        public int? Transnature { get; set; }
        public string? BatchNumber { get; set; }
        public string? PaymentRef { get; set; }
        public string? MandateRefNum { get; set; }
        public string? BillerID { get; set; }
        public string? BillerName { get; set; }
        public string? SenderBank { get; set; }
        public string? SenderAccount { get; set; }
        public string? SenderName { get; set; }
        [Column("Amount", TypeName ="decimal(18,2)")]
        public decimal? Amount { get; set; }
        [Column("Feecharge",TypeName ="decimal(18,2)")]
        public decimal? Feecharge { get; set; }
        public string? BranchCode { get; set; }
        public string? CustomerNumber { get; set; }
        public string? CurrencyCode { get; set; }
        public string? LedCode { get; set; }
        public string? SubAccountCode { get; set; }
        public string? BeneficiaryAccountName { get; set; }
        public string? Remark { get; set; }
        public DateTime? ApprovedDate { get; set; }
        [Column("Approvevalue",TypeName ="tinyint")]
        public int? Approvevalue { get; set; }
        public string? TransactionType { get; set; }
        public string? ResponseCode { get; set; }
        public string? ResponseMessage { get; set; }
        public int? FTadvice { get; set; }
        public DateTime? FTadviceDate { get; set; }
        public int? TransactionProcessed { get; set; }
        public DateTime? TransactionProcessedDate { get; set; }
        public string? ReversalStatus { get; set; }
        public string? TransactionStatus { get; set; }
        public int? RepairStatus { get; set; }
        public int? ExceedThreshold { get; set; }
        public DateTime? ExceedThresholddate { get; set; }
        public int? CountTrasactionFreq { get; set; }
        public string? Errcode { get; set; }
        public string? Requery { get; set; }
        public DateTime? RequeryTimeSent { get; set; }
        public DateTime? RequeryTimeReceived { get; set; }
        public int? InwardType { get; set; }
        public string? NameEnquiryRef { get; set; }
        public string? BeneficiaryBankVerificationNumber { get; set; }
        public string? OriginatorAccountNumber { get; set; }
        public string? OriginatorBankVerificationNumber { get; set; }
        public int? OriginatorKYCLevel { get; set; }
        public string? TransactionLocation { get; set; }
        [Column("SettleFlag",TypeName ="tinyint")]
        public int? SettleFlag { get; set; }
        [Column("SettleDiff", TypeName ="decimal(18,2)")]
        public decimal? SettleDiff { get; set; }
        public DateTime? SettleDate { get; set; }
        public string? SettleRemark { get; set; }
        public DateTime? SwitchDate { get; set; }
        public string? VtellerPrinRsp { get; set; }
        public string? FeeRsp { get; set; }
        public string? VatRsp { get; set; }
        public DateTime? OriginalInputdate { get; set; }
        public int? Statusflag { get; set; }
        public string? AccountStatus { get; set; }
        public string? Restriction { get; set; }
        public string? AccountDescp { get; set; }
        public DateTime? DateReactivated { get; set; }
        [Column("BalanceAtTransTime", TypeName ="decimal(18,2)")]
        public decimal? BalanceAtTransTime { get; set; }
        public int? ReasonFlag { get; set; }
        public string? SendersEmail { get; set; }
        public string? SendersMobile { get; set; }
        public string? SendersMobile2 { get; set; }
        public bool? IsSterlingCustomer { get; set; }
        public int? PickedStatus { get; set; }
        public int? IsSent { get; set; }
        public string? WalletAcct { get; set; }
        public int? WalletAcctStatus { get; set; }
        public string? ThirdpartyNuban { get; set; }
        public int? ThirdpartyID { get; set; }
        [Column("PenalAmt", TypeName ="decimal(18,0)")]
        public decimal? PenalAmt { get; set; }
        public int? GivingNg { get; set; }
        [Column("GivingNgAmount", TypeName ="decimal(18,2)")]
        public decimal? GivingNgAmount { get; set; }
        public string? FTReference { get; set; }
        public string? PAPSSResponseCode { get; set; }

        [Column("ChannelCode", TypeName = "tinyint")]
        public int ChannelCode { get; set; }
        [Column("SessionId", TypeName = "varchar")]
        public string? SessionId { get; set; }
        [Column("StaggingStatus")]
        public int StaggingStatus { get; set; }
        [Column("SenderBankCode", TypeName ="char")]
        public string? SenderBankCode { get; set; }
        [Column("TSQcount")]
        public int TSQcount { get; set; }
        [Column("BeneficiaryAccountNumber")]
        public string? BeneficiaryAccountNumber { get; set; }
        [Column("InputDate")]
        public DateTime InputDate { get; set; }
    }
}
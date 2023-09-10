using System.ComponentModel.DataAnnotations;

namespace ReconciliationReport.Entities
{
    public class OutwardReport
    {
        [Key]
        public long Id { get; set; } 
        public string? BatchId { get; set; }
        public string? CHANNEL { get; set; }
        public string? SESSION_ID { get; set; }
        public string? TRANSACTION_TYPE { get; set; }
        public string? RESPONSE { get; set; }
        public string? Amount { get; set; }
        public string? TRANSACTION_TIME { get; set; }
        public string? ORIGINATOR_INSTITUTION { get; set; }
        public string? ORIGINATOR { get; set; }
        public string? DESTINATION_INSTITUTION { get; set; }
        public string? DESTINATION_ACCOUNT_NAME { get; set; }
        public string? DESTINATION_ACCOUNT_NO { get; set; }
        public string? NARRATION { get; set; }
        public string? PAYMENT_REFERENCE { get; set; }
        public string? LAST_12_DIGITS_OF_SESSION_ID { get; set; }
        public DateTime? EntryDate { get; set; } = null;
        public bool? IsDebited { get; set; } = null;
        public bool IsProcessed { get; set; } = false;
        public DateTime? ProcessedDate { get; set; } = null;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateModified { get; set; } = DateTime.UtcNow;
        public string? CoreBankingReference { get; set; }
        public bool? TransactionExist { get; set; }
        public string Remark { get; set; }
    }
}

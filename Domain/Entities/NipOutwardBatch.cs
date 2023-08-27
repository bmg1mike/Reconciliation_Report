using System.ComponentModel.DataAnnotations;

namespace ReconciliationReport.Entities
{
    public class NipOutwardBatch
    {

        [Key]
        public string? BatchId { get; set; }
        public string? FileName { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateModified { get; set; } = DateTime.UtcNow;
    }
}

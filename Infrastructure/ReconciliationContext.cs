using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ReconciliationReport.Entities;

namespace ReconciliationReport.Data
{
    public class ReconciliationContext : DbContext
    {
        public DbSet<InwardReport> InwardReports { get; set; } = null!;
        public DbSet<OutwardReport> OutwardReports { get; set; } = null!;
        public DbSet<NipInwardBatch> NipInwardBatches { get; set; } = null!;
        public DbSet<NipOutwardBatch> NipOutwardBatches { get; set; } = null!;

        public ReconciliationContext(DbContextOptions<ReconciliationContext> options):base(options)
        {
            
        }

    }
}

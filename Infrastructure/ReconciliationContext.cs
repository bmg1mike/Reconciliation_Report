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
        public DbSet<NIPOutwardTransaction> NIPOutwardTransactions { get; set; } = null!;
        public DbSet<NIPInboundTransaction> NIPInboundTransactions { get; set; } = null!;
        public ReconciliationContext(DbContextOptions options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NIPOutwardTransaction>()
                        .ToTable("tbl_NIPOutwardTransactions",t => t.ExcludeFromMigrations());

            modelBuilder.Entity<NIPInboundTransaction>()
                        .ToTable("NIPInboundTransactionTb",t => t.ExcludeFromMigrations());
            base.OnModelCreating(modelBuilder);
        }

    }
}

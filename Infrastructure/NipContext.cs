using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class NipContext : DbContext
{
    public DbSet<NIPOutwardTransaction> NIPOutwardTransactions { get; set; } = null!;
    public DbSet<NIPInboundTransaction> NIPInboundTransactions { get; set; } = null!;
    public NipContext(DbContextOptions<NipContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NIPOutwardTransaction>()
                    .ToTable("tbl_NIPOutwardTransactions", t => t.ExcludeFromMigrations());

        modelBuilder.Entity<NIPInboundTransaction>()
                    .ToTable("NIPInboundTransactionTb", t => t.ExcludeFromMigrations());
        base.OnModelCreating(modelBuilder);
    }
}
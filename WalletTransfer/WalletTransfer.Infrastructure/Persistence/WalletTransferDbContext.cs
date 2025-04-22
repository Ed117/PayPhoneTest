using Microsoft.EntityFrameworkCore;
using WalletTransfer.Domain.Entities;

namespace WalletTransfer.Infrastructure.Persistence
{
    public class WalletTransferDbContext : DbContext
    {
        public WalletTransferDbContext(DbContextOptions<WalletTransferDbContext> opts)
            : base(opts) { }

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<TransactionHistory> TransactionHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplica todas las configuraciones de entidad en este ensamblado
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WalletTransferDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}

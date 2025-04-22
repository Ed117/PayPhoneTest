using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WalletTransfer.Domain.Entities;

namespace WalletTransfer.Infrastructure.Persistence.Configurations
{
    public class TransactionHistoryConfiguration : IEntityTypeConfiguration<TransactionHistory>
    {
        public void Configure(EntityTypeBuilder<TransactionHistory> builder)
        {
            builder.ToTable("TransactionHistory");

            builder.HasKey(th => th.Id);

            builder.Property(th => th.Amount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(th => th.Type)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(th => th.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasOne(th => th.Wallet)
                   .WithMany(w => w.TransactionHistories)
                   .HasForeignKey(th => th.WalletId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

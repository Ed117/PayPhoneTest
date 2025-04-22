using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WalletTransfer.Domain.Entities;

namespace WalletTransfer.Infrastructure.Persistence.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallet");
            builder.HasKey(w => w.Id);
            builder.Property(w => w.DocumentId)
                   .HasMaxLength(50)
                   .IsRequired();
            builder.Property(w => w.OwnerName)
                   .HasMaxLength(100)
                   .IsRequired();
            builder.Property(w => w.Balance)
                   .HasColumnType("decimal(18,2)")
                   .HasDefaultValue(0m);
            builder.Property(w => w.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            builder.Property(w => w.UpdatedAt)
                   .ValueGeneratedOnAddOrUpdate()
                   .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            builder.HasMany(w => w.TransactionHistories)
                   .WithOne(th => th.Wallet)
                   .HasForeignKey(th => th.WalletId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

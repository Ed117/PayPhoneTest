using Microsoft.EntityFrameworkCore;
using WalletTransfer.Infrastructure.Persistence;
using WalletTransfer.Infrastructure.Persistence.Repositories;
using WalletTransfer.Domain.Entities;

namespace WalletTransfer.Infrastructure.Tests.WalletRepositories
{
    public class GetByIdAsyncTests : IDisposable
    {
        private readonly WalletTransferDbContext _context;
        private readonly WalletRepository _repo;

        public GetByIdAsyncTests()
        {
            var options = new DbContextOptionsBuilder<WalletTransferDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new WalletTransferDbContext(options);
            _repo = new WalletRepository(_context);
        }

        [Fact]
        public async Task ReturnsNull_WhenWalletDoesNotExist()
        {
            // Act
            var result = await _repo.GetByIdAsync(123);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ReturnsWalletWithHistories_WhenExists()
        {
            // Arrange
            var wallet = new Wallet { DocumentId = "DX", OwnerName = "Z", Balance = 5m };
            await _repo.AddAsync(wallet);
            await _repo.SaveChangesAsync();
            // EF Core won't auto-load histories, so manually add
            var history = new TransactionHistory { WalletId = wallet.Id, Amount = 5m, Type = TransactionType.Credit };
            _context.TransactionHistories.Add(history);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetByIdAsync(wallet.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(wallet.Id, result.Id);
            Assert.NotNull(result.TransactionHistories);
            Assert.Single(result.TransactionHistories);
            Assert.Equal(5m, result.TransactionHistories.First().Amount);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

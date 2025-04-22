using Microsoft.EntityFrameworkCore;
using WalletTransfer.Infrastructure.Persistence;
using WalletTransfer.Infrastructure.Persistence.Repositories;
using WalletTransfer.Domain.Entities;

namespace WalletTransfer.Infrastructure.Tests.TransactionHistoryRepositories
{
    public class GetByIdAsyncTests : IDisposable
    {
        private readonly WalletTransferDbContext _context;
        private readonly TransactionHistoryRepository _repo;

        public GetByIdAsyncTests()
        {
            var options = new DbContextOptionsBuilder<WalletTransferDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new WalletTransferDbContext(options);
            _repo = new TransactionHistoryRepository(_context);
        }

        [Fact]
        public async Task ReturnsNull_WhenHistoryDoesNotExist()
        {
            // Act
            var result = await _repo.GetByIdAsync(123);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ReturnsHistory_WithCorrectFields_WhenExists()
        {
            // Arrange
            var wallet = new Wallet { DocumentId = "D2", OwnerName = "Y", Balance = 0m };
            await _context.Wallets.AddAsync(wallet);
            await _context.SaveChangesAsync();

            var history = new TransactionHistory
            {
                WalletId = wallet.Id,
                Amount = 20m,
                Type = TransactionType.Debit,
                CreatedAt = DateTime.UtcNow
            };
            await _context.TransactionHistories.AddAsync(history);
            await _context.SaveChangesAsync();

            // Act
            var fetched = await _repo.GetByIdAsync(history.Id);

            // Assert
            Assert.NotNull(fetched);
            Assert.Equal(history.Id, fetched.Id);
            Assert.Equal(history.WalletId, fetched.WalletId);
            Assert.Equal(history.Amount, fetched.Amount);
            Assert.Equal(history.Type, fetched.Type);
        }

        public void Dispose() => _context.Dispose();
    }
}

using Microsoft.EntityFrameworkCore;
using WalletTransfer.Infrastructure.Persistence;
using WalletTransfer.Infrastructure.Persistence.Repositories;
using WalletTransfer.Domain.Entities;

namespace WalletTransfer.Infrastructure.Tests.TransactionHistoryRepositories
{
    public class AddAsyncTests : IDisposable
    {
        private readonly WalletTransferDbContext _context;
        private readonly TransactionHistoryRepository _repo;

        public AddAsyncTests()
        {
            var options = new DbContextOptionsBuilder<WalletTransferDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new WalletTransferDbContext(options);
            _repo = new TransactionHistoryRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddHistory_WhenCalled()
        {
            // Arrange
            var wallet = new Wallet { DocumentId = "D1", OwnerName = "X", Balance = 0m };
            await _context.Wallets.AddAsync(wallet);
            await _context.SaveChangesAsync();
            var history = new TransactionHistory
            {
                WalletId = wallet.Id,
                Amount = 15m,
                Type = TransactionType.Credit,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            await _repo.AddAsync(history);
            await _repo.SaveChangesAsync();
            var list = await _context.TransactionHistories.ToListAsync();

            // Assert
            Assert.Single(list);
            var added = list.First();
            Assert.Equal(history.WalletId, added.WalletId);
            Assert.Equal(history.Amount, added.Amount);
            Assert.Equal(history.Type, added.Type);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowArgumentNullException_WhenHistoryIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repo.AddAsync(null));
        }

        public void Dispose() => _context.Dispose();
    }
}

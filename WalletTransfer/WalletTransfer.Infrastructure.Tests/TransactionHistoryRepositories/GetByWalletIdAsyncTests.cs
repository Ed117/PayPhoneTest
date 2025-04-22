using Microsoft.EntityFrameworkCore;
using WalletTransfer.Infrastructure.Persistence;
using WalletTransfer.Infrastructure.Persistence.Repositories;
using WalletTransfer.Domain.Entities;

namespace WalletTransfer.Infrastructure.Tests.TransactionHistoryRepositories
{
    public class GetByWalletIdAsyncTests : IDisposable
    {
        private readonly WalletTransferDbContext _context;
        private readonly TransactionHistoryRepository _repo;

        public GetByWalletIdAsyncTests()
        {
            var options = new DbContextOptionsBuilder<WalletTransferDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new WalletTransferDbContext(options);
            _repo = new TransactionHistoryRepository(_context);
        }

        [Fact]
        public async Task ReturnsEmpty_WhenNoHistoriesForWallet()
        {
            // Act
            var list = await _repo.GetByWalletIdAsync(99);

            // Assert
            Assert.NotNull(list);
            Assert.Empty(list);
        }

        [Fact]
        public async Task ReturnsOnlyMatchingHistories_WhenMultipleExist()
        {
            // Arrange
            var w1 = new Wallet { DocumentId = "D1", OwnerName = "A", Balance = 0m };
            var w2 = new Wallet { DocumentId = "D2", OwnerName = "B", Balance = 0m };
            await _context.Wallets.AddAsync(w1);
            await _context.Wallets.AddAsync(w2);
            await _context.SaveChangesAsync();

            var h1 = new TransactionHistory { WalletId = w1.Id, Amount = 5m, Type = TransactionType.Credit };
            var h2 = new TransactionHistory { WalletId = w2.Id, Amount = 7m, Type = TransactionType.Debit };
            var h3 = new TransactionHistory { WalletId = w1.Id, Amount = 3m, Type = TransactionType.Debit };
            await _context.TransactionHistories.AddRangeAsync(h1, h2, h3);
            await _context.SaveChangesAsync();

            // Act
            var list = await _repo.GetByWalletIdAsync(w1.Id);

            // Assert
            Assert.Equal(2, list.Count());
            Assert.All(list, th => Assert.Equal(w1.Id, th.WalletId));
            Assert.Contains(list, th => th.Amount == 5m);
            Assert.Contains(list, th => th.Amount == 3m);
        }

        public void Dispose() => _context.Dispose();
    }
}
using Microsoft.EntityFrameworkCore;
using WalletTransfer.Infrastructure.Persistence;
using WalletTransfer.Infrastructure.Persistence.Repositories;
using WalletTransfer.Domain.Entities;

namespace WalletTransfer.Infrastructure.Tests.WalletRepositories
{
    public class GetAllAsyncTests : IDisposable
    {
        private readonly WalletTransferDbContext _context;
        private readonly WalletRepository _repo;

        public GetAllAsyncTests()
        {
            var options = new DbContextOptionsBuilder<WalletTransferDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new WalletTransferDbContext(options);
            _repo = new WalletRepository(_context);
        }

        [Fact]
        public async Task ReturnsEmpty_WhenNoWallets()
        {
            // Act
            var result = await _repo.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ReturnsAllWallets_WhenTheyExist()
        {
            // Arrange
            var w1 = new Wallet { DocumentId = "D1", OwnerName = "A", Balance = 10m };
            var w2 = new Wallet { DocumentId = "D2", OwnerName = "B", Balance = 20m };
            await _repo.AddAsync(w1);
            await _repo.AddAsync(w2);
            await _repo.SaveChangesAsync();

            // Act
            var list = await _repo.GetAllAsync();

            // Assert
            Assert.Equal(2, list.Count());
            Assert.Contains(list, w => w.DocumentId == "D1" && w.OwnerName == "A");
            Assert.Contains(list, w => w.DocumentId == "D2" && w.OwnerName == "B");
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

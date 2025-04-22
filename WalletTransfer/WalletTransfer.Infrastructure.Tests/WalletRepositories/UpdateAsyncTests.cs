using Microsoft.EntityFrameworkCore;
using WalletTransfer.Infrastructure.Persistence;
using WalletTransfer.Infrastructure.Persistence.Repositories;
using WalletTransfer.Domain.Entities;

namespace WalletTransfer.Infrastructure.Tests.WalletRepositories
{
    public class UpdateAsyncTests : IDisposable
    {
        private readonly WalletTransferDbContext _context;
        private readonly WalletRepository _repo;

        public UpdateAsyncTests()
        {
            var options = new DbContextOptionsBuilder<WalletTransferDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new WalletTransferDbContext(options);
            _repo = new WalletRepository(_context);
        }

        [Fact]
        public async Task UpdatesEntity_WhenCalled()
        {
            // Arrange
            var wallet = new Wallet { DocumentId = "D5", OwnerName = "Old", Balance = 10m };
            await _repo.AddAsync(wallet);
            await _repo.SaveChangesAsync();

            // Act
            wallet.OwnerName = "New";
            wallet.Balance = 15m;
            await _repo.UpdateAsync(wallet);
            await _repo.SaveChangesAsync();

            var fromDb = await _repo.GetByIdAsync(wallet.Id);

            // Assert
            Assert.Equal("New", fromDb.OwnerName);
            Assert.Equal(15m, fromDb.Balance);
        }

        [Fact]
        public async Task ThrowsArgumentNullException_WhenWalletIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repo.UpdateAsync(null));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using WalletTransfer.Infrastructure.Persistence;
using WalletTransfer.Infrastructure.Persistence.Repositories;
using WalletTransfer.Domain.Entities;

namespace WalletTransfer.Infrastructure.Tests.WalletRepositories
{
    public class DeleteAsyncTests : IDisposable
    {
        private readonly WalletTransferDbContext _context;
        private readonly WalletRepository _repo;

        public DeleteAsyncTests()
        {
            var options = new DbContextOptionsBuilder<WalletTransferDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new WalletTransferDbContext(options);
            _repo = new WalletRepository(_context);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEntity_WhenItExists()
        {
            // Arrange
            var wallet = new Wallet { DocumentId = "D200", OwnerName = "ToDelete", Balance = 0m };
            await _repo.AddAsync(wallet);
            await _repo.SaveChangesAsync();

            // Act
            await _repo.DeleteAsync(wallet);
            await _repo.SaveChangesAsync();
            var list = await _repo.GetAllAsync();

            // Assert
            Assert.Empty(list);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowArgumentNullException_WhenWalletIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repo.DeleteAsync(null));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

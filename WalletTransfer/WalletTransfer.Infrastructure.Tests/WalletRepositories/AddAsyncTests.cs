using Microsoft.EntityFrameworkCore;
using WalletTransfer.Infrastructure.Persistence;
using WalletTransfer.Infrastructure.Persistence.Repositories;
using WalletTransfer.Domain.Entities;

namespace WalletTransfer.Infrastructure.Tests.WalletRepositories
{
    public class AddAsyncTests : IDisposable
    {
        private readonly WalletTransferDbContext _context;
        private readonly WalletRepository _repo;

        public AddAsyncTests()
        {
            var options = new DbContextOptionsBuilder<WalletTransferDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new WalletTransferDbContext(options);
            _repo = new WalletRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEntity_WhenCalled()
        {
            // Arrange
            var wallet = new Wallet { DocumentId = "D100", OwnerName = "Test User", Balance = 250m };

            // Act
            await _repo.AddAsync(wallet);
            await _repo.SaveChangesAsync();
            var list = await _repo.GetAllAsync();

            // Assert
            Assert.Single(list);
            var added = list.First();
            Assert.Equal("D100", added.DocumentId);
            Assert.Equal("Test User", added.OwnerName);
            Assert.Equal(250m, added.Balance);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowArgumentNullException_WhenWalletIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repo.AddAsync(null));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

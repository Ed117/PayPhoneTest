using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using WalletTransfer.Application.DTOs;
using WalletTransfer.Application.Services;
using WalletTransfer.Domain.Entities;
using WalletTransfer.Domain.Interfaces;

namespace WalletTransfer.Application.Tests.WalletServices
{
    public class GetAllAsyncTests
    {
        private readonly IWalletRepository _walletRepo = Substitute.For<IWalletRepository>();
        private readonly ITransactionHistoryRepository _historyRepo = Substitute.For<ITransactionHistoryRepository>();
        private readonly WalletService _service;
        private readonly CompareLogic _comparer = new CompareLogic();

        public GetAllAsyncTests()
        {
            _service = new WalletService(_walletRepo, _historyRepo);
        }

        [Fact]
        public async Task ReturnsEmpty_WhenRepositoryReturnsEmpty()
        {
            // Arrange
            _walletRepo.GetAllAsync().Returns(Task.FromResult<IEnumerable<Wallet>>(new List<Wallet>()));

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            await _walletRepo.Received(1).GetAllAsync();
        }

        [Fact]
        public async Task ReturnsMappedDtos_WhenRepositoryReturnsData()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var wallets = new[]
            {
                new Wallet { Id = 1, DocumentId = "D1", OwnerName = "Alice", Balance = 100m, CreatedAt = now, UpdatedAt = now },
                new Wallet { Id = 2, DocumentId = "D2", OwnerName = "Bob",   Balance = 200m, CreatedAt = now.AddHours(-1), UpdatedAt = now.AddHours(-1) }
            };
            _walletRepo.GetAllAsync().Returns(Task.FromResult((IEnumerable<Wallet>)wallets));

            var expected = new List<WalletDto>
            {
                new() { Id = 1, DocumentId = "D1", OwnerName = "Alice", Balance = 100m, CreatedAt = now, UpdatedAt = now },
                new() { Id = 2, DocumentId = "D2", OwnerName = "Bob",   Balance = 200m, CreatedAt = now.AddHours(-1), UpdatedAt = now.AddHours(-1) }
            };

            // Act
            var actual = (await _service.GetAllAsync()).ToList();

            // Assert
            var comparison = _comparer.Compare(expected, actual);
            Assert.True(comparison.AreEqual, comparison.DifferencesString);
            await _walletRepo.Received(1).GetAllAsync();
        }

        [Fact]
        public async Task ThrowsNullReferenceException_WhenRepositoryReturnsNull()
        {
            // Arrange
            _walletRepo.GetAllAsync().Returns(Task.FromResult<IEnumerable<Wallet>>(null));

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _service.GetAllAsync());
            await _walletRepo.Received(1).GetAllAsync();
        }
    }
}
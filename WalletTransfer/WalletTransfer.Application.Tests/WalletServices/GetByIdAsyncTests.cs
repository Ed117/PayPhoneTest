using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using WalletTransfer.Application.DTOs;
using WalletTransfer.Application.Services;
using WalletTransfer.Domain.Entities;
using WalletTransfer.Domain.Interfaces;

namespace WalletTransfer.Application.Tests.WalletServices
{
    public class GetByIdAsyncTests
    {
        private readonly IWalletRepository _walletRepo = Substitute.For<IWalletRepository>();
        private readonly ITransactionHistoryRepository _historyRepo = Substitute.For<ITransactionHistoryRepository>();
        private readonly WalletService _service;
        private readonly CompareLogic _comparer = new CompareLogic();

        public GetByIdAsyncTests()
        {
            _service = new WalletService(_walletRepo, _historyRepo);
        }

        [Fact]
        public async Task ReturnsMappedDto_WhenWalletExists()
        {
            // Arrange
            int id = 5;
            var now = DateTime.UtcNow;
            var wallet = new Wallet
            {
                Id = id,
                DocumentId = "D5",
                OwnerName = "Carol",
                Balance = 500m,
                CreatedAt = now,
                UpdatedAt = now
            };
            _walletRepo.GetByIdAsync(id).Returns(Task.FromResult(wallet));

            var expected = new WalletDto
            {
                Id = id,
                DocumentId = "D5",
                OwnerName = "Carol",
                Balance = 500m,
                CreatedAt = now,
                UpdatedAt = now
            };

            // Act
            var actual = await _service.GetByIdAsync(id);

            // Assert
            var comparison = _comparer.Compare(expected, actual);
            Assert.True(comparison.AreEqual, comparison.DifferencesString);
            await _walletRepo.Received(1).GetByIdAsync(id);
        }

        [Fact]
        public async Task ThrowsKeyNotFoundException_WhenWalletDoesNotExist()
        {
            // Arrange
            int missingId = 999;
            _walletRepo.GetByIdAsync(missingId).Returns(Task.FromResult<Wallet>(null));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdAsync(missingId));
            Assert.Contains(missingId.ToString(), ex.Message);
            await _walletRepo.Received(1).GetByIdAsync(missingId);
        }
    }
}
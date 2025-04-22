using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using WalletTransfer.Application.DTOs;
using WalletTransfer.Application.Services;
using WalletTransfer.Domain.Entities;
using WalletTransfer.Domain.Interfaces;

namespace WalletTransfer.Application.Tests.TransactionHistoryServices
{
    public class GetByWalletIdAsyncTests
    {
        private readonly ITransactionHistoryRepository _repo = Substitute.For<ITransactionHistoryRepository>();
        private readonly TransactionHistoryService _service;
        private readonly CompareLogic _comparer = new();

        public GetByWalletIdAsyncTests()
        {
            _service = new TransactionHistoryService(_repo);
        }

        [Fact]
        public async Task ReturnsEmpty_WhenRepositoryReturnsEmptyList()
        {
            // Arrange
            int walletId = 99;
            _repo.GetByWalletIdAsync(walletId).Returns(Task.FromResult<IEnumerable<TransactionHistory>>(new List<TransactionHistory>()));

            // Act
            var result = await _service.GetByWalletIdAsync(walletId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            await _repo.Received(1).GetByWalletIdAsync(walletId);
        }

        [Fact]
        public async Task MapsAllFields_Correctly()
        {
            // Arrange
            int walletId = 1;
            var now = new DateTime(2025, 4, 22, 12, 0, 0, DateTimeKind.Utc);
            var histories = new[]
            {
                new TransactionHistory { Id = 10, WalletId = walletId, Amount = 100.5m, Type = TransactionType.Credit, CreatedAt = now },
                new TransactionHistory { Id = 20, WalletId = walletId, Amount =  50.0m, Type = TransactionType.Debit,  CreatedAt = now.AddMinutes(5) }
            };
            _repo.GetByWalletIdAsync(walletId).Returns(Task.FromResult((IEnumerable<TransactionHistory>)histories));

            var expected = new List<TransactionHistoryDto>
            {
                new() { Id = 10, WalletId = walletId, Amount = 100.5m, Type = "Credit", CreatedAt = now },
                new() { Id = 20, WalletId = walletId, Amount = 50.0m,  Type = "Debit",  CreatedAt = now.AddMinutes(5) }
            };

            // Act
            var actual = (await _service.GetByWalletIdAsync(walletId)).ToList();

            // Assert
            var comparison = _comparer.Compare(expected, actual);
            Assert.True(comparison.AreEqual, comparison.DifferencesString);
            await _repo.Received(1).GetByWalletIdAsync(walletId);
        }

        [Fact]
        public async Task ThrowsArgumentNullException_WhenRepositoryReturnsNull()
        {
            // Arrange
            int walletId = 5;
            _ = _repo.GetByWalletIdAsync(walletId).Returns(await Task.FromResult<IEnumerable<TransactionHistory>>(null));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.GetByWalletIdAsync(walletId));
            await _repo.Received(1).GetByWalletIdAsync(walletId);
        }
    }
}
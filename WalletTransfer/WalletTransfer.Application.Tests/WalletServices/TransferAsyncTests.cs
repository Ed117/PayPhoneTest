using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using WalletTransfer.Application.Services;
using WalletTransfer.Domain.Entities;
using WalletTransfer.Domain.Interfaces;

namespace WalletTransfer.Application.Tests.WalletServices
{
    public class TransferAsyncTests
    {
        private readonly IWalletRepository _walletRepo = Substitute.For<IWalletRepository>();
        private readonly ITransactionHistoryRepository _historyRepo = Substitute.For<ITransactionHistoryRepository>();
        private readonly WalletService _service;
        private readonly CompareLogic _comparer = new();

        public TransferAsyncTests()
        {
            _service = new WalletService(_walletRepo, _historyRepo);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task ThrowsArgumentException_WhenAmountNotPositive(decimal amount)
        {
            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.TransferAsync(1, 2, amount));
            Assert.Contains("positive", ex.Message);
            await _walletRepo.DidNotReceive().GetByIdAsync(Arg.Any<int>());
        }

        [Fact]
        public async Task ThrowsKeyNotFoundException_WhenFromWalletNotFound()
        {
            // Arrange
            int fromId = 1, toId = 2;
            _walletRepo.GetByIdAsync(fromId).Returns(Task.FromResult<Wallet>(null));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.TransferAsync(fromId, toId, 50m));
            Assert.Contains(fromId.ToString(), ex.Message);
            await _walletRepo.Received(1).GetByIdAsync(fromId);
            await _walletRepo.DidNotReceive().GetByIdAsync(toId);
        }

        [Fact]
        public async Task ThrowsKeyNotFoundException_WhenToWalletNotFound()
        {
            // Arrange
            int fromId = 1, toId = 2;
            var fromWallet = new Wallet { Id = fromId, Balance = 100m, DocumentId = "123", OwnerName = "Paco" };
            _walletRepo.GetByIdAsync(fromId).Returns(Task.FromResult(fromWallet));
            _walletRepo.GetByIdAsync(toId).Returns(Task.FromResult<Wallet>(null));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.TransferAsync(fromId, toId, 50m));
            Assert.Contains(toId.ToString(), ex.Message);
            await _walletRepo.Received(1).GetByIdAsync(fromId);
            await _walletRepo.Received(1).GetByIdAsync(toId);
        }

        [Fact]
        public async Task ThrowsInvalidOperationException_WhenInsufficientFunds()
        {
            // Arrange
            int fromId = 1, toId = 2;
            var fromWallet = new Wallet { Id = fromId, Balance = 30m, DocumentId = "123", OwnerName = "Paco" };
            var toWallet = new Wallet { Id = toId, Balance = 10m, DocumentId = "234", OwnerName = "Lola" };
            _walletRepo.GetByIdAsync(fromId).Returns(Task.FromResult(fromWallet));
            _walletRepo.GetByIdAsync(toId).Returns(Task.FromResult(toWallet));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.TransferAsync(fromId, toId, 50m));
            await _walletRepo.Received(1).GetByIdAsync(fromId);
            await _walletRepo.Received(1).GetByIdAsync(toId);
        }

        [Fact]
        public async Task PerformsTransfer_Correctly_WhenValid()
        {
            // Arrange
            int fromId = 1, toId = 2;
            var fromWallet = new Wallet { Id = fromId, Balance = 100m, DocumentId = "123", OwnerName = "Paco" };
            var toWallet = new Wallet { Id = toId, Balance = 20m, DocumentId = "234", OwnerName = "Lola" };
            _walletRepo.GetByIdAsync(fromId).Returns(Task.FromResult(fromWallet));
            _walletRepo.GetByIdAsync(toId).Returns(Task.FromResult(toWallet));
            decimal amount = 25m;

            // Act
            await _service.TransferAsync(fromId, toId, amount);

            // Assert balances updated
            Assert.Equal(75m, fromWallet.Balance);
            Assert.Equal(45m, toWallet.Balance);

            // Verify history entries created
            await _historyRepo.Received(1).AddAsync(Arg.Is<TransactionHistory>(th =>
                th.WalletId == fromId && th.Amount == amount && th.Type == TransactionType.Debit));
            await _historyRepo.Received(1).AddAsync(Arg.Is<TransactionHistory>(th =>
                th.WalletId == toId && th.Amount == amount && th.Type == TransactionType.Credit));

            // Verify save order
            Received.InOrder(async () =>
            {
                await _walletRepo.SaveChangesAsync();
                await _historyRepo.SaveChangesAsync();
            });
        }

        [Fact]
        public async Task PerformsSelfTransfer_CreatesEntries_ButBalanceUnchanged()
        {
            // Arrange
            int walletId = 3;
            var wallet = new Wallet { Id = walletId, Balance = 50m, DocumentId = "123", OwnerName = "Paco" };
            _walletRepo.GetByIdAsync(walletId).Returns(Task.FromResult(wallet));
            decimal amount = 10m;

            // Act
            await _service.TransferAsync(walletId, walletId, amount);

            // Assert balance unchanged
            Assert.Equal(50m, wallet.Balance);

            // Verify history entries
            await _historyRepo.Received(2).AddAsync(Arg.Any<TransactionHistory>());
            // Verify save
            Received.InOrder(async () =>
            {
                await _walletRepo.SaveChangesAsync();
                await _historyRepo.SaveChangesAsync();
            });
        }
    }
}

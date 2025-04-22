using NSubstitute;
using WalletTransfer.Application.Services;
using WalletTransfer.Domain.Entities;
using WalletTransfer.Domain.Interfaces;

namespace WalletTransfer.Application.Tests.WalletServices
{
    public class DeleteAsyncTests
    {
        private readonly IWalletRepository _walletRepo = Substitute.For<IWalletRepository>();
        private readonly ITransactionHistoryRepository _historyRepo = Substitute.For<ITransactionHistoryRepository>();
        private readonly WalletService _service;

        public DeleteAsyncTests()
        {
            _service = new WalletService(_walletRepo, _historyRepo);
        }

        [Fact]
        public async Task DeletesWallet_WhenItExists()
        {
            // Arrange
            var wallet = new Wallet { Id = 1, DocumentId = "D1", OwnerName = "Test", Balance = 0m };
            _walletRepo.GetByIdAsync(wallet.Id).Returns(Task.FromResult(wallet));

            // Act
            await _service.DeleteAsync(wallet.Id);

            // Assert
            await _walletRepo.Received(1).GetByIdAsync(wallet.Id);
            await _walletRepo.Received(1).DeleteAsync(wallet);
            await _walletRepo.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task ThrowsKeyNotFoundException_WhenWalletDoesNotExist()
        {
            // Arrange
            int missingId = 99;
            _walletRepo.GetByIdAsync(missingId).Returns(Task.FromResult<Wallet>(null));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.DeleteAsync(missingId));
            await _walletRepo.Received(1).GetByIdAsync(missingId);
            await _walletRepo.DidNotReceive().DeleteAsync(Arg.Any<Wallet>());
            await _walletRepo.DidNotReceive().SaveChangesAsync();
        }
    }
}

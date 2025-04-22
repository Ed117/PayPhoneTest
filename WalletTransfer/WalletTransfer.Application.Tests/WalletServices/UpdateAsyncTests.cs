using NSubstitute;
using WalletTransfer.Application.DTOs;
using WalletTransfer.Application.Services;
using WalletTransfer.Domain.Entities;
using WalletTransfer.Domain.Interfaces;

namespace WalletTransfer.Application.Tests.WalletServices
{
    public class UpdateAsyncTests
    {
        private readonly IWalletRepository _walletRepo = Substitute.For<IWalletRepository>();
        private readonly ITransactionHistoryRepository _historyRepo = Substitute.For<ITransactionHistoryRepository>();
        private readonly WalletService _service;

        public UpdateAsyncTests()
        {
            _service = new WalletService(_walletRepo, _historyRepo);
        }

        [Fact]
        public async Task ThrowsNullReferenceException_WhenDtoIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _service.UpdateAsync(null));
            await _walletRepo.DidNotReceive().GetByIdAsync(Arg.Any<int>());
        }

        [Fact]
        public async Task ThrowsKeyNotFoundException_WhenWalletNotFound()
        {
            // Arrange
            var dto = new UpdateWalletDto { Id = 99, DocumentId = "D99", OwnerName = "X" };
            _walletRepo.GetByIdAsync(dto.Id).Returns(Task.FromResult<Wallet>(null));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(dto));
            Assert.Contains(dto.Id.ToString(), ex.Message);
            await _walletRepo.Received(1).GetByIdAsync(dto.Id);
            await _walletRepo.DidNotReceive().UpdateAsync(Arg.Any<Wallet>());
            await _walletRepo.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task UpdatesFields_And_Saves_WhenDtoIsValid()
        {
            // Arrange
            int id = 5;
            var original = new Wallet { Id = id, DocumentId = "D5", OwnerName = "Old", UpdatedAt = DateTime.UtcNow.AddDays(-1) };
            _walletRepo.GetByIdAsync(id).Returns(Task.FromResult(original));
            var dto = new UpdateWalletDto { Id = id, DocumentId = "D5_new", OwnerName = "NewName" };

            // Act
            await _service.UpdateAsync(dto);

            // Assert
            await _walletRepo.Received(1).GetByIdAsync(id);
            Assert.Equal(dto.DocumentId, original.DocumentId);
            Assert.Equal(dto.OwnerName, original.OwnerName);
            Assert.True(original.UpdatedAt > DateTime.UtcNow.AddMinutes(-1));
            await _walletRepo.Received(1).UpdateAsync(original);
            await _walletRepo.Received(1).SaveChangesAsync();
        }
    }
}

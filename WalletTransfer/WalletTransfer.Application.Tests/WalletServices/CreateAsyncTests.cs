using NSubstitute;
using WalletTransfer.Application.Services;
using WalletTransfer.Domain.Entities;
using WalletTransfer.Domain.Interfaces;

namespace WalletTransfer.Application.Tests.WalletServices
{
    public class CreateAsyncTests
    {
        private readonly IWalletRepository _walletRepo = Substitute.For<IWalletRepository>();
        private readonly ITransactionHistoryRepository _historyRepo = Substitute.For<ITransactionHistoryRepository>();
        private readonly WalletService _service;

        public CreateAsyncTests()
        {
            _service = new WalletService(_walletRepo, _historyRepo);
        }

        [Fact]
        public async Task ReturnsNewId_And_PersistsWallet_WhenDtoIsValid()
        {
            // Arrange
            var dto = new CreateWalletDto
            {
                DocumentId = "D1",
                OwnerName = "Alice",
                InitialBalance = 150m
            };
            _walletRepo
                .When(x => x.AddAsync(Arg.Any<Wallet>()))
                .Do(ci => ci.Arg<Wallet>().Id = 100);

            // Act
            var resultId = await _service.CreateAsync(dto);

            // Assert
            Assert.Equal(100, resultId);
            await _walletRepo.Received(1).AddAsync(Arg.Is<Wallet>(w =>
                w.DocumentId == dto.DocumentId &&
                w.OwnerName == dto.OwnerName &&
                w.Balance == dto.InitialBalance &&
                w.CreatedAt <= DateTime.UtcNow &&
                w.UpdatedAt <= DateTime.UtcNow
            ));
            await _walletRepo.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task ThrowsNullReferenceException_WhenDtoIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _service.CreateAsync(null));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public async Task AllowsNonPositiveInitialBalance(decimal initialBalance)
        {
            // Arrange
            var dto = new CreateWalletDto
            {
                DocumentId = "D2",
                OwnerName = "Bob",
                InitialBalance = initialBalance
            };
            _walletRepo
                .When(x => x.AddAsync(Arg.Any<Wallet>()))
                .Do(ci => ci.Arg<Wallet>().Id = 200);

            // Act
            var id = await _service.CreateAsync(dto);

            // Assert
            Assert.Equal(200, id);
            await _walletRepo.Received(1).AddAsync(Arg.Is<Wallet>(w => w.Balance == initialBalance));
        }
    }
}

using WalletTransfer.Application.DTOs;
using WalletTransfer.Application.Interfaces;
using WalletTransfer.Domain.Entities;
using WalletTransfer.Domain.Interfaces;

namespace WalletTransfer.Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepo;
        private readonly ITransactionHistoryRepository _historyRepo;

        public WalletService(
            IWalletRepository walletRepo,
            ITransactionHistoryRepository historyRepo)
        {
            _walletRepo = walletRepo;
            _historyRepo = historyRepo;
        }

        public async Task<int> CreateAsync(CreateWalletDto dto)
        {
            var wallet = new Wallet
            {
                DocumentId = dto.DocumentId,
                OwnerName = dto.OwnerName,
                Balance = dto.InitialBalance,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _walletRepo.AddAsync(wallet);
            await _walletRepo.SaveChangesAsync();
            return wallet.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var wallet = await _walletRepo.GetByIdAsync(id)
                          ?? throw new KeyNotFoundException($"Wallet {id} not found");
            await _walletRepo.DeleteAsync(wallet);
            await _walletRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<WalletDto>> GetAllAsync()
        {
            var wallets = await _walletRepo.GetAllAsync();
            // Map to DTO
            var list = new List<WalletDto>();
            foreach (var w in wallets)
            {
                list.Add(new WalletDto
                {
                    Id = w.Id,
                    DocumentId = w.DocumentId,
                    OwnerName = w.OwnerName,
                    Balance = w.Balance,
                    CreatedAt = w.CreatedAt,
                    UpdatedAt = w.UpdatedAt
                });
            }
            return list;
        }

        public async Task<WalletDto> GetByIdAsync(int id)
        {
            var w = await _walletRepo.GetByIdAsync(id)
                    ?? throw new KeyNotFoundException($"Wallet {id} not found");
            return new WalletDto
            {
                Id = w.Id,
                DocumentId = w.DocumentId,
                OwnerName = w.OwnerName,
                Balance = w.Balance,
                CreatedAt = w.CreatedAt,
                UpdatedAt = w.UpdatedAt
            };
        }

        public async Task TransferAsync(int fromWalletId, int toWalletId, decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be positive");

            var from = await _walletRepo.GetByIdAsync(fromWalletId)
                       ?? throw new KeyNotFoundException($"Wallet {fromWalletId} not found");
            var to = await _walletRepo.GetByIdAsync(toWalletId)
                     ?? throw new KeyNotFoundException($"Wallet {toWalletId} not found");

            if (from.Balance < amount)
                throw new InvalidOperationException("Insufficient balance");

            // Débito
            from.Balance -= amount;
            var debit = new TransactionHistory
            {
                WalletId = from.Id,
                Amount = amount,
                Type = TransactionType.Debit,
                CreatedAt = DateTime.UtcNow
            };
            await _historyRepo.AddAsync(debit);

            // Crédito
            to.Balance += amount;
            var credit = new TransactionHistory
            {
                WalletId = to.Id,
                Amount = amount,
                Type = TransactionType.Credit,
                CreatedAt = DateTime.UtcNow
            };
            await _historyRepo.AddAsync(credit);

            // Persistir cambios
            await _walletRepo.SaveChangesAsync();
            await _historyRepo.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateWalletDto dto)
        {
            var w = await _walletRepo.GetByIdAsync(dto.Id)
                     ?? throw new KeyNotFoundException($"Wallet {dto.Id} not found");
            w.DocumentId = dto.DocumentId;
            w.OwnerName = dto.OwnerName;
            w.UpdatedAt = DateTime.UtcNow;
            await _walletRepo.UpdateAsync(w);
            await _walletRepo.SaveChangesAsync();
        }
    }
}

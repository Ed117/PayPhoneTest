using WalletTransfer.Application.DTOs;
using WalletTransfer.Application.Interfaces;
using WalletTransfer.Domain.Interfaces;

namespace WalletTransfer.Application.Services
{
    public class TransactionHistoryService : ITransactionHistoryService
    {
        private readonly ITransactionHistoryRepository _repo;

        public TransactionHistoryService(ITransactionHistoryRepository repo)
            => _repo = repo;

        public async Task<IEnumerable<TransactionHistoryDto>> GetByWalletIdAsync(int walletId)
        {
            var items = await _repo.GetByWalletIdAsync(walletId);
            return items.Select(th => new TransactionHistoryDto
            {
                Id = th.Id,
                WalletId = th.WalletId,
                Amount = th.Amount,
                Type = th.Type.ToString(),
                CreatedAt = th.CreatedAt
            });
        }
    }
}

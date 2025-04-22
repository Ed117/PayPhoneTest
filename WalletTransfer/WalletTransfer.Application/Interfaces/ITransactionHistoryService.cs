using WalletTransfer.Application.DTOs;

namespace WalletTransfer.Application.Interfaces
{
    public interface ITransactionHistoryService
    {
        Task<IEnumerable<TransactionHistoryDto>> GetByWalletIdAsync(int walletId);
    }
}

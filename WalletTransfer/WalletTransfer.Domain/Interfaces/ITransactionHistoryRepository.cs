using WalletTransfer.Domain.Entities;

namespace WalletTransfer.Domain.Interfaces
{
    public interface ITransactionHistoryRepository
    {
        Task<IEnumerable<TransactionHistory>> GetByWalletIdAsync(int walletId);
        Task<TransactionHistory> GetByIdAsync(int id);
        Task AddAsync(TransactionHistory history);
        
        Task<int> SaveChangesAsync();
    }
}

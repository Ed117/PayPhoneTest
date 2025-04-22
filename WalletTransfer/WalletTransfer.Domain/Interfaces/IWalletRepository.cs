using WalletTransfer.Domain.Entities;

namespace WalletTransfer.Domain.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet> GetByIdAsync(int id);
        Task<IEnumerable<Wallet>> GetAllAsync();
        Task AddAsync(Wallet wallet);
        Task UpdateAsync(Wallet wallet);
        Task DeleteAsync(Wallet wallet);

        Task<int> SaveChangesAsync();
    }
}

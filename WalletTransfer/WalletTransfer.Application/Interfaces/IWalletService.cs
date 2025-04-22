using WalletTransfer.Application.DTOs;

namespace WalletTransfer.Application.Interfaces
{
    public interface IWalletService
    {
        Task<WalletDto> GetByIdAsync(int id);
        Task<IEnumerable<WalletDto>> GetAllAsync();
        Task<int> CreateAsync(CreateWalletDto dto);
        Task UpdateAsync(UpdateWalletDto dto);
        Task DeleteAsync(int id);
        Task TransferAsync(int fromWalletId, int toWalletId, decimal amount);
    }
}

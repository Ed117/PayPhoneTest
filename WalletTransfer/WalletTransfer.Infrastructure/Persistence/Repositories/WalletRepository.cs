using Microsoft.EntityFrameworkCore;
using WalletTransfer.Domain.Entities;
using WalletTransfer.Domain.Interfaces;

namespace WalletTransfer.Infrastructure.Persistence.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletTransferDbContext _context;

        public WalletRepository(WalletTransferDbContext context)
            => _context = context;

        public async Task AddAsync(Wallet wallet)
        {
            await _context.Wallets.AddAsync(wallet);
        }

        public async Task DeleteAsync(Wallet wallet)
        {
            _context.Wallets.Remove(wallet);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Wallet>> GetAllAsync()
            => await _context.Wallets.AsNoTracking().ToListAsync();

        public async Task<Wallet> GetByIdAsync(int id)
            => await _context.Wallets
                 .Include(w => w.TransactionHistories)
                 .FirstOrDefaultAsync(w => w.Id == id);

        public async Task UpdateAsync(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            await Task.CompletedTask;
        }

        public Task<int> SaveChangesAsync()
            => _context.SaveChangesAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using WalletTransfer.Domain.Entities;
using WalletTransfer.Domain.Interfaces;

namespace WalletTransfer.Infrastructure.Persistence.Repositories
{
    public class TransactionHistoryRepository : ITransactionHistoryRepository
    {
        private readonly WalletTransferDbContext _context;

        public TransactionHistoryRepository(WalletTransferDbContext context)
            => _context = context;

        public async Task AddAsync(TransactionHistory history)
        {
            await _context.TransactionHistories.AddAsync(history);
        }

        public async Task<TransactionHistory> GetByIdAsync(int id)
            => await _context.TransactionHistories
                 .AsNoTracking()
                 .FirstOrDefaultAsync(th => th.Id == id);

        public async Task<IEnumerable<TransactionHistory>> GetByWalletIdAsync(int walletId)
            => await _context.TransactionHistories
                 .AsNoTracking()
                 .Where(th => th.WalletId == walletId)
                 .ToListAsync();

        public Task<int> SaveChangesAsync()
            => _context.SaveChangesAsync();
    }
}

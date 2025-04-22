using Microsoft.AspNetCore.Mvc;
using WalletTransfer.Application.DTOs;
using WalletTransfer.Application.Interfaces;

namespace WalletTransfer.API.Controllers
{
    [ApiController]
    [Route("api/wallets/{walletId}/[controller]")]
    public class TransactionHistoryController : ControllerBase
    {
        private readonly ITransactionHistoryService _historyService;

        public TransactionHistoryController(ITransactionHistoryService historyService)
            => _historyService = historyService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionHistoryDto>>> GetByWallet(int walletId)
        {
            var history = await _historyService.GetByWalletIdAsync(walletId);
            return Ok(history);
        }
    }
}

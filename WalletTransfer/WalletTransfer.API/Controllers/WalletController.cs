using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WalletTransfer.Application.DTOs;
using WalletTransfer.Application.Interfaces;

namespace WalletTransfer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IMapper _mapper;

        public WalletController(IWalletService walletService, IMapper mapper)
        {
            _walletService = walletService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WalletDto>>> GetAll()
        {
            var wallets = await _walletService.GetAllAsync();
            return Ok(wallets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WalletDto>> GetById(int id)
        {
            var wallet = await _walletService.GetByIdAsync(id);
            return Ok(wallet);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateWalletDto dto)
        {
            var id = await _walletService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWalletDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            await _walletService.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _walletService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{fromId}/transfer/{toId}")]
        public async Task<IActionResult> Transfer(int fromId, int toId, [FromQuery] decimal amount)
        {
            await _walletService.TransferAsync(fromId, toId, amount);
            return NoContent();
        }
    }
}

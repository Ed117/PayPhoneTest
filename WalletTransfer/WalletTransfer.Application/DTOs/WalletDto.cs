namespace WalletTransfer.Application.DTOs
{
    public class WalletDto
    {
        public int Id { get; set; }
        public required string DocumentId { get; set; }
        public required string OwnerName { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

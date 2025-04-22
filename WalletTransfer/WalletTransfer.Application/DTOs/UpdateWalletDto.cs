namespace WalletTransfer.Application.DTOs
{
    public class UpdateWalletDto
    {
        public int Id { get; set; }
        public required string DocumentId { get; set; }
        public required string OwnerName { get; set; }
    }
}

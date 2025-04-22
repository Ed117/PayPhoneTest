public class CreateWalletDto
{
    public required string DocumentId { get; set; }
    public required string OwnerName { get; set; }
    public decimal InitialBalance { get; set; }
}
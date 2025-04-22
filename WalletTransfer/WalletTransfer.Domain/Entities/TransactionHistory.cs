namespace WalletTransfer.Domain.Entities
{
    public class TransactionHistory
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public DateTime CreatedAt { get; set; }

        public Wallet Wallet { get; set; }
    }
}
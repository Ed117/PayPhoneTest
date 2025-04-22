﻿namespace WalletTransfer.Application.DTOs
{
    public class TransactionHistoryDto
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public required string Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

namespace ApiTask
{
    public enum TransactionStatuses
    {
        Success,
        Failed,
        InProcess,
    }
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string TransactionDate { get; set; } = string.Empty;
        public string CurrencyType { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public string CurrencyName { get; set; } = string.Empty;
        public decimal CurrencyRate { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal RecievedAmount { get; set; }
        public TransactionStatuses TransactionStatus { get; set; }
    }
}

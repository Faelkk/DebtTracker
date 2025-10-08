namespace DebtTrack.Dtos.Debt;

public class DebtDto
{
    public string DebtId { get; set; }
    public string DebtorId { get; set; }
    public string CreditorId { get; set; }
    public string Description { get; set; }
    public decimal TotalAmount { get; set; }
    public int Installments { get; set; }
    public decimal InstallmentValue { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public bool DebtorConfirmedPayment { get; set; }
    public bool CreditorConfirmedReceipt { get; set; }
    public bool IsPaid { get; set; }
}
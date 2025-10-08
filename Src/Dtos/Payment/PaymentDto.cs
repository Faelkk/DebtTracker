namespace DebtTrack.Dtos.Payment;

public class PaymentDto
{
    public string PaymentId { get; set; }
    public string DebtId { get; set; }
    public string InstallmentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
}
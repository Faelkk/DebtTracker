namespace DebtTrack.Dtos.Payment;

public class PaymentCreateDto
{
    public string DebtId { get; set; }
    public string InstallmentId { get; set; }
    public decimal Amount { get; set; }
}
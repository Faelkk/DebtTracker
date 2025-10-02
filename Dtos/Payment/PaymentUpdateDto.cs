namespace DebtTrack.Dtos.Payment;

public class PaymentUpdateDto
{
    public decimal? Amount { get; set; }
    public DateTime? PaidAt { get; set; }
}
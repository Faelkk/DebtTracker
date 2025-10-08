namespace DebtTrack.Dtos.Installment;

public class InstallmentUpdateDto
{
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public bool IsPaid { get; set; }
}
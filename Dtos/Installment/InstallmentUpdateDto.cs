namespace DebtTrack.Dtos.Installment;

public class InstallmentUpdateDto
{
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public bool IsPaid { get; set; }
}
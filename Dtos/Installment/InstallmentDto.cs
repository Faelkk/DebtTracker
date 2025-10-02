namespace DebtTrack.Dtos.Installment;

public class InstallmentDto
{
    public string InstallmentId { get; set; } = default!;
    public string DebtId { get; set; } = default!;
    public int Number { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public bool IsPaid { get; set; }
}
namespace DebtTrack.Dtos.Installment;

public class InstallmentCreateDto
{
    public string DebtId { get; set; } = default!;
    public int Number { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }
}
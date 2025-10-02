namespace DebtTrack.Dtos.Debt;

public class DebtCreateDto
{
    public string UserId { get; set; }
    public string Description { get; set; }
    public decimal TotalAmount { get; set; }
    public int Installments { get; set; }
    public decimal InstallmentValue { get; set; }
    public DateTime DueDate { get; set; }
}
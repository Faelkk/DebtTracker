namespace DebtTrack.Dtos.Debt;

public class DebtUpdateDto
{
    public string? Description { get; set; }
    public decimal? TotalAmount { get; set; }
    public int? Installments { get; set; }
    public decimal? InstallmentValue { get; set; }
    public DateTime? DueDate { get; set; }
    public bool? IsPaid { get; set; }
}
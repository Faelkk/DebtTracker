namespace DebtTrack.Dtos.Debt
{
    public class DebtUpdateDto
    {
        public string? Description { get; set; }
        public bool? DebtorConfirmedPayment { get; set; }
        public bool? CreditorConfirmedReceipt { get; set; }
    }
}
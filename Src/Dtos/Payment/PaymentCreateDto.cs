using System.ComponentModel.DataAnnotations;

namespace DebtTrack.Dtos.Payment
{
    public class PaymentCreateDto
    {
        [Required(ErrorMessage = "DebtId é obrigatório")]
        public string DebtId { get; set; }

        [Required(ErrorMessage = "InstallmentId é obrigatório")]
        public string InstallmentId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount deve ser maior que 0")]
        public decimal Amount { get; set; }
    }
}
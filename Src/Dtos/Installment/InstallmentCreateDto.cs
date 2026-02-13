using System.ComponentModel.DataAnnotations;

namespace DebtTrack.Dtos.Installment
{
    public class InstallmentCreateDto
    {
        [Required(ErrorMessage = "DebtId é obrigatório")]
        public string DebtId { get; set; } = default!;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number deve ser pelo menos 1")]
        public int Number { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount deve ser maior que 0")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Informe o ID do usuário")]
        [StringLength(100, ErrorMessage = "O ID do usuário pode ter no máximo 100 caracteres")]
        public string UserId { get; set; }
    }
}
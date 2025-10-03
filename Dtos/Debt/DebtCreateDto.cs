using System.ComponentModel.DataAnnotations;

public class DebtCreateDto
    {
        [Required(ErrorMessage = "DebtorId é obrigatório")]
        public string DebtorId { get; set; }

        [Required(ErrorMessage = "CreditorId é obrigatório")]
        public string CreditorId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "TotalAmount deve ser maior que 0")]
        public decimal TotalAmount { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Installments deve ser pelo menos 1")]
        public int Installments { get; set; }

        public decimal InstallmentValue { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
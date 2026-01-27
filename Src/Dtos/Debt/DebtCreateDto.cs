using System.ComponentModel.DataAnnotations;

public class DebtCreateDto
{
    [Required]
    public bool IsMyDebt { get; set; }

    [Required(ErrorMessage = "Informe o nome da pessoa envolvida")]
    [StringLength(100, ErrorMessage = "O nome pode ter no máximo 100 caracteres")]
    public string InvolvedPartyName { get; set; }

    [Required(ErrorMessage = "Descrição é obrigatória")]
    public string Description { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "TotalAmount deve ser maior que 0")]
    public decimal TotalAmount { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Installments deve ser pelo menos 1")]
    public int Installments { get; set; }

    [Required(ErrorMessage = "DueDate é obrigatória")]
    public DateTime DueDate { get; set; }
}

using System.ComponentModel.DataAnnotations;
namespace JiuManager.Models;

public class Mensalidade : Registro
{
    [Required(ErrorMessage = "Informe aluno.")]
    [Display(Name = "Aluno")]
    public int AlunoId
    {
        get; set;
    }
    [Required(ErrorMessage = "Informe competência.")]
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Competência")] public string Competencia { get; set; } = "";
    [Required(ErrorMessage = "Informe vencimento.")]
    [Display(Name = "Vencimento")] public DateTime Vencimento { get; set; } = DateTime.Today;
    [Range(typeof(decimal), "0.01", "9999999", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true, ErrorMessage = "Valor fora do intervalo permitido.")]
    [Display(Name = "Valor")]
    public decimal Valor
    {
        get; set;
    }
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Status")] public string Status { get; set; } = "Pendente";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Observações")] public string Observacoes { get; set; } = "";
}


using System.ComponentModel.DataAnnotations;
namespace JiuManager.Models;

public class Plano : Registro
{
    [Required(ErrorMessage = "Informe nome.")]
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Nome")] public string Nome { get; set; } = "";
    [Range(typeof(decimal), "0.01", "9999999", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true, ErrorMessage = "Valor mensal fora do intervalo permitido.")]
    [Display(Name = "Valor mensal")]
    public decimal Valor
    {
        get; set;
    }
    [Range(typeof(int), "1", "28", ErrorMessage = "Dia do vencimento fora do intervalo permitido.")]
    [Display(Name = "Dia do vencimento")]
    public int DiaVencimento
    {
        get; set;
    }
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Status")] public string Status { get; set; } = "Ativo";
}


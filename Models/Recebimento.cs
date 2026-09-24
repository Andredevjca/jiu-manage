using System.ComponentModel.DataAnnotations;
namespace JiuManager.Models;

public class Recebimento : Registro
{
    [Required(ErrorMessage = "Informe mensalidade.")]
    [Display(Name = "Mensalidade")]
    public int MensalidadeId
    {
        get; set;
    }
    [Required(ErrorMessage = "Informe data do recebimento.")]
    [Display(Name = "Data do recebimento")] public DateTime Data { get; set; } = DateTime.Today;
    [Range(typeof(decimal), "0.01", "9999999", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true, ErrorMessage = "Valor recebido fora do intervalo permitido.")]
    [Display(Name = "Valor recebido")]
    public decimal Valor
    {
        get; set;
    }
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Forma de pagamento")] public string FormaPagamento { get; set; } = "PIX";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Observações")] public string Observacoes { get; set; } = "";
}


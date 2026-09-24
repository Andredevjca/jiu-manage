using System.ComponentModel.DataAnnotations;
namespace JiuManager.Models;

public class InscricaoCampeonato : Registro
{
    [Required(ErrorMessage = "Informe campeonato.")]
    [Display(Name = "Campeonato")]
    public int CampeonatoId
    {
        get; set;
    }
    [Required(ErrorMessage = "Informe aluno.")]
    [Display(Name = "Aluno")]
    public int AlunoId
    {
        get; set;
    }
    [Required(ErrorMessage = "Informe categoria.")]
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Categoria")] public string Categoria { get; set; } = "";
    [Required(ErrorMessage = "Informe faixa.")]
    [Display(Name = "Faixa")]
    public int FaixaId
    {
        get; set;
    }
    [Range(typeof(decimal), "0.01", "9999999", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true, ErrorMessage = "Peso (kg) fora do intervalo permitido.")]
    [Display(Name = "Peso (kg)")]
    public decimal Peso
    {
        get; set;
    }
    [Range(typeof(int), "3", "100", ErrorMessage = "Idade fora do intervalo permitido.")]
    [Display(Name = "Idade")]
    public int Idade
    {
        get; set;
    }
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Resultado")] public string Resultado { get; set; } = "Inscrito";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Observações")] public string Observacoes { get; set; } = "";
}


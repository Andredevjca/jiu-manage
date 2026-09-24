using System.ComponentModel.DataAnnotations;
namespace JiuManager.Models;

public class Faixa : Registro
{
    [Required(ErrorMessage = "Informe nome.")]
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Nome")] public string Nome { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Cor")] public string Cor { get; set; } = "#365bea";
    [Range(typeof(int), "0", "999999", ErrorMessage = "Ordem fora do intervalo permitido.")]
    [Display(Name = "Ordem")]
    public int Ordem
    {
        get; set;
    }
    [Range(typeof(int), "1", "120", ErrorMessage = "Meses para próxima graduação fora do intervalo permitido.")]
    [Display(Name = "Meses para próxima graduação")]
    public int MesesMinimos
    {
        get; set;
    }
}


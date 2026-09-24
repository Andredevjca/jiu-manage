using System.ComponentModel.DataAnnotations;
namespace JiuManager.Models;

public class Grau : Registro
{
    [Required(ErrorMessage = "Informe nome.")]
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Nome")] public string Nome { get; set; } = "";
    [Range(typeof(int), "0", "10", ErrorMessage = "Número fora do intervalo permitido.")]
    [Display(Name = "Número")]
    public int Numero
    {
        get; set;
    }
}


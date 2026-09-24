using System.ComponentModel.DataAnnotations;
namespace JiuManager.Models;

public class Evento : Registro
{
    [Required(ErrorMessage = "Informe nome.")]
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Nome")] public string Nome { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Tipo")] public string Tipo { get; set; } = "Seminário";
    [Required(ErrorMessage = "Informe data.")]
    [Display(Name = "Data")] public DateTime Data { get; set; } = DateTime.Today;
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Local")] public string Local { get; set; } = "";
    [Required(ErrorMessage = "Informe unidade.")]
    [Display(Name = "Unidade")]
    public int UnidadeId
    {
        get; set;
    }
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Observações")] public string Observacoes { get; set; } = "";
}


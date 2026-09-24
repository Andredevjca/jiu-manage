using System.ComponentModel.DataAnnotations;
namespace JiuManager.Models;

public class Campeonato : Registro
{
    [Required(ErrorMessage = "Informe nome.")]
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Nome")] public string Nome { get; set; } = "";
    [Required(ErrorMessage = "Informe data.")]
    [Display(Name = "Data")] public DateTime Data { get; set; } = DateTime.Today;
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Local")] public string Local { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Cidade")] public string Cidade { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Organização")] public string Organizacao { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Status")] public string Status { get; set; } = "Inscrições abertas";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Observações")] public string Observacoes { get; set; } = "";
}


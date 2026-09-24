using System.ComponentModel.DataAnnotations;
namespace JiuManager.Models;

public class Usuario : Registro
{
    [Required(ErrorMessage = "Informe nome.")]
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Nome")] public string Nome { get; set; } = "";
    [Required(ErrorMessage = "Informe e-mail.")]
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [JiuManager.Utils.EmailValido(ErrorMessage = "Informe um e-mail válido.")]
    [Display(Name = "E-mail")] public string Email { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Perfil")] public string Perfil { get; set; } = "Administrador";

    [Display(Name = "Aluno vinculado")]
    public int? AlunoId
    {
        get; set;
    }
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Status")] public string Status { get; set; } = "Ativo";
    [System.Text.Json.Serialization.JsonIgnore] public string SenhaHash { get; set; } = "";
}


using System.ComponentModel.DataAnnotations;
namespace JiuManager.Models;

public class Equipe : Registro
{
    [Required(ErrorMessage = "Informe nome.")]
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Nome")] public string Nome { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Nome fantasia")] public string NomeFantasia { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Logo")] public string Logo { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Telefone")] public string Telefone { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "WhatsApp")] public string Whatsapp { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [JiuManager.Utils.EmailValido(ErrorMessage = "Informe um e-mail válido.")]
    [Display(Name = "E-mail")] public string Email { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Site")] public string Site { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "CEP")] public string Cep { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Endereço")] public string Endereco { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Número")] public string Numero { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Bairro")] public string Bairro { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Cidade")] public string Cidade { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Estado")] public string Estado { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Status")] public string Status { get; set; } = "Ativo";
}


using System.ComponentModel.DataAnnotations;
namespace JiuManager.Models;

public class Professor : Registro
{
    [Required(ErrorMessage = "Informe nome.")]
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Nome")] public string Nome { get; set; } = "";
    [Required(ErrorMessage = "Informe unidade.")]
    [Display(Name = "Unidade")]
    public int UnidadeId
    {
        get; set;
    }
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "CPF")] public string Cpf { get; set; } = "";

    [Display(Name = "Nascimento")]
    public DateTime? DataNascimento
    {
        get; set;
    }
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Telefone")] public string Telefone { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "WhatsApp")] public string Whatsapp { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [JiuManager.Utils.EmailValido(ErrorMessage = "Informe um e-mail válido.")]
    [Display(Name = "E-mail")] public string Email { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Foto")] public string Foto { get; set; } = "";
    [Required(ErrorMessage = "Informe faixa.")]
    [Display(Name = "Faixa")]
    public int FaixaId
    {
        get; set;
    }
    [Required(ErrorMessage = "Informe grau.")]
    [Display(Name = "Grau")]
    public int GrauId
    {
        get; set;
    }
    [Required(ErrorMessage = "Informe entrada.")]
    [Display(Name = "Entrada")] public DateTime DataEntrada { get; set; } = DateTime.Today;

    [Display(Name = "Graduação")]
    public DateTime? DataGraduacao
    {
        get; set;
    }

    [Display(Name = "Professor responsável")]
    public int? ProfessorResponsavelId
    {
        get; set;
    }
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Status")] public string Status { get; set; } = "Ativo";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Observações")] public string Observacoes { get; set; } = "";
}


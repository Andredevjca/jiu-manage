using System.ComponentModel.DataAnnotations;
namespace JiuManager.Models;

public class Aluno : Registro
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
    [Display(Name = "Sexo")] public string Sexo { get; set; } = "Não informado";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Telefone")] public string Telefone { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "WhatsApp")] public string Whatsapp { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [JiuManager.Utils.EmailValido(ErrorMessage = "Informe um e-mail válido.")]
    [Display(Name = "E-mail")] public string Email { get; set; } = "";
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
    [Display(Name = "Foto")] public string Foto { get; set; } = "";
    [Required(ErrorMessage = "Informe início.")]
    [Display(Name = "Início")] public DateTime DataInicio { get; set; } = DateTime.Today;
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

    [Display(Name = "Última graduação")]
    public DateTime? DataUltimaGraduacao
    {
        get; set;
    }
    [Required(ErrorMessage = "Informe professor responsável.")]
    [Display(Name = "Professor responsável")]
    public int ProfessorResponsavelId
    {
        get; set;
    }

    [Display(Name = "Plano")]
    public int? PlanoId
    {
        get; set;
    }
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Status")] public string Status { get; set; } = "Ativo";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Observações")] public string Observacoes { get; set; } = "";
    public string CodigoPublico { get; set; } = Guid.NewGuid().ToString("N");
}


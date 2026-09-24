using System.ComponentModel.DataAnnotations;
namespace JiuManager.Models;

public class Graduacao : Registro
{
    [Required(ErrorMessage = "Informe aluno.")]
    [Display(Name = "Aluno")]
    public int AlunoId
    {
        get; set;
    }
    [Required(ErrorMessage = "Informe professor.")]
    [Display(Name = "Professor")]
    public int ProfessorId
    {
        get; set;
    }
    [Required(ErrorMessage = "Informe unidade.")]
    [Display(Name = "Unidade")]
    public int UnidadeId
    {
        get; set;
    }

    [Display(Name = "Faixa anterior")]
    public int? FaixaAnteriorId
    {
        get; set;
    }

    [Display(Name = "Grau anterior")]
    public int? GrauAnteriorId
    {
        get; set;
    }
    [Required(ErrorMessage = "Informe nova faixa.")]
    [Display(Name = "Nova faixa")]
    public int FaixaNovaId
    {
        get; set;
    }
    [Required(ErrorMessage = "Informe novo grau.")]
    [Display(Name = "Novo grau")]
    public int GrauNovoId
    {
        get; set;
    }
    [Required(ErrorMessage = "Informe data da graduação.")]
    [Display(Name = "Data da graduação")] public DateTime DataGraduacao { get; set; } = DateTime.Today;
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Observações")] public string Observacoes { get; set; } = "";
}


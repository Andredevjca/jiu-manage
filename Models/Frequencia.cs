using System.ComponentModel.DataAnnotations;
namespace JiuManager.Models;

public class Frequencia : Registro
{
    [Required(ErrorMessage = "Informe turma.")]
    [Display(Name = "Turma")]
    public int TurmaId
    {
        get; set;
    }
    [Required(ErrorMessage = "Informe aluno.")]
    [Display(Name = "Aluno")]
    public int AlunoId
    {
        get; set;
    }
    [Required(ErrorMessage = "Informe unidade.")]
    [Display(Name = "Unidade")]
    public int UnidadeId
    {
        get; set;
    }
    [Required(ErrorMessage = "Informe data.")]
    [Display(Name = "Data")] public DateTime Data { get; set; } = DateTime.Today;

    [Display(Name = "Presente")]
    public bool Presente
    {
        get; set;
    }
}


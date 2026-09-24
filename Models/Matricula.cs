using System.ComponentModel.DataAnnotations;
namespace JiuManager.Models;

public class Matricula : Registro
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
}


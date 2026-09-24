using System.ComponentModel.DataAnnotations;
namespace JiuManager.Models;

public class Turma : Registro
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
    [Required(ErrorMessage = "Informe professor.")]
    [Display(Name = "Professor")]
    public int ProfessorId
    {
        get; set;
    }
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Descrição")] public string Descricao { get; set; } = "";
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Dia da semana")] public string DiaSemana { get; set; } = "Segunda-feira";
    [Required(ErrorMessage = "Informe início.")]
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Início")] public string HoraInicio { get; set; } = "19:00";
    [Required(ErrorMessage = "Informe término.")]
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Término")] public string HoraFim { get; set; } = "20:30";
    [Required(ErrorMessage = "Informe faixa mínima.")]
    [Display(Name = "Faixa mínima")]
    public int FaixaMinimaId
    {
        get; set;
    }
    [Required(ErrorMessage = "Informe faixa máxima.")]
    [Display(Name = "Faixa máxima")]
    public int FaixaMaximaId
    {
        get; set;
    }
    [Range(typeof(int), "1", "500", ErrorMessage = "Capacidade fora do intervalo permitido.")]
    [Display(Name = "Capacidade")]
    public int Capacidade
    {
        get; set;
    }
    [StringLength(255, ErrorMessage = "Use no máximo 255 caracteres.")]
    [Display(Name = "Status")] public string Status { get; set; } = "Ativo";
}


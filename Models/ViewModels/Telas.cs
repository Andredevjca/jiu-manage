using JiuManager.Configuracoes;
namespace JiuManager.Models.ViewModels;

public class ListaModelo
{
    public Modulo Modulo { get; set; } = new(); public List<Dictionary<string, object?>> Registros { get; set; } = []; public Dictionary<string, Dictionary<int, string>> Opcoes { get; set; } = []; public string Busca { get; set; } = ""; public string Status { get; set; } = ""; public int Pagina { get; set; } = 1; public int Total
    {
        get; set;
    }
    public int Paginas => (int)Math.Ceiling(Total / 10d);
}
public class FormularioModelo
{
    public Modulo Modulo { get; set; } = new(); public Registro Registro { get; set; } = null!; public Dictionary<string, Dictionary<int, string>> Opcoes { get; set; } = [];
}
public class DetalhesModelo
{
    public Modulo Modulo { get; set; } = new(); public Registro Registro { get; set; } = null!; public Dictionary<string, Dictionary<int, string>> Opcoes { get; set; } = []; public List<Dictionary<string, object?>> Graduacoes { get; set; } = []; public List<Dictionary<string, object?>> Mensalidades { get; set; } = []; public List<Dictionary<string, object?>> Campeonatos { get; set; } = []; public decimal Frequencia
    {
        get; set;
    }
}
public class LoginModelo
{
    [System.ComponentModel.DataAnnotations.Required] public string Email { get; set; } = ""; [System.ComponentModel.DataAnnotations.Required] public string Senha { get; set; } = ""; public string? Retorno
    {
        get; set;
    }
}
public class ChamadaModelo
{
    public int TurmaId
    {
        get; set;
    }
    public DateTime Data { get; set; } = DateTime.Today; public Dictionary<int, string> Turmas { get; set; } = []; public List<Aluno> Alunos { get; set; } = []; public HashSet<int> Presentes { get; set; } = [];
}
public class DashboardModelo
{
    public bool ExibirFinanceiro
    {
        get; set;
    }
    public int Alunos
    {
        get; set;
    }
    public int Professores
    {
        get; set;
    }
    public int Unidades
    {
        get; set;
    }
    public int Graduacoes
    {
        get; set;
    }
    public decimal EmAberto
    {
        get; set;
    }
    public List<(string Nome, string Cor, int Total)> Faixas { get; set; } = []; public List<(string Mes, int Total)> Evolucao { get; set; } = []; public List<Dictionary<string, object?>> ProximasGraduacoes { get; set; } = []; public List<Dictionary<string, object?>> Mensalidades { get; set; } = []; public List<Dictionary<string, object?>> Eventos { get; set; } = [];
}

using System.Text.Json;
using JiuManager.Models;
namespace JiuManager.Configuracoes;

public class Campo
{
    public string Nome { get; set; } = ""; public string Tipo { get; set; } = ""; public string Rotulo { get; set; } = ""; public string? Referencia
    {
        get; set;
    }
    public string? Entrada
    {
        get; set;
    }
    public string[]? Opcoes
    {
        get; set;
    }
    public bool Obrigatorio
    {
        get; set;
    }
    public decimal? Min
    {
        get; set;
    }
    public decimal? Max
    {
        get; set;
    }
}
public class Modulo
{
    public string Nome { get; set; } = ""; public string Plural { get; set; } = ""; public string Titulo { get; set; } = ""; public string Tabela { get; set; } = ""; public string Icone { get; set; } = ""; public string Grupo { get; set; } = ""; public bool Imutavel
    {
        get; set;
    }
    public List<Campo> Campos { get; set; } = [];
}
public class CatalogoModulos
{
    public IReadOnlyList<Modulo> Todos
    {
        get;
    }
    public CatalogoModulos(IWebHostEnvironment ambiente)
    {
        Todos = JsonSerializer.Deserialize<List<Modulo>>(File.ReadAllText(Path.Combine(ambiente.ContentRootPath, "Configuracoes/modulos.json")), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }
    public Modulo Obter(string nome) => Todos.Single(m => m.Nome == nome || m.Plural == nome); public Modulo Obter<T>() where T : Registro => Obter(typeof(T).Name);
}

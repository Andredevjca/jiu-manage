using JiuManager.Configuracoes;
namespace JiuManager.Repositories;

public class PerfilAlunoRepositorio(ConsultaRepositorio consultas, ContextoEquipe contexto)
{
    public async Task<List<Dictionary<string, object?>>> GraduacoesAsync(int id) => await consultas.ConsultarAsync("SELECT g.*, f.Nome Faixa, gr.Nome Grau, p.Nome Professor, u.Nome Unidade FROM graduacoes g JOIN faixas f ON f.Id=g.FaixaNovaId JOIN graus gr ON gr.Id=g.GrauNovoId JOIN professores p ON p.Id=g.ProfessorId JOIN unidades u ON u.Id=g.UnidadeId WHERE g.EquipeId=@EquipeId AND g.AlunoId=@Id ORDER BY g.DataGraduacao DESC,g.Id DESC", new { contexto.EquipeId, Id = id });
    public async Task<List<Dictionary<string, object?>>> CampeonatosAsync(int id) => await consultas.ConsultarAsync("SELECT c.Nome,i.Categoria,i.Resultado FROM inscricoes_campeonatos i JOIN campeonatos c ON c.Id=i.CampeonatoId WHERE i.EquipeId=@EquipeId AND i.AlunoId=@Id", new { contexto.EquipeId, Id = id });
}

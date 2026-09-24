using Dapper;
using JiuManager.Configuracoes;
using JiuManager.Models;
namespace JiuManager.Repositories;

public class DocumentoRepositorio(FabricaConexao conexoes, ContextoEquipe contexto)
{
    public async Task<Dictionary<string, object?>?> AlunoAsync(int? id = null, string? codigo = null)
    {
        await using var conexao = conexoes.Criar();
        var registro = await conexao.QuerySingleOrDefaultAsync("SELECT a.Id,a.Nome,a.Foto,a.Status,a.CodigoPublico,a.DataInicio,f.Nome Faixa,g.Nome Grau,u.Nome Unidade,e.Nome Equipe,e.Logo FROM alunos a JOIN faixas f ON f.Id=a.FaixaId JOIN graus g ON g.Id=a.GrauId JOIN unidades u ON u.Id=a.UnidadeId JOIN equipes e ON e.Id=a.EquipeId WHERE " + (codigo != null ? "a.CodigoPublico=@codigo" : "a.Id=@id AND a.EquipeId=@equipe"), new
        {
            id,
            codigo,
            equipe = codigo == null ? contexto.EquipeId : 0
        });
        return registro == null ? null : new Dictionary<string, object?>((IDictionary<string, object?>)registro);
    }
    public async Task<Dictionary<string, object?>?> CertificadoAsync(int id)
    {
        await using var conexao = conexoes.Criar();
        var registro = await conexao.QuerySingleOrDefaultAsync("SELECT g.Id,g.DataGraduacao,a.Nome Aluno,f.Nome Faixa,gr.Nome Grau,p.Nome Professor,e.Nome Equipe,e.Logo FROM graduacoes g JOIN alunos a ON a.Id=g.AlunoId JOIN faixas f ON f.Id=g.FaixaNovaId JOIN graus gr ON gr.Id=g.GrauNovoId JOIN professores p ON p.Id=g.ProfessorId JOIN equipes e ON e.Id=g.EquipeId WHERE g.Id=@id AND g.EquipeId=@EquipeId", new
        {
            id,
            contexto.EquipeId
        });
        return registro == null ? null : new Dictionary<string, object?>((IDictionary<string, object?>)registro);
    }
    public async Task EmitirAsync(int graduacaoId)
    {
        await using var conexao = conexoes.Criar();
        await conexao.ExecuteAsync("INSERT INTO certificados(EquipeId,GraduacaoId,Codigo) SELECT EquipeId,Id,@codigo FROM graduacoes WHERE Id=@graduacaoId AND EquipeId=@EquipeId ON DUPLICATE KEY UPDATE GraduacaoId=GraduacaoId", new
        {
            contexto.EquipeId,
            graduacaoId,
            codigo = Guid.NewGuid().ToString("N")
        });
    }
}


using Dapper;
using JiuManager.Configuracoes;
using JiuManager.Models;
namespace JiuManager.Repositories;

public class ConsultaRepositorio(FabricaConexao conexoes, ContextoEquipe contexto, CatalogoModulos catalogo)
{
    public async Task<List<Dictionary<string, object?>>> ListarAsync(string modulo, bool filtrarUnidade = false)
    {
        var definicao = catalogo.Obter(modulo);
        var filtro = filtrarUnidade && contexto.UnidadeId.HasValue && definicao.Campos.Any(c => c.Nome == "UnidadeId") ? " AND UnidadeId=@UnidadeId" : "";
        await using var conexao = conexoes.Criar();
        var linhas = await conexao.QueryAsync($"SELECT * FROM `{definicao.Tabela}` WHERE EquipeId=@EquipeId {filtro} ORDER BY Id DESC", new
        {
            contexto.EquipeId,
            contexto.UnidadeId
        });
        return linhas.Select(l => new Dictionary<string, object?>((IDictionary<string, object?>)l)).ToList();
    }
    public async Task<bool> ExisteAsync(string modulo, int id)
    {
        var definicao = catalogo.Obter(modulo);
        await using var conexao = conexoes.Criar();
        return await conexao.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM `{definicao.Tabela}` WHERE EquipeId=@EquipeId AND Id=@id", new
        {
            contexto.EquipeId,
            id
        }) > 0;
    }
    public async Task<List<Dictionary<string, object?>>> ConsultarAsync(string sql, object parametros)
    {
        await using var conexao = conexoes.Criar();
        return (await conexao.QueryAsync(sql, parametros)).Select(l => new Dictionary<string, object?>((IDictionary<string, object?>)l)).ToList();
    }
}


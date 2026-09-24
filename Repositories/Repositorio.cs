using Dapper;
using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
namespace JiuManager.Repositories;

public class Repositorio<T>(FabricaConexao conexoes, ContextoEquipe contexto, CatalogoModulos catalogo) : IRepositorio<T> where T : Registro
{
    protected readonly FabricaConexao Conexoes = conexoes; protected readonly ContextoEquipe Contexto = contexto; protected readonly Modulo Modulo = catalogo.Obter<T>();
    public async Task<List<T>> ListarAsync()
    {
        await using var conexao = Conexoes.Criar();
        return (await conexao.QueryAsync<T>($"SELECT * FROM `{Modulo.Tabela}` WHERE EquipeId=@EquipeId ORDER BY Id DESC", new
        {
            Contexto.EquipeId
        })).ToList();
    }
    public async Task<T?> ObterAsync(int id)
    {
        await using var conexao = Conexoes.Criar();
        return await conexao.QuerySingleOrDefaultAsync<T>($"SELECT * FROM `{Modulo.Tabela}` WHERE Id=@id AND EquipeId=@EquipeId", new
        {
            id,
            Contexto.EquipeId
        });
    }
    public async Task<int> SalvarAsync(T registro)
    {
        registro.EquipeId = Contexto.EquipeId;
        var campos = Modulo.Campos.Select(c => c.Nome).ToList();
        if (typeof(T) == typeof(Aluno))
            campos.Add("CodigoPublico");
        if (typeof(T) == typeof(Usuario))
            campos.Add("SenhaHash");
        await using var conexao = Conexoes.Criar();
        if (registro.Id == 0)
        {
            return await conexao.ExecuteScalarAsync<int>($"INSERT INTO `{Modulo.Tabela}` (EquipeId,{string.Join(",", campos.Select(c => $"`{c}`"))}) VALUES (@EquipeId,{string.Join(",", campos.Select(c => "@" + c))}); SELECT LAST_INSERT_ID();", registro);
        }
        var alterados = await conexao.ExecuteAsync($"UPDATE `{Modulo.Tabela}` SET {string.Join(",", campos.Select(c => $"`{c}`=@{c}"))} WHERE Id=@Id AND EquipeId=@EquipeId", registro);
        if (alterados == 0)
            throw new RegraNegocioException("Registro não encontrado.");
        return registro.Id;
    }
    public async Task ExcluirAsync(int id)
    {
        await using var conexao = Conexoes.Criar();
        await conexao.ExecuteAsync($"DELETE FROM `{Modulo.Tabela}` WHERE Id=@id AND EquipeId=@EquipeId", new
        {
            id,
            Contexto.EquipeId
        });
    }
}


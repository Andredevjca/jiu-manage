using Dapper;
using JiuManager.Models;
using JiuManager.Configuracoes;
namespace JiuManager.Repositories;

public class PermissaoRepositorio(FabricaConexao conexoes, ContextoEquipe contexto)
{
    public async Task<List<(int Id, string Nome, bool Concedida)>> ListarAsync(int usuarioId)
    {
        await using var conexao = conexoes.Criar();
        var linhas = await conexao.QueryAsync("SELECT p.Id,p.Nome,EXISTS(SELECT 1 FROM usuario_permissoes up JOIN usuarios u ON u.Id=up.UsuarioId WHERE up.PermissaoId=p.Id AND u.Id=@usuarioId AND u.EquipeId=@EquipeId) Concedida FROM permissoes p ORDER BY p.Nome", new
        {
            usuarioId,
            contexto.EquipeId
        });
        return linhas.Select(l => ((int)l.Id, (string)l.Nome, (bool)Convert.ToBoolean(l.Concedida))).ToList();
    }
    public async Task SalvarAsync(int usuarioId, int[] permissoes)
    {
        await using var conexao = conexoes.Criar();
        await conexao.OpenAsync();
        await using var transacao = await conexao.BeginTransactionAsync();
        if (await conexao.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM usuarios WHERE Id=@usuarioId AND EquipeId=@EquipeId", new
        {
            usuarioId,
            contexto.EquipeId
        }, transacao) == 0)
            throw new RegraNegocioException("Usuário inválido.");
        await conexao.ExecuteAsync("DELETE FROM usuario_permissoes WHERE UsuarioId=@usuarioId", new
        {
            usuarioId
        }, transacao);
        foreach (var id in permissoes.Distinct())
            await conexao.ExecuteAsync("INSERT INTO usuario_permissoes(UsuarioId,PermissaoId) SELECT @usuarioId,Id FROM permissoes WHERE Id=@id", new
            {
                usuarioId,
                id
            }, transacao);
        await transacao.CommitAsync();
    }
}



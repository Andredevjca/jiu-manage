using Dapper;
using JiuManager.Models;
namespace JiuManager.Repositories;

public class AutenticacaoRepositorio(FabricaConexao conexoes)
{
    public async Task<Usuario?> PorEmailAsync(string email)
    {
        await using var conexao = conexoes.Criar();
        return await conexao.QuerySingleOrDefaultAsync<Usuario>("SELECT * FROM usuarios WHERE Email=@email AND Status='Ativo'", new
        {
            email
        });
    }
    public async Task<Usuario?> PorIdAsync(int id)
    {
        await using var conexao = conexoes.Criar();
        return await conexao.QuerySingleOrDefaultAsync<Usuario>("SELECT * FROM usuarios WHERE Id=@id AND Status='Ativo'", new
        {
            id
        });
    }
    public async Task AlterarSenhaAsync(int id, string hash)
    {
        await using var conexao = conexoes.Criar();
        await conexao.ExecuteAsync("UPDATE usuarios SET SenhaHash=@hash WHERE Id=@id", new
        {
            id,
            hash
        });
    }
    public async Task<List<string>> PermissoesAsync(int id)
    {
        await using var conexao = conexoes.Criar();
        return (await conexao.QueryAsync<string>("SELECT p.Nome FROM permissoes p JOIN usuario_permissoes up ON up.PermissaoId=p.Id WHERE up.UsuarioId=@id", new
        {
            id
        })).ToList();
    }
}


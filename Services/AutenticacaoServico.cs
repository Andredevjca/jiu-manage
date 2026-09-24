using JiuManager.Models;
using JiuManager.Repositories;
using JiuManager.Utils;
namespace JiuManager.Services;

public class AutenticacaoServico(AutenticacaoRepositorio repositorio)
{
    public async Task<Usuario?> EntrarAsync(string email, string senha)
    {
        var usuario = await repositorio.PorEmailAsync(email.Trim());
        return usuario != null && CriptografiaUtil.Conferir(usuario, senha) ? usuario : null;
    }
    public async Task AlterarSenhaAsync(int id, string atual, string nova, string confirmacao)
    {
        var usuario = await repositorio.PorIdAsync(id) ?? throw new RegraNegocioException("Usuário não encontrado.");
        if (!CriptografiaUtil.Conferir(usuario, atual))
            throw new RegraNegocioException("A senha atual está incorreta.");
        if (nova.Length < 8 || nova.Length > 128)
            throw new RegraNegocioException("A nova senha deve ter de 8 a 128 caracteres.");
        if (nova != confirmacao)
            throw new RegraNegocioException("A confirmação da senha não confere.");
        await repositorio.AlterarSenhaAsync(id, CriptografiaUtil.GerarHash(usuario, nova));
    }
}

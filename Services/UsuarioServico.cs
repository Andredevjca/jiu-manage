using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
using JiuManager.Repositories;
using JiuManager.Utils;
namespace JiuManager.Services;

public class UsuarioServico(IUsuarioRepositorio repositorio, ConsultaRepositorio consultas, CatalogoModulos catalogo, ContextoEquipe contexto) : CadastroServico<Usuario>(repositorio, consultas, catalogo)
{
    public async Task<int> SalvarComSenhaAsync(Usuario usuario, string? senha)
    {
        usuario.Email = usuario.Email.Trim().ToLowerInvariant();
        await ValidarAsync(usuario);
        var anterior = usuario.Id > 0 ? await ObterAsync(usuario.Id) : null;
        if (usuario.Id > 0 && anterior == null)
            throw new RegraNegocioException("Usuário não encontrado.");
        if (usuario.Id == contexto.UsuarioId && (usuario.Perfil != "Administrador" || usuario.Status != "Ativo"))
            throw new RegraNegocioException("Você não pode desativar ou remover seu próprio acesso de administrador.");
        if (usuario.Perfil == "Aluno" && !usuario.AlunoId.HasValue)
            throw new RegraNegocioException("Vincule o usuário a um aluno.");
        if (!string.IsNullOrWhiteSpace(senha))
        {
            if (senha.Length < 8 || senha.Length > 128)
                throw new RegraNegocioException("Use uma senha de 8 a 128 caracteres.");
            usuario.SenhaHash = CriptografiaUtil.GerarHash(usuario, senha);
        }
        else if (anterior != null)
            usuario.SenhaHash = anterior.SenhaHash;
        else
            throw new RegraNegocioException("Informe uma senha para o novo usuário.");
        return await Repositorio.SalvarAsync(usuario);
    }
    public override Task ExcluirAsync(int id)
    {
        if (id == contexto.UsuarioId)
            throw new RegraNegocioException("Você não pode excluir seu próprio usuário.");
        return base.ExcluirAsync(id);
    }
}

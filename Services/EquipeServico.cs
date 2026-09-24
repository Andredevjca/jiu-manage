using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
using JiuManager.Repositories;
namespace JiuManager.Services;

public class EquipeServico(IEquipeRepositorio repositorio, ConsultaRepositorio consultas, CatalogoModulos catalogo, ContextoEquipe contexto) : CadastroServico<Equipe>(repositorio, consultas, catalogo)
{
    public override Task<int> SalvarAsync(Equipe equipe)
    {
        if (equipe.Id != contexto.EquipeId)
            throw new RegraNegocioException("Edite a equipe vinculada ao seu usuário.");
        return base.SalvarAsync(equipe);
    }
    public override Task ExcluirAsync(int id) => throw new RegraNegocioException("A equipe principal não pode ser excluída.");
}

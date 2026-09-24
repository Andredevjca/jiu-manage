using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
using JiuManager.Repositories;
namespace JiuManager.Services;

public class GraduacaoServico(IGraduacaoRepositorio repositorio, ConsultaRepositorio consultas, CatalogoModulos catalogo, OperacaoRepositorio operacoes) : CadastroServico<Graduacao>(repositorio, consultas, catalogo)
{
    public override async Task<int> SalvarAsync(Graduacao graduacao)
    {
        if (graduacao.Id > 0)
            throw new RegraNegocioException("O histórico de graduação não pode ser alterado.");
        await ValidarAsync(graduacao);
        return await operacoes.GraduarAsync(graduacao);
    }
}

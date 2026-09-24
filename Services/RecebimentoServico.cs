using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
using JiuManager.Repositories;
namespace JiuManager.Services;

public class RecebimentoServico(IRecebimentoRepositorio repositorio, ConsultaRepositorio consultas, CatalogoModulos catalogo, OperacaoRepositorio operacoes) : CadastroServico<Recebimento>(repositorio, consultas, catalogo)
{
    public override async Task<int> SalvarAsync(Recebimento recebimento)
    {
        if (recebimento.Id > 0)
            throw new RegraNegocioException("Recebimentos não podem ser alterados.");
        await ValidarAsync(recebimento);
        if (recebimento.Data.Date > DateTime.Today)
            throw new RegraNegocioException("O recebimento não pode estar no futuro.");
        return await operacoes.ReceberAsync(recebimento);
    }
}

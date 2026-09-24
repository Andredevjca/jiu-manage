using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
using JiuManager.Repositories;
namespace JiuManager.Services;

public class MensalidadeServico(IMensalidadeRepositorio repositorio, ConsultaRepositorio consultas, CatalogoModulos catalogo, OperacaoRepositorio operacoes) : CadastroServico<Mensalidade>(repositorio, consultas, catalogo)
{
    public override async Task<int> SalvarAsync(Mensalidade mensalidade)
    {
        await ValidarAsync(mensalidade);
        if (!DateTime.TryParseExact(mensalidade.Competencia, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _))
            throw new RegraNegocioException("Informe uma competência válida.");
        return await operacoes.SalvarMensalidadeAsync(mensalidade);
    }
    public Task<int> GerarAsync(DateTime competencia) => operacoes.GerarMensalidadesAsync(competencia);
}

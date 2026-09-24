using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
using JiuManager.Repositories;
namespace JiuManager.Services;

public class TurmaServico(ITurmaRepositorio repositorio, ConsultaRepositorio consultas, CatalogoModulos catalogo) : CadastroServico<Turma>(repositorio, consultas, catalogo)
{
    public override async Task<int> SalvarAsync(Turma turma)
    {
        if (!TimeOnly.TryParse(turma.HoraInicio, out var inicio) || !TimeOnly.TryParse(turma.HoraFim, out var fim) || fim <= inicio)
            throw new RegraNegocioException("O término deve ser posterior ao início.");
        return await base.SalvarAsync(turma);
    }
}

using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
using JiuManager.Repositories;
namespace JiuManager.Services;

public class MatriculaServico(IMatriculaRepositorio repositorio, ConsultaRepositorio consultas, CatalogoModulos catalogo, OperacaoRepositorio operacoes) : CadastroServico<Matricula>(repositorio, consultas, catalogo)
{
    public override async Task<int> SalvarAsync(Matricula matricula)
    {
        await ValidarAsync(matricula);
        return await operacoes.MatricularAsync(matricula);
    }
}

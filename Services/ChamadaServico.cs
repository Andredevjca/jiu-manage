using JiuManager.Models;
using JiuManager.Models.ViewModels;
using JiuManager.Repositories;
using JiuManager.Interfaces;
namespace JiuManager.Services;

public class ChamadaServico(OperacaoRepositorio operacoes, ITurmaRepositorio turmas)
{
    public async Task<ChamadaModelo> CarregarAsync(int turmaId, DateTime? data)
    {
        var lista = await turmas.ListarAsync();
        var modelo = new ChamadaModelo { TurmaId = turmaId, Data = data ?? DateTime.Today, Turmas = lista.Where(t => t.Status == "Ativo").ToDictionary(t => t.Id, t => t.Nome) };
        if (turmaId > 0 && modelo.Turmas.ContainsKey(turmaId))
        {
            modelo.Alunos = await operacoes.AlunosTurmaAsync(turmaId);
            modelo.Presentes = await operacoes.PresentesAsync(turmaId, modelo.Data);
        }
        return modelo;
    }
    public async Task RegistrarAsync(int turmaId, DateTime data, int[] presentes)
    {
        if (data.Date > DateTime.Today || data.Year < 2000)
            throw new RegraNegocioException("Informe uma data válida, até hoje.");
        await operacoes.RegistrarChamadaAsync(turmaId, data, presentes);
    }
}

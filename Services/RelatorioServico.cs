using JiuManager.Repositories;
namespace JiuManager.Services;

public class RelatorioServico(RelatorioRepositorio repositorio)
{
    public Task<List<Dictionary<string, object?>>> DadosAsync(string tipo) => repositorio.DadosAsync(tipo);
}

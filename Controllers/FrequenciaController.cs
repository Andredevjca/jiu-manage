using Microsoft.AspNetCore.Mvc;
using JiuManager.Services;
using JiuManager.Models;
namespace JiuManager.Controllers;

public class FrequenciaController(ChamadaServico servico) : Controller
{
    public async Task<IActionResult> Index(int turmaId = 0, DateTime? data = null) => View(await servico.CarregarAsync(turmaId, data)); [HttpPost]
    public async Task<IActionResult> Registrar(int turmaId, DateTime data, int[] presentes)
    {
        try
        {
            await servico.RegistrarAsync(turmaId, data, presentes);
            TempData["Sucesso"] = "Chamada salva. A frequência dos alunos foi atualizada.";
        }
        catch (RegraNegocioException erro) { TempData["Erro"] = erro.Message; }
        return RedirectToAction("Index", new
        {
            turmaId,
            data = data.ToString("yyyy-MM-dd")
        });
    }
}

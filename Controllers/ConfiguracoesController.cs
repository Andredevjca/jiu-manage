using Microsoft.AspNetCore.Mvc;
using JiuManager.Services;
namespace JiuManager.Controllers;

public class ConfiguracoesController(EquipeServico equipes) : Controller
{
    public async Task<IActionResult> Index()
    {
        var equipe = (await equipes.ListarAsync()).First();
        return RedirectToAction("Editar", "Equipes", new
        {
            id = equipe.Id
        });
    }
}

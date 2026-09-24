using Microsoft.AspNetCore.Mvc;
using JiuManager.Services;
namespace JiuManager.Controllers;

public class DashboardController(DashboardServico servico) : Controller
{
    public async Task<IActionResult> Index() => View(await servico.CarregarAsync());
}

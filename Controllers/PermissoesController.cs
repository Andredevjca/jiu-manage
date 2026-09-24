using Microsoft.AspNetCore.Mvc;
using JiuManager.Repositories;
using JiuManager.Services;
namespace JiuManager.Controllers;

public class PermissoesController(PermissaoRepositorio repositorio, UsuarioServico usuarios) : Controller
{
    public async Task<IActionResult> Index(int usuarioId = 0)
    {
        ViewBag.Usuarios = await usuarios.ListarAsync();
        ViewBag.UsuarioId = usuarioId;
        return View(await repositorio.ListarAsync(usuarioId));
    }
    [HttpPost]
    public async Task<IActionResult> Salvar(int usuarioId, int[] permissoes)
    {
        await repositorio.SalvarAsync(usuarioId, permissoes);
        TempData["Sucesso"] = "Permissões adicionais atualizadas.";
        return RedirectToAction(nameof(Index), new
        {
            usuarioId
        });
    }
}

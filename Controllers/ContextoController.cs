using Microsoft.AspNetCore.Mvc;
using JiuManager.Repositories;
namespace JiuManager.Controllers;

public class ContextoController(ConsultaRepositorio consultas) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Unidade(int unidadeId, string? retorno)
    {
        if (unidadeId > 0 && !await consultas.ExisteAsync("Unidades", unidadeId))
            return BadRequest();
        Response.Cookies.Append("unidade", unidadeId.ToString(), new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Lax, IsEssential = true });
        return !string.IsNullOrEmpty(retorno) && Url.IsLocalUrl(retorno) ? LocalRedirect(retorno) : RedirectToAction("Index", "Dashboard");
    }
}

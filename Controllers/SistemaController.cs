using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace JiuManager.Controllers;

public class SistemaController : Controller
{
    [AllowAnonymous]
    public IActionResult Erro()
    {
        Response.StatusCode = 500;
        return View();
    }
}

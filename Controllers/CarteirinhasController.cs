using Microsoft.AspNetCore.Mvc;
using JiuManager.Services;
namespace JiuManager.Controllers;

public class CarteirinhasController(DocumentoServico documentos, TelaServico telas) : Controller
{
    public async Task<IActionResult> Index(string? busca, int pagina = 1) => View(await telas.ListaAsync("Alunos", busca, pagina, null)); public async Task<IActionResult> Visualizar(int id)
    {
        var modelo = await documentos.CarteirinhaAsync(id);
        return modelo == null ? NotFound() : View(modelo);
    }
}

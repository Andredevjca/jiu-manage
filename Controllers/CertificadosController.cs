using Microsoft.AspNetCore.Mvc;
using JiuManager.Services;
namespace JiuManager.Controllers;

public class CertificadosController(DocumentoServico documentos, TelaServico telas) : Controller
{
    public async Task<IActionResult> Index(string? busca, int pagina = 1) => View(await telas.ListaAsync("Graduacoes", busca, pagina, null)); [HttpPost]
    public async Task<IActionResult> Gerar(int id)
    {
        if (await documentos.CertificadoAsync(id) == null)
            return NotFound();
        await documentos.EmitirAsync(id);
        return RedirectToAction(nameof(Visualizar), new
        {
            id
        });
    }
    public async Task<IActionResult> Visualizar(int id)
    {
        var modelo = await documentos.CertificadoAsync(id);
        return modelo == null ? NotFound() : View(modelo);
    }
}

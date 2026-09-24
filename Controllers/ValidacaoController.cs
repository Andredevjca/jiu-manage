using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JiuManager.Services;
namespace JiuManager.Controllers;

[AllowAnonymous]
public class ValidacaoController(DocumentoServico documentos) : Controller
{
    [HttpGet("/validar/aluno/{codigo}")]
    public async Task<IActionResult> Aluno(string codigo)
    {
        var modelo = await documentos.ValidarAsync(codigo);
        return modelo == null ? NotFound("Carteirinha não encontrada.") : View(modelo);
    }
}

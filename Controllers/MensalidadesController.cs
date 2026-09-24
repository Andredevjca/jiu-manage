using Microsoft.AspNetCore.Mvc;
using JiuManager.Models;
using JiuManager.Services;
using JiuManager.Configuracoes;
namespace JiuManager.Controllers;

public class MensalidadesController(MensalidadeServico servico, TelaServico telas, CatalogoModulos catalogo, ImagemServico imagens) : CadastroController<Mensalidade>(servico, telas, catalogo, imagens)
{
    [HttpPost]
    public async Task<IActionResult> Gerar(string competencia)
    {
        if (DateTime.TryParseExact(competencia, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var data))
        {
            var total = await servico.GerarAsync(data);
            TempData["Sucesso"] = $"{total} mensalidades geradas. Competências existentes foram preservadas.";
        }
        else
            TempData["Erro"] = "Competência inválida.";
        return RedirectToAction("Index");
    }
}

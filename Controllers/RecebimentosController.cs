using Microsoft.AspNetCore.Mvc;
using JiuManager.Models;
using JiuManager.Models.ViewModels;
using JiuManager.Services;
using JiuManager.Repositories;
using JiuManager.Configuracoes;
namespace JiuManager.Controllers;

public class RecebimentosController(RecebimentoServico servico, TelaServico telas, CatalogoModulos catalogo, ImagemServico imagens, MensalidadeServico mensalidades) : CadastroController<Recebimento>(servico, telas, catalogo, imagens)
{
    public override async Task<IActionResult> Criar()
    {
        var recebimento = new Recebimento();
        if (int.TryParse(Request.Query["mensalidadeId"], out var id))
        {
            var mensalidade = await mensalidades.ObterAsync(id);
            if (mensalidade == null)
                return NotFound();
            recebimento.MensalidadeId = id;
            recebimento.Valor = mensalidade.Valor - (await servico.ListarAsync()).Where(r => r.MensalidadeId == id).Sum(r => r.Valor);
        }
        return View(new FormularioModelo { Modulo = Modulo, Registro = recebimento, Opcoes = await Telas.OpcoesAsync(Modulo) });
    }
}

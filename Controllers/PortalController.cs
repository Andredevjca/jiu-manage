using Microsoft.AspNetCore.Mvc;
using JiuManager.Services;
namespace JiuManager.Controllers;

public class PortalController(PermissaoServico permissoes, AlunoServico alunos, TelaServico telas, JiuManager.Configuracoes.CatalogoModulos catalogo, DocumentoServico documentos) : Controller
{
    public async Task<IActionResult> Index()
    {
        var usuario = await permissoes.UsuarioAsync();
        if (usuario?.AlunoId == null)
            return RedirectToAction("Senha", "Conta");
        var aluno = await alunos.ObterAsync(usuario.AlunoId.Value);
        return aluno == null ? NotFound() : View("~/Views/Alunos/Detalhes.cshtml", await telas.DetalhesAsync(aluno, catalogo.Obter("Alunos")));
    }
    public async Task<IActionResult> Carteirinha()
    {
        var usuario = await permissoes.UsuarioAsync();
        if (usuario?.AlunoId == null)
            return NotFound();
        var modelo = await documentos.CarteirinhaAsync(usuario.AlunoId.Value);
        return modelo == null ? NotFound() : View("~/Views/Carteirinhas/Visualizar.cshtml", modelo);
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using JiuManager.Services;
namespace JiuManager.Helpers;

public class FiltroPermissao(PermissaoServico permissoes) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext contexto)
    {
        if (contexto.ActionDescriptor.EndpointMetadata.OfType<IAllowAnonymous>().Any())
            return;
        if (contexto.HttpContext.User.Identity?.IsAuthenticated != true)
        {
            contexto.Result = new ChallengeResult();
            return;
        }
        var usuario = await permissoes.UsuarioAsync();
        if (usuario == null)
        {
            contexto.Result = new ChallengeResult();
            return;
        }
        var modulo = contexto.RouteData.Values["controller"]?.ToString() ?? "";
        var acao = contexto.RouteData.Values["action"]?.ToString() ?? "";
        if (modulo is "Conta" or "Portal" or "Sistema")
            return;
        if (modulo == "Dashboard" && usuario.Perfil != "Aluno")
            return;
        if (modulo == "Contexto" && usuario.Perfil != "Aluno")
            return;
        var operacao = acao == "Excluir" ? "excluir" : acao == "Salvar" ? (contexto.HttpContext.Request.HasFormContentType && int.TryParse(contexto.HttpContext.Request.Form["Id"], out var id) && id > 0 ? "editar" : "criar") : acao is "Criar" or "Registrar" or "Gerar" ? "criar" : acao == "Editar" ? "editar" : "visualizar";
        if (!await permissoes.PodeAsync(modulo, operacao))
            contexto.Result = new ForbidResult();
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using JiuManager.Models;
using JiuManager.Models.ViewModels;
using JiuManager.Services;
using JiuManager.Configuracoes;
namespace JiuManager.Controllers;

public class ContaController(AutenticacaoServico servico, ContextoEquipe contexto) : Controller
{
    [AllowAnonymous] public IActionResult Entrar(string? returnUrl) => View(new LoginModelo { Retorno = returnUrl });
    [AllowAnonymous, HttpPost, EnableRateLimiting("login")]
    public async Task<IActionResult> Entrar(LoginModelo modelo)
    {
        if (ModelState.IsValid)
        {
            var usuario = await servico.EntrarAsync(modelo.Email, modelo.Senha);
            if (usuario != null)
            {
                var identidade = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()), new Claim(ClaimTypes.Name, usuario.Nome), new Claim(ClaimTypes.Role, usuario.Perfil), new Claim("EquipeId", usuario.EquipeId.ToString()), new Claim("Sessao", Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(usuario.SenhaHash)))) }, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidade), new AuthenticationProperties { IsPersistent = false });
                if (usuario.Perfil == "Aluno")
                    return RedirectToAction("Index", "Portal");
                return !string.IsNullOrEmpty(modelo.Retorno) && Url.IsLocalUrl(modelo.Retorno) ? LocalRedirect(modelo.Retorno) : RedirectToAction("Index", "Dashboard");
            }
            ModelState.AddModelError("", "E-mail ou senha inválidos.");
        }
        return View(modelo);
    }
    [Authorize, HttpPost]
    public async Task<IActionResult> Sair()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction(nameof(Entrar));
    }
    [Authorize] public IActionResult Senha() => View();
    [Authorize, HttpPost]
    public async Task<IActionResult> Senha(string atual, string nova, string confirmacao)
    {
        try
        {
            await servico.AlterarSenhaAsync(contexto.UsuarioId, atual ?? "", nova ?? "", confirmacao ?? "");
            await HttpContext.SignOutAsync();
            TempData["Sucesso"] = "Senha alterada. Entre novamente.";
            return RedirectToAction(nameof(Entrar));
        }
        catch (RegraNegocioException erro) { ModelState.AddModelError("", erro.Message); return View(); }
    }
    [AllowAnonymous]
    public IActionResult AcessoNegado()
    {
        Response.StatusCode = 403;
        return View();
    }
}

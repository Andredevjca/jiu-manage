using System.Security.Claims;
namespace JiuManager.Configuracoes;

public class ContextoEquipe(IHttpContextAccessor acesso)
{
    public int EquipeId => int.TryParse(acesso.HttpContext?.User.FindFirstValue("EquipeId"), out var id) ? id : throw new UnauthorizedAccessException(); public int UsuarioId => int.TryParse(acesso.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : throw new UnauthorizedAccessException(); public int? UnidadeId => int.TryParse(acesso.HttpContext?.Request.Cookies["unidade"], out var id) && id > 0 ? id : null;
}

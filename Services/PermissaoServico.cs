using JiuManager.Models;
using JiuManager.Repositories;
namespace JiuManager.Services;

public class PermissaoServico(AutenticacaoRepositorio repositorio, IHttpContextAccessor acesso)
{
    private Usuario? usuario; private List<string>? adicionais;
    public async Task<Usuario?> UsuarioAsync()
    {
        if (usuario != null)
            return usuario;
        var id = acesso.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return usuario = int.TryParse(id, out var numero) ? await repositorio.PorIdAsync(numero) : null;
    }
    public async Task<bool> PodeAsync(string modulo, string acao)
    {
        var atual = await UsuarioAsync();
        if (atual == null)
            return false;
        if (atual.Perfil == "Administrador")
            return true;
        if (atual.Perfil == "Aluno")
            return false;
        adicionais ??= await repositorio.PermissoesAsync(atual.Id);
        if (adicionais.Contains(modulo.ToLowerInvariant() + "." + acao))
            return true;
        string[] permitidos = atual.Perfil switch
        {
            "Professor" => ["Alunos", "Turmas", "Matriculas", "Frequencia", "Graduacoes", "Campeonatos", "Inscricoes"],
            "Secretaria" => ["Alunos", "Professores", "Mensalidades", "Recebimentos", "Planos", "Relatorios"],
            _ => []
        };
        return permitidos.Contains(modulo) && acao != "excluir";
    }
}

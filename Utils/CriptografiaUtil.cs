using Microsoft.AspNetCore.Identity;
using JiuManager.Models;
namespace JiuManager.Utils;

public static class CriptografiaUtil
{
    private static readonly PasswordHasher<Usuario> Gerador = new(); public static string GerarHash(Usuario usuario, string senha) => Gerador.HashPassword(usuario, senha); public static bool Conferir(Usuario usuario, string senha)
    {
        try
        {
            return Gerador.VerifyHashedPassword(usuario, usuario.SenhaHash, senha) != PasswordVerificationResult.Failed;
        }
        catch (FormatException) { return false; }
    }
}

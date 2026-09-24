using System.Globalization;
using JiuManager.Configuracoes;
namespace JiuManager.Helpers;

public static class HelperFormatacao
{
    public static string Valor(object? valor, Campo campo, Dictionary<string, Dictionary<int, string>> opcoes)
    {
        if (valor == null)
            return "—";
        if (campo.Referencia != null && int.TryParse(valor.ToString(), out var id))
            return opcoes.GetValueOrDefault(campo.Referencia)?.GetValueOrDefault(id) ?? "—";
        if (valor is DateTime data)
            return data.ToString("dd/MM/yyyy");
        if (valor is decimal numero)
            return campo.Nome == "Peso" ? numero.ToString("N1") : numero.ToString("C", CultureInfo.GetCultureInfo("pt-BR"));
        if (valor is bool booleano)
            return booleano ? "Sim" : "Não";
        return valor.ToString() ?? "—";
    }
    public static string Iniciais(string nome) => string.Concat(nome.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2).Select(p => p[0])).ToUpperInvariant();
}

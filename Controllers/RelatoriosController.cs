using Microsoft.AspNetCore.Mvc;
using JiuManager.Repositories;
using JiuManager.Configuracoes;
using System.Text;
namespace JiuManager.Controllers;

public class RelatoriosController(JiuManager.Services.RelatorioServico servico) : Controller
{
    public async Task<IActionResult> Index(string tipo = "AlunosPorFaixa", string? busca = null, int pagina = 1)
    {
        var dados = await servico.DadosAsync(tipo);
        if (!string.IsNullOrWhiteSpace(busca))
            dados = dados.Where(d => d.Values.Any(v => v?.ToString()?.Contains(busca, StringComparison.OrdinalIgnoreCase) == true)).ToList();
        ViewBag.Tipo = tipo;
        ViewBag.Busca = busca;
        ViewBag.Total = dados.Count;
        ViewBag.Pagina = Math.Clamp(pagina, 1, Math.Max(1, (int)Math.Ceiling(dados.Count / 20d)));
        return View(dados.Skip(((int)ViewBag.Pagina - 1) * 20).Take(20).ToList());
    }
    public async Task<IActionResult> Exportar(string tipo = "AlunosPorFaixa")
    {
        var dados = await servico.DadosAsync(tipo);
        static string Escapar(object? valor)
        {
            var texto = valor is DateTime data ? data.ToString("dd/MM/yyyy") : valor?.ToString() ?? "";
            if (texto.Length > 0 && "=+-@\t\r".Contains(texto[0]))
                texto = "'" + texto;
            return "\"" + texto.Replace("\"", "\"\"") + "\"";
        }
        var linhas = new List<string>();
        if (dados.Count > 0)
        {
            linhas.Add(string.Join(";", dados[0].Keys.Select(Escapar)));
            linhas.AddRange(dados.Select(d => string.Join(";", d.Values.Select(Escapar))));
        }
        return File(Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(string.Join("\r\n", linhas))).ToArray(), "text/csv; charset=utf-8", "relatorio.csv");
    }
}

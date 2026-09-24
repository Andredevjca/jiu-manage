using JiuManager.Models.ViewModels;
using JiuManager.Repositories;
using JiuManager.Configuracoes;
namespace JiuManager.Services;

public class DashboardServico(ConsultaRepositorio consultas, ContextoEquipe contexto, PermissaoServico permissoes)
{
    public async Task<DashboardModelo> CarregarAsync()
    {
        var financeiro = await permissoes.PodeAsync("Mensalidades", "visualizar");
        var alunos = await consultas.ListarAsync("Alunos", true);
        var professores = await consultas.ListarAsync("Professores", true);
        var faixas = await consultas.ListarAsync("Faixas");
        var unidades = await consultas.ListarAsync("Unidades");
        var graduacoes = await consultas.ListarAsync("Graduacoes", true);
        var ids = alunos.Select(a => Convert.ToInt32(a["Id"])).ToHashSet();
        var mensalidades = (financeiro ? await consultas.ListarAsync("Mensalidades") : new List<Dictionary<string, object?>>()).Where(m => ids.Contains(Convert.ToInt32(m["AlunoId"])) && m["Status"]?.ToString() == "Pendente").ToList();
        var recebimentos = financeiro ? await consultas.ListarAsync("Recebimentos") : new List<Dictionary<string, object?>>();
        foreach (var m in mensalidades)
        {
            m["Aluno"] = alunos.First(a => Equals(a["Id"], m["AlunoId"]))["Nome"];
            m["Saldo"] = Convert.ToDecimal(m["Valor"]) - recebimentos.Where(r => Equals(r["MensalidadeId"], m["Id"])).Sum(r => Convert.ToDecimal(r["Valor"]));
            m["Status"] = Convert.ToDateTime(m["Vencimento"]) < DateTime.Today ? "Atrasado" : "Pendente";
        }
        var modelo = new DashboardModelo { ExibirFinanceiro = financeiro, Alunos = alunos.Count(a => a["Status"]?.ToString() == "Ativo"), Professores = professores.Count(p => p["Status"]?.ToString() == "Ativo"), Unidades = unidades.Count(u => u["Status"]?.ToString() == "Ativo" && (!contexto.UnidadeId.HasValue || Convert.ToInt32(u["Id"]) == contexto.UnidadeId)), Graduacoes = graduacoes.Count(g => Convert.ToDateTime(g["DataGraduacao"]).ToString("yyyy-MM") == DateTime.Today.ToString("yyyy-MM")), EmAberto = mensalidades.Sum(m => Convert.ToDecimal(m["Saldo"])), Mensalidades = mensalidades.OrderBy(m => m["Vencimento"]).Take(5).ToList(), Eventos = (await consultas.ListarAsync("Eventos", true)).Where(e => Convert.ToDateTime(e["Data"]) >= DateTime.Today).OrderBy(e => e["Data"]).Take(4).ToList() };
        modelo.Faixas = faixas.OrderBy(f => f["Ordem"]).Select(f => (f["Nome"]!.ToString()!, f["Cor"]!.ToString()!, alunos.Count(a => Equals(a["FaixaId"], f["Id"]) && a["Status"]?.ToString() == "Ativo"))).ToList();
        for (int i = 5; i >= 0; i--)
        {
            var mes = DateTime.Today.AddMonths(-i);
            var fim = new DateTime(mes.Year, mes.Month, 1).AddMonths(1);
            modelo.Evolucao.Add((mes.ToString("MMM"), alunos.Count(a => Convert.ToDateTime(a["DataInicio"]) < fim)));
        }
        foreach (var a in alunos.Where(a => a["Status"]?.ToString() == "Ativo"))
        {
            var faixa = faixas.First(f => Equals(f["Id"], a["FaixaId"]));
            var previsao = Convert.ToDateTime(a["DataUltimaGraduacao"] ?? a["DataInicio"]).AddMonths(Convert.ToInt32(faixa["MesesMinimos"]));
            a["Previsao"] = previsao;
            a["Faixa"] = faixa["Nome"];
            a["Dias"] = (previsao.Date - DateTime.Today).Days;
        }
        modelo.ProximasGraduacoes = alunos.Where(a => a.ContainsKey("Previsao")).OrderBy(a => a["Previsao"]).Take(4).ToList();
        return modelo;
    }
}


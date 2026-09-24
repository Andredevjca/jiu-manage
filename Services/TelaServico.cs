using JiuManager.Configuracoes;
using JiuManager.Models;
using JiuManager.Models.ViewModels;
using JiuManager.Repositories;
namespace JiuManager.Services;

public class TelaServico(ConsultaRepositorio consultas, CatalogoModulos catalogo, PerfilAlunoRepositorio perfil, PermissaoServico permissoes)
{
    public async Task<Dictionary<string, Dictionary<int, string>>> OpcoesAsync(Modulo modulo)
    {
        var resultado = new Dictionary<string, Dictionary<int, string>>();
        foreach (var destino in modulo.Campos.Where(c => c.Referencia != null).Select(c => c.Referencia!).Distinct())
        {
            var registros = await consultas.ListarAsync(destino);
            Dictionary<int, string> alunos = destino == "Mensalidades" ? (await consultas.ListarAsync("Alunos")).ToDictionary(r => Convert.ToInt32(r["Id"]), r => r["Nome"]?.ToString() ?? "") : [];
            resultado[destino] = registros.ToDictionary(r => Convert.ToInt32(r["Id"]), r => r.ContainsKey("Nome") ? r["Nome"]?.ToString() ?? "" : $"#{r["Id"]} · {alunos.GetValueOrDefault(Convert.ToInt32(r.GetValueOrDefault("AlunoId")), "")} · {r.GetValueOrDefault("Competencia")}");
        }
        return resultado;
    }
    public async Task<ListaModelo> ListaAsync(string modulo, string? busca, int pagina, string? status)
    {
        var definicao = catalogo.Obter(modulo);
        var registros = await consultas.ListarAsync(modulo, true);
        var opcoes = await OpcoesAsync(definicao);
        if (modulo == "Mensalidades")
            foreach (var r in registros)
                if (r["Status"]?.ToString() == "Pendente" && Convert.ToDateTime(r["Vencimento"]) < DateTime.Today)
                    r["Status"] = "Atrasado";
        if (!string.IsNullOrWhiteSpace(busca))
            registros = registros.Where(r => definicao.Campos.Any(c => Helpers.HelperFormatacao.Valor(r.GetValueOrDefault(c.Nome), c, opcoes).Contains(busca, StringComparison.OrdinalIgnoreCase))).ToList();
        if (!string.IsNullOrWhiteSpace(status))
            registros = registros.Where(r => r.GetValueOrDefault("Status")?.ToString() == status).ToList();
        var total = registros.Count;
        pagina = Math.Clamp(pagina, 1, Math.Max(1, (int)Math.Ceiling(total / 10d)));
        return new()
        {
            Modulo = definicao,
            Registros = registros.Skip((pagina - 1) * 10).Take(10).ToList(),
            Total = total,
            Pagina = pagina,
            Busca = busca ?? "",
            Status = status ?? "",
            Opcoes = opcoes
        };
    }
    public async Task<DetalhesModelo> DetalhesAsync(Registro registro, Modulo modulo)
    {
        var tela = new DetalhesModelo { Registro = registro, Modulo = modulo, Opcoes = await OpcoesAsync(modulo) };
        if (registro is Aluno aluno)
        {
            tela.Graduacoes = await perfil.GraduacoesAsync(aluno.Id);
            tela.Mensalidades = ((await permissoes.UsuarioAsync())?.Perfil == "Aluno" || await permissoes.PodeAsync("Mensalidades", "visualizar") ? await consultas.ListarAsync("Mensalidades") : new List<Dictionary<string, object?>>()).Where(r => Convert.ToInt32(r["AlunoId"]) == aluno.Id).ToList();
            tela.Campeonatos = await perfil.CampeonatosAsync(aluno.Id);
            var presencas = (await consultas.ListarAsync("Frequencia")).Where(r => Convert.ToInt32(r["AlunoId"]) == aluno.Id).ToList();
            tela.Frequencia = presencas.Count == 0 ? 0 : Math.Round(presencas.Count(r => Convert.ToBoolean(r["Presente"])) * 100m / presencas.Count, 1);
        }
        return tela;
    }
}

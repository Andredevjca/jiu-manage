namespace JiuManager.Helpers;

public record CobrancaAluno(int Id, int AlunoId, string Competencia, DateTime Vencimento,
    decimal Valor, decimal Recebido, decimal Saldo, string Status)
{
    public string Classe => Status == "Pago" ? "positivo" : Status == "Atrasado" ? "negativo" : "neutro";
}

public record ResumoFinanceiroAluno(string Situacao, decimal EmAberto, decimal EmAtraso, decimal Recebido)
{
    public string Classe => Situacao == "Em dia" ? "positivo" : Situacao == "Em atraso" ? "negativo" : "neutro";
}

public static class FinanceiroAluno
{
    public static List<CobrancaAluno> Calcular(IEnumerable<Dictionary<string, object?>> mensalidades,
        IEnumerable<Dictionary<string, object?>> recebimentos, DateTime hoje)
    {
        var pagos = recebimentos.GroupBy(r => Convert.ToInt32(r["MensalidadeId"]))
            .ToDictionary(g => g.Key, g => g.Sum(r => Convert.ToDecimal(r["Valor"])));
        return mensalidades.Select(m =>
        {
            var id = Convert.ToInt32(m["Id"]);
            var valor = Convert.ToDecimal(m["Valor"]);
            var recebido = pagos.GetValueOrDefault(id);
            var vencimento = Convert.ToDateTime(m["Vencimento"]).Date;
            var cancelada = m["Status"]?.ToString() == "Cancelado";
            var saldo = cancelada ? 0 : Math.Max(0, valor - recebido);
            var status = cancelada ? "Cancelado" : saldo == 0 ? "Pago" : vencimento < hoje.Date ? "Atrasado" : recebido > 0 ? "Parcial" : "Pendente";
            return new CobrancaAluno(id, Convert.ToInt32(m["AlunoId"]), m["Competencia"]?.ToString() ?? "",
                vencimento, valor, recebido, saldo, status);
        }).OrderByDescending(m => m.Vencimento).ToList();
    }

    public static ResumoFinanceiroAluno Resumir(IEnumerable<CobrancaAluno> mensalidades)
    {
        var validas = mensalidades.Where(m => m.Status != "Cancelado").ToList();
        var aberto = validas.Sum(m => m.Saldo);
        var atraso = validas.Where(m => m.Status == "Atrasado").Sum(m => m.Saldo);
        var situacao = validas.Count == 0 ? "Sem cobranças" : atraso > 0 ? "Em atraso" : aberto > 0 ? "A vencer" : "Em dia";
        return new(situacao, aberto, atraso, validas.Sum(m => m.Recebido));
    }
}

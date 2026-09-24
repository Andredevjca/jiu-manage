using JiuManager.Configuracoes;
namespace JiuManager.Repositories;

public class RelatorioRepositorio(ConsultaRepositorio consultas, ContextoEquipe contexto)
{
    public async Task<List<Dictionary<string, object?>>> DadosAsync(string tipo)
    {
        var filtro = " AND (@UnidadeId IS NULL OR a.UnidadeId=@UnidadeId)";
        var sql = tipo switch
        {
            "Graduacoes" => "SELECT a.Nome Aluno,f.Nome Faixa,gr.Nome Grau,g.DataGraduacao Data,p.Nome Professor FROM graduacoes g JOIN alunos a ON a.Id=g.AlunoId JOIN faixas f ON f.Id=g.FaixaNovaId JOIN graus gr ON gr.Id=g.GrauNovoId JOIN professores p ON p.Id=g.ProfessorId WHERE g.EquipeId=@EquipeId" + filtro + " ORDER BY g.DataGraduacao DESC",
            "Frequencia" => "SELECT a.Nome Aluno,COUNT(*) Aulas,SUM(f.Presente) Presenças,ROUND(AVG(f.Presente)*100,1) Percentual FROM frequencias f JOIN alunos a ON a.Id=f.AlunoId WHERE f.EquipeId=@EquipeId" + filtro + " GROUP BY a.Id,a.Nome",
            "Financeiro" => "SELECT a.Nome Aluno,m.Competencia Competência,m.Vencimento,m.Valor,COALESCE(r.Recebido,0) Recebido,m.Valor-COALESCE(r.Recebido,0) Saldo,CASE WHEN m.Status='Pendente' AND m.Vencimento<CURRENT_DATE THEN 'Atrasado' ELSE m.Status END Status FROM mensalidades m JOIN alunos a ON a.Id=m.AlunoId LEFT JOIN (SELECT MensalidadeId,SUM(Valor) Recebido FROM recebimentos GROUP BY MensalidadeId) r ON r.MensalidadeId=m.Id WHERE m.EquipeId=@EquipeId" + filtro + " ORDER BY m.Vencimento DESC",
            "Atletas" => "SELECT a.Nome Atleta,c.Nome Campeonato,i.Categoria,i.Resultado FROM inscricoes_campeonatos i JOIN alunos a ON a.Id=i.AlunoId JOIN campeonatos c ON c.Id=i.CampeonatoId WHERE i.EquipeId=@EquipeId" + filtro,
            _ => "SELECT f.Nome Faixa,COUNT(a.Id) Alunos FROM faixas f LEFT JOIN alunos a ON a.FaixaId=f.Id AND a.EquipeId=f.EquipeId AND a.Status='Ativo' AND (@UnidadeId IS NULL OR a.UnidadeId=@UnidadeId) WHERE f.EquipeId=@EquipeId GROUP BY f.Id,f.Nome,f.Ordem ORDER BY f.Ordem"
        };
        return await consultas.ConsultarAsync(sql, new
        {
            contexto.EquipeId,
            contexto.UnidadeId
        });
    }
}

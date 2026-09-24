using Dapper;
using JiuManager.Models;
using JiuManager.Configuracoes;
using MySqlConnector;
namespace JiuManager.Repositories;

public class OperacaoRepositorio(FabricaConexao conexoes, ContextoEquipe contexto)
{
    public async Task<int> GraduarAsync(Graduacao graduacao)
    {
        await using var conexao = conexoes.Criar();
        await conexao.OpenAsync();
        await using var transacao = await conexao.BeginTransactionAsync();
        var aluno = await conexao.QuerySingleOrDefaultAsync<Aluno>("SELECT * FROM alunos WHERE Id=@AlunoId AND EquipeId=@EquipeId FOR UPDATE", new
        {
            graduacao.AlunoId,
            contexto.EquipeId
        }, transacao) ?? throw new RegraNegocioException("Aluno não encontrado.");
        if (aluno.Status != "Ativo")
            throw new RegraNegocioException("O aluno precisa estar ativo para receber graduação.");
        if (graduacao.DataGraduacao.Date > DateTime.Today || graduacao.DataGraduacao.Date < (aluno.DataUltimaGraduacao ?? aluno.DataInicio).Date)
            throw new RegraNegocioException("A graduação deve ser posterior à última graduação e não pode estar no futuro.");
        var ordemAnterior = await conexao.ExecuteScalarAsync<int>("SELECT Ordem FROM faixas WHERE Id=@FaixaId", aluno, transacao);
        var ordemNova = await conexao.ExecuteScalarAsync<int>("SELECT Ordem FROM faixas WHERE Id=@FaixaNovaId", graduacao, transacao);
        var grauAnterior = await conexao.ExecuteScalarAsync<int>("SELECT Numero FROM graus WHERE Id=@GrauId", aluno, transacao);
        var grauNovo = await conexao.ExecuteScalarAsync<int>("SELECT Numero FROM graus WHERE Id=@GrauNovoId", graduacao, transacao);
        if (ordemNova < ordemAnterior || (ordemNova == ordemAnterior && grauNovo <= grauAnterior))
            throw new RegraNegocioException("A graduação deve avançar a faixa ou o grau atual.");
        graduacao.EquipeId = contexto.EquipeId;
        graduacao.FaixaAnteriorId = aluno.FaixaId;
        graduacao.GrauAnteriorId = aluno.GrauId;
        graduacao.UnidadeId = aluno.UnidadeId;
        var id = await conexao.ExecuteScalarAsync<int>("INSERT INTO graduacoes(EquipeId,AlunoId,ProfessorId,UnidadeId,FaixaAnteriorId,GrauAnteriorId,FaixaNovaId,GrauNovoId,DataGraduacao,Observacoes) VALUES(@EquipeId,@AlunoId,@ProfessorId,@UnidadeId,@FaixaAnteriorId,@GrauAnteriorId,@FaixaNovaId,@GrauNovoId,@DataGraduacao,@Observacoes); SELECT LAST_INSERT_ID();", graduacao, transacao);
        await conexao.ExecuteAsync("UPDATE alunos SET FaixaId=@FaixaNovaId,GrauId=@GrauNovoId,DataUltimaGraduacao=@DataGraduacao WHERE Id=@AlunoId AND EquipeId=@EquipeId", graduacao, transacao);
        await transacao.CommitAsync();
        return id;
    }
    public async Task<int> ReceberAsync(Recebimento recebimento)
    {
        await using var conexao = conexoes.Criar();
        await conexao.OpenAsync();
        await using var transacao = await conexao.BeginTransactionAsync();
        var mensalidade = await conexao.QuerySingleOrDefaultAsync<Mensalidade>("SELECT * FROM mensalidades WHERE Id=@MensalidadeId AND EquipeId=@EquipeId FOR UPDATE", new
        {
            recebimento.MensalidadeId,
            contexto.EquipeId
        }, transacao) ?? throw new RegraNegocioException("Mensalidade não encontrada.");
        if (mensalidade.Status is "Pago" or "Cancelado")
            throw new RegraNegocioException("Esta mensalidade não permite recebimentos.");
        var recebido = await conexao.ExecuteScalarAsync<decimal>("SELECT COALESCE(SUM(Valor),0) FROM recebimentos WHERE MensalidadeId=@Id AND EquipeId=@EquipeId", mensalidade, transacao);
        if (recebimento.Valor > mensalidade.Valor - recebido)
            throw new RegraNegocioException($"O valor excede o saldo de {(mensalidade.Valor - recebido):C}.");
        recebimento.EquipeId = contexto.EquipeId;
        var id = await conexao.ExecuteScalarAsync<int>("INSERT INTO recebimentos(EquipeId,MensalidadeId,Data,Valor,FormaPagamento,Observacoes) VALUES(@EquipeId,@MensalidadeId,@Data,@Valor,@FormaPagamento,@Observacoes);SELECT LAST_INSERT_ID();", recebimento, transacao);
        if (recebido + recebimento.Valor == mensalidade.Valor)
            await conexao.ExecuteAsync("UPDATE mensalidades SET Status='Pago' WHERE Id=@Id AND EquipeId=@EquipeId", mensalidade, transacao);
        await transacao.CommitAsync();
        return id;
    }
    public async Task<int> MatricularAsync(Matricula matricula)
    {
        await using var conexao = conexoes.Criar();
        await conexao.OpenAsync();
        await using var transacao = await conexao.BeginTransactionAsync();
        var turma = await conexao.QuerySingleOrDefaultAsync<Turma>("SELECT * FROM turmas WHERE Id=@TurmaId AND EquipeId=@EquipeId FOR UPDATE", new
        {
            matricula.TurmaId,
            contexto.EquipeId
        }, transacao) ?? throw new RegraNegocioException("Turma não encontrada.");
        var aluno = await conexao.QuerySingleAsync<Aluno>("SELECT * FROM alunos WHERE Id=@AlunoId AND EquipeId=@EquipeId", new
        {
            matricula.AlunoId,
            contexto.EquipeId
        }, transacao);
        if (turma.Status != "Ativo" || aluno.Status != "Ativo" || aluno.UnidadeId != turma.UnidadeId)
            throw new RegraNegocioException("Selecione aluno e turma ativos da mesma unidade.");
        var total = await conexao.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM turma_alunos WHERE TurmaId=@TurmaId AND Id<>@Id", matricula, transacao);
        if (total >= turma.Capacidade)
            throw new RegraNegocioException("A turma atingiu a capacidade máxima.");
        var ordem = await conexao.ExecuteScalarAsync<int>("SELECT Ordem FROM faixas WHERE Id=@FaixaId", aluno, transacao);
        var minima = await conexao.ExecuteScalarAsync<int>("SELECT Ordem FROM faixas WHERE Id=@FaixaMinimaId", turma, transacao);
        var maxima = await conexao.ExecuteScalarAsync<int>("SELECT Ordem FROM faixas WHERE Id=@FaixaMaximaId", turma, transacao);
        if (ordem < minima || ordem > maxima)
            throw new RegraNegocioException("A faixa do aluno está fora do intervalo da turma.");
        matricula.EquipeId = contexto.EquipeId;
        if (matricula.Id > 0)
            throw new RegraNegocioException("Remova a matrícula e registre o novo vínculo.");
        var id = await conexao.ExecuteScalarAsync<int>("INSERT INTO turma_alunos(EquipeId,TurmaId,AlunoId) VALUES(@EquipeId,@TurmaId,@AlunoId);SELECT LAST_INSERT_ID();", matricula, transacao);
        await transacao.CommitAsync();
        return id;
    }
    public async Task<List<Aluno>> AlunosTurmaAsync(int turmaId)
    {
        await using var conexao = conexoes.Criar();
        return (await conexao.QueryAsync<Aluno>("SELECT a.* FROM alunos a JOIN turma_alunos t ON t.AlunoId=a.Id AND t.EquipeId=a.EquipeId WHERE t.TurmaId=@turmaId AND a.EquipeId=@EquipeId AND a.Status='Ativo' ORDER BY a.Nome", new
        {
            turmaId,
            contexto.EquipeId
        })).ToList();
    }
    public async Task<HashSet<int>> PresentesAsync(int turmaId, DateTime data)
    {
        await using var conexao = conexoes.Criar();
        return (await conexao.QueryAsync<int>("SELECT AlunoId FROM frequencias WHERE EquipeId=@EquipeId AND TurmaId=@turmaId AND Data=@data AND Presente=1", new
        {
            contexto.EquipeId,
            turmaId,
            data = data.Date
        })).ToHashSet();
    }
    public async Task RegistrarChamadaAsync(int turmaId, DateTime data, int[] presentes)
    {
        await using var conexao = conexoes.Criar();
        await conexao.OpenAsync();
        await using var transacao = await conexao.BeginTransactionAsync();
        var turma = await conexao.QuerySingleOrDefaultAsync<Turma>("SELECT * FROM turmas WHERE Id=@turmaId AND EquipeId=@EquipeId FOR UPDATE", new
        {
            turmaId,
            contexto.EquipeId
        }, transacao) ?? throw new RegraNegocioException("Turma não encontrada.");
        if (turma.Status != "Ativo")
            throw new RegraNegocioException("Selecione uma turma ativa.");
        var alunos = (await conexao.QueryAsync<int>("SELECT a.Id FROM alunos a JOIN turma_alunos t ON t.AlunoId=a.Id WHERE t.TurmaId=@turmaId AND a.EquipeId=@EquipeId AND a.Status='Ativo'", new
        {
            turmaId,
            contexto.EquipeId
        }, transacao)).ToHashSet();
        if (presentes.Any(id => !alunos.Contains(id)))
            throw new RegraNegocioException("A chamada contém alunos que não pertencem à turma.");
        if (alunos.Count == 0)
            throw new RegraNegocioException("Matricule alunos nesta turma antes de registrar a chamada.");
        foreach (var alunoId in alunos)
            await conexao.ExecuteAsync("INSERT INTO frequencias(EquipeId,TurmaId,AlunoId,UnidadeId,Data,Presente) VALUES(@EquipeId,@turmaId,@alunoId,@UnidadeId,@data,@presente) ON DUPLICATE KEY UPDATE Presente=@presente", new
            {
                contexto.EquipeId,
                turmaId,
                alunoId,
                turma.UnidadeId,
                data = data.Date,
                presente = presentes.Contains(alunoId)
            }, transacao);
        await transacao.CommitAsync();
    }
    public async Task<int> SalvarMensalidadeAsync(Mensalidade mensalidade)
    {
        await using var conexao = conexoes.Criar();
        await conexao.OpenAsync();
        await using var transacao = await conexao.BeginTransactionAsync();
        mensalidade.EquipeId = contexto.EquipeId;
        if (mensalidade.Id > 0)
        {
            var anterior = await conexao.QuerySingleOrDefaultAsync<Mensalidade>("SELECT * FROM mensalidades WHERE Id=@Id AND EquipeId=@EquipeId FOR UPDATE", mensalidade, transacao) ?? throw new RegraNegocioException("Mensalidade não encontrada.");
            var recebimentos = await conexao.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM recebimentos WHERE MensalidadeId=@Id", mensalidade, transacao);
            if (recebimentos > 0)
                throw new RegraNegocioException("Mensalidades com recebimentos não podem ser alteradas.");
            await conexao.ExecuteAsync("UPDATE mensalidades SET AlunoId=@AlunoId,Competencia=@Competencia,Vencimento=@Vencimento,Valor=@Valor,Status=@Status,Observacoes=@Observacoes WHERE Id=@Id AND EquipeId=@EquipeId", mensalidade, transacao);
        }
        else
            mensalidade.Id = await conexao.ExecuteScalarAsync<int>("INSERT INTO mensalidades(EquipeId,AlunoId,Competencia,Vencimento,Valor,Status,Observacoes) VALUES(@EquipeId,@AlunoId,@Competencia,@Vencimento,@Valor,@Status,@Observacoes);SELECT LAST_INSERT_ID();", mensalidade, transacao);
        await transacao.CommitAsync();
        return mensalidade.Id;
    }
    public async Task<int> GerarMensalidadesAsync(DateTime competencia)
    {
        await using var conexao = conexoes.Criar();
        return await conexao.ExecuteAsync("INSERT IGNORE INTO mensalidades(EquipeId,AlunoId,Competencia,Vencimento,Valor,Status,Observacoes) SELECT a.EquipeId,a.Id,@referencia,DATE_ADD(@inicio,INTERVAL (p.DiaVencimento-1) DAY),p.Valor,'Pendente','Gerada pelo plano' FROM alunos a JOIN planos p ON p.Id=a.PlanoId AND p.EquipeId=a.EquipeId WHERE a.EquipeId=@EquipeId AND a.Status='Ativo' AND p.Status='Ativo' AND NOT EXISTS(SELECT 1 FROM mensalidades m WHERE m.EquipeId=a.EquipeId AND m.AlunoId=a.Id AND m.Competencia=@referencia)", new
        {
            contexto.EquipeId,
            referencia = competencia.ToString("yyyy-MM"),
            inicio = new DateTime(competencia.Year, competencia.Month, 1)
        });
    }
}


using Dapper;
using MySqlConnector;
using JiuManager.Models;
using JiuManager.Utils;
namespace JiuManager.Dados;

public static class DadosIniciais
{
    public static async Task InserirAsync(MySqlConnection conexao, bool demonstracao)
    {
        await using var transacao = await conexao.BeginTransactionAsync();
        await conexao.ExecuteAsync("INSERT INTO equipes(Id,EquipeId,Nome,NomeFantasia,Logo,Telefone,Whatsapp,Email,Site,Cep,Endereco,Numero,Bairro,Cidade,Estado,Status) VALUES(1,1,'JiuManager','JiuManager','','','','contato@equipe.com','','','','','','Fortaleza','CE','Ativo')", transaction: transacao);
        var administrador = new Usuario { Nome = "Administrador", Email = "admin@admin.com", Perfil = "Administrador", EquipeId = 1, Status = "Ativo" };
        administrador.SenhaHash = CriptografiaUtil.GerarHash(administrador, "admin");
        await conexao.ExecuteAsync("INSERT INTO usuarios(EquipeId,Nome,Email,Perfil,AlunoId,Status,SenhaHash) VALUES(@EquipeId,@Nome,@Email,@Perfil,NULL,@Status,@SenhaHash)", administrador, transacao);
        string[] faixas = ["Branca", "Azul", "Roxa", "Marrom", "Preta"];
        string[] cores = ["#dbe1e9", "#4878df", "#8955bb", "#906649", "#252a36"];
        for (int i = 0; i < 5; i++)
            await conexao.ExecuteAsync("INSERT INTO faixas(Id,EquipeId,Nome,Cor,Ordem,MesesMinimos) VALUES(@id,1,@nome,@cor,@ordem,12)", new
            {
                id = i + 1,
                nome = faixas[i],
                cor = cores[i],
                ordem = i
            }, transacao);
        for (int i = 0; i <= 4; i++)
            await conexao.ExecuteAsync("INSERT INTO graus(Id,EquipeId,Nome,Numero) VALUES(@id,1,@nome,@numero)", new
            {
                id = i + 1,
                nome = i == 0 ? "Sem grau" : $"Grau {i}",
                numero = i
            }, transacao);
        await conexao.ExecuteAsync("INSERT INTO planos(Id,EquipeId,Nome,Valor,DiaVencimento,Status) VALUES(1,1,'Essencial · 3x por semana',150,10,'Ativo'),(2,1,'Ilimitado',220,10,'Ativo')", transaction: transacao);
        string[] unidades = demonstracao ? ["Matriz", "Aldeota", "Eusébio"] : ["Matriz"];
        for (int i = 0; i < unidades.Length; i++)
            await conexao.ExecuteAsync("INSERT INTO unidades(Id,EquipeId,Nome,Tipo,Telefone,Whatsapp,Email,Cep,Endereco,Numero,Complemento,Bairro,Cidade,Estado,Status) VALUES(@id,1,@nome,@tipo,'','','','','','','','','Fortaleza','CE','Ativo')", new
            {
                id = i + 1,
                nome = unidades[i],
                tipo = i == 0 ? "Matriz" : "Filial"
            }, transacao);
        if (demonstracao)
        {
            string[] professores = ["Rafael Mendes", "André Costa", "Mariana Lima", "Bruno Almeida", "Felipe Rocha"];
            for (int i = 0; i < 5; i++)
                await conexao.ExecuteAsync("INSERT INTO professores(Id,EquipeId,Nome,UnidadeId,Cpf,Telefone,Whatsapp,Email,Foto,FaixaId,GrauId,DataEntrada,Status,Observacoes) VALUES(@id,1,@nome,@unidade,'','','',@email,'',5,3,@data,'Ativo','Dado de demonstração')", new
                {
                    id = i + 1,
                    nome = professores[i],
                    unidade = i % 3 + 1,
                    email = $"professor{i + 1}@exemplo.com",
                    data = DateTime.Today.AddYears(-3)
                }, transacao);
            string[] alunos = ["Lucas Oliveira", "Beatriz Santos", "Gabriel Souza", "Ana Carolina", "Pedro Henrique", "Mariana Ribeiro", "João Victor", "Camila Almeida", "Matheus Costa", "Isabela Lima", "Rafael Martins", "Julia Ferreira", "Gustavo Rocha", "Larissa Gomes", "Felipe Barbosa", "Amanda Dias", "Bruno Teixeira", "Letícia Moreira", "Diego Nascimento", "Fernanda Alves"];
            for (int i = 0; i < 20; i++)
            {
                int faixa = i < 7 ? 1 : i < 13 ? 2 : i < 17 ? 3 : i < 19 ? 4 : 5;
                await conexao.ExecuteAsync("INSERT INTO alunos(Id,EquipeId,Nome,UnidadeId,Cpf,DataNascimento,Sexo,Telefone,Whatsapp,Email,Cep,Endereco,Numero,Bairro,Cidade,Estado,Foto,DataInicio,FaixaId,GrauId,DataUltimaGraduacao,ProfessorResponsavelId,PlanoId,Status,Observacoes,CodigoPublico) VALUES(@id,1,@nome,@unidade,'',@nascimento,'Não informado','','',@email,'','','','','Fortaleza','CE','',@inicio,@faixa,2,@graduacao,@professor,1,'Ativo','Dado de demonstração',@codigo)", new
                {
                    id = i + 1,
                    nome = alunos[i],
                    unidade = i % 3 + 1,
                    nascimento = DateTime.Today.AddYears(-20 - i),
                    email = $"aluno{i + 1}@exemplo.com",
                    inicio = DateTime.Today.AddMonths(-24 + i),
                    faixa,
                    graduacao = DateTime.Today.AddMonths(-11).AddDays(i * 2 - 20),
                    professor = i % 3 + 1,
                    codigo = Guid.NewGuid().ToString("N")
                }, transacao);
                await conexao.ExecuteAsync("INSERT INTO graduacoes(EquipeId,AlunoId,ProfessorId,UnidadeId,FaixaAnteriorId,GrauAnteriorId,FaixaNovaId,GrauNovoId,DataGraduacao,Observacoes) VALUES(1,@id,@professor,@unidade,@faixa,1,@faixa,2,@data,'Histórico de demonstração')", new
                {
                    id = i + 1,
                    professor = i % 3 + 1,
                    unidade = i % 3 + 1,
                    faixa,
                    data = DateTime.Today.AddMonths(-11).AddDays(i * 2 - 20)
                }, transacao);
                for (int mes = 0; mes < 3; mes++)
                {
                    var vencimento = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 10).AddMonths(-mes);
                    var pago = mes > 0 || i % 3 == 0;
                    var mensalidadeId = await conexao.ExecuteScalarAsync<int>("INSERT INTO mensalidades(EquipeId,AlunoId,Competencia,Vencimento,Valor,Status,Observacoes) VALUES(1,@aluno,@competencia,@vencimento,150,@status,'Dado de demonstração');SELECT LAST_INSERT_ID();", new
                    {
                        aluno = i + 1,
                        competencia = vencimento.ToString("yyyy-MM"),
                        vencimento,
                        status = pago ? "Pago" : "Pendente"
                    }, transacao);
                    if (pago)
                        await conexao.ExecuteAsync("INSERT INTO recebimentos(EquipeId,MensalidadeId,Data,Valor,FormaPagamento,Observacoes) VALUES(1,@mensalidadeId,@data,150,'PIX','Dado de demonstração')", new
                        {
                            mensalidadeId,
                            data = vencimento.AddDays(-2)
                        }, transacao);
                }
            }
            for (int i = 1; i <= 3; i++)
            {
                await conexao.ExecuteAsync("INSERT INTO turmas(Id,EquipeId,Nome,UnidadeId,ProfessorId,Descricao,DiaSemana,HoraInicio,HoraFim,FaixaMinimaId,FaixaMaximaId,Capacidade,Status) VALUES(@id,1,@nome,@id,@id,'Fundamentos e evolução técnica','Segunda-feira','19:00','20:30',1,5,30,'Ativo')", new
                {
                    id = i,
                    nome = new[] { "Adulto · Noite", "Competição", "Fundamentos" }[i - 1]
                }, transacao);
                await conexao.ExecuteAsync("INSERT INTO turma_alunos(EquipeId,TurmaId,AlunoId) SELECT 1,@id,Id FROM alunos WHERE UnidadeId=@id", new
                {
                    id = i
                }, transacao);
                for (int dia = 1; dia <= 6; dia++)
                    await conexao.ExecuteAsync("INSERT INTO frequencias(EquipeId,TurmaId,AlunoId,UnidadeId,Data,Presente) SELECT 1,@id,Id,@id,@data,MOD(Id+@dia,5)<>0 FROM alunos WHERE UnidadeId=@id", new
                    {
                        id = i,
                        data = DateTime.Today.AddDays(-dia * 2),
                        dia
                    }, transacao);
            }
            await conexao.ExecuteAsync("INSERT INTO campeonatos(Id,EquipeId,Nome,Data,Local,Cidade,Organizacao,Status,Observacoes) VALUES(1,1,'Open de Jiu-Jitsu',@data,'Ginásio municipal','Fortaleza','Federação local','Inscrições abertas','Dado de demonstração')", new
            {
                data = DateTime.Today.AddDays(28)
            }, transacao);
            await conexao.ExecuteAsync("INSERT INTO inscricoes_campeonatos(EquipeId,CampeonatoId,AlunoId,Categoria,FaixaId,Peso,Idade,Resultado,Observacoes) VALUES(1,1,1,'Adulto leve',1,74,22,'Inscrito','')", transaction: transacao);
            string[] eventos = ["Seminário de guarda", "Cerimônia de graduação", "Treino aberto da equipe"];
            for (int i = 0; i < 3; i++)
                await conexao.ExecuteAsync("INSERT INTO eventos(EquipeId,Nome,Tipo,Data,Local,UnidadeId,Observacoes) VALUES(1,@nome,@tipo,@data,'Tatame principal',1,'Dado de demonstração')", new
                {
                    nome = eventos[i],
                    tipo = i == 1 ? "Graduação" : "Seminário",
                    data = DateTime.Today.AddDays(5 + i * 7)
                }, transacao);
        }
        await transacao.CommitAsync();
    }
}

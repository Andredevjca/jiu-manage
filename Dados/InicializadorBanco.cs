using Dapper;
using MySqlConnector;
using JiuManager.Configuracoes;
using JiuManager.Models;
using JiuManager.Utils;
namespace JiuManager.Dados;

public static class InicializadorBanco
{
    public static async Task ExecutarAsync(IServiceProvider servicos, IConfiguration configuracao, IWebHostEnvironment ambiente)
    {
        var csBuilder = new MySqlConnectionStringBuilder(configuracao.GetConnectionString("Banco") ?? throw new InvalidOperationException("Configure a conexão Banco."));
        var banco = csBuilder.Database;
        if (!System.Text.RegularExpressions.Regex.IsMatch(banco, "^[a-zA-Z0-9_]+$"))
            throw new InvalidOperationException("Nome do banco inválido.");
        
        csBuilder.Database = "";
        await using var conexao = new MySqlConnection(csBuilder.ConnectionString);
        await conexao.OpenAsync();
        await conexao.ExecuteAsync($"CREATE DATABASE IF NOT EXISTS `{banco}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci");
        await conexao.ChangeDatabaseAsync(banco);
        
        var bloqueio = await conexao.ExecuteScalarAsync<int>("SELECT GET_LOCK('jiumanager_inicializacao',30)");
        if (bloqueio != 1)
            throw new InvalidOperationException("Outra instância está inicializando o banco.");
        try
        {
            await conexao.ExecuteAsync(await File.ReadAllTextAsync(Path.Combine(ambiente.ContentRootPath, "Dados/esquema.sql")));
            var catalogo = servicos.GetRequiredService<CatalogoModulos>();
            foreach (var modulo in catalogo.Todos)
            {
                var relacoes = modulo.Campos.Where(c => c.Referencia != null).Select(c => (Campo: c.Nome, Tabela: catalogo.Obter(c.Referencia!).Tabela)).ToList();
                if (modulo.Nome != "Equipe")
                    relacoes.Add(("EquipeId", "equipes"));
                foreach (var relacao in relacoes)
                {
                    var nome = $"fk_{modulo.Tabela}_{relacao.Campo}";
                    var existe = await conexao.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_SCHEMA=@banco AND TABLE_NAME=@tabela AND CONSTRAINT_NAME=@nome", new
                    {
                        banco,
                        tabela = modulo.Tabela,
                        nome
                    });
                    if (existe == 0)
                        await conexao.ExecuteAsync($"ALTER TABLE `{modulo.Tabela}` ADD CONSTRAINT `{nome}` FOREIGN KEY (`{relacao.Campo}`) REFERENCES `{relacao.Tabela}` (Id)");
                }
            }
            foreach (var modulo in catalogo.Todos.Select(m => m.Plural).Concat(new[] { "Relatorios", "Carteirinhas", "Certificados", "Configuracoes", "Permissoes" }))
                foreach (var acao in new[] { "visualizar", "criar", "editar", "excluir" })
                    await conexao.ExecuteAsync("INSERT IGNORE INTO permissoes(Nome) VALUES(@nome)", new
                    {
                        nome = modulo.ToLowerInvariant() + "." + acao
                    });
            if (await conexao.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM equipes") == 0)
                await DadosIniciais.InserirAsync(conexao, configuracao.GetValue<bool>("Dados:Demonstracao"));
        }
        finally { await conexao.ExecuteAsync("SELECT RELEASE_LOCK('jiumanager_inicializacao')"); }
    }
}


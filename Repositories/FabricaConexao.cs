using MySqlConnector;
namespace JiuManager.Repositories;

public class FabricaConexao(IConfiguration configuracao)
{
    public MySqlConnection Criar() => new(configuracao.GetConnectionString("Banco") ?? throw new InvalidOperationException("Configure a conexão Banco."));
}


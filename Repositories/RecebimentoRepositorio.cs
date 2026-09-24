using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
namespace JiuManager.Repositories;

public class RecebimentoRepositorio(FabricaConexao conexoes, ContextoEquipe contexto, CatalogoModulos catalogo) : Repositorio<Recebimento>(conexoes, contexto, catalogo), IRecebimentoRepositorio
{
}

using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
namespace JiuManager.Repositories;

public class UnidadeRepositorio(FabricaConexao conexoes, ContextoEquipe contexto, CatalogoModulos catalogo) : Repositorio<Unidade>(conexoes, contexto, catalogo), IUnidadeRepositorio
{
}

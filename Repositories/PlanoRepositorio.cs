using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
namespace JiuManager.Repositories;

public class PlanoRepositorio(FabricaConexao conexoes, ContextoEquipe contexto, CatalogoModulos catalogo) : Repositorio<Plano>(conexoes, contexto, catalogo), IPlanoRepositorio
{
}

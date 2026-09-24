using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
namespace JiuManager.Repositories;

public class MatriculaRepositorio(FabricaConexao conexoes, ContextoEquipe contexto, CatalogoModulos catalogo) : Repositorio<Matricula>(conexoes, contexto, catalogo), IMatriculaRepositorio
{
}

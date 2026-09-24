using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
namespace JiuManager.Repositories;

public class GraduacaoRepositorio(FabricaConexao conexoes, ContextoEquipe contexto, CatalogoModulos catalogo) : Repositorio<Graduacao>(conexoes, contexto, catalogo), IGraduacaoRepositorio
{
}

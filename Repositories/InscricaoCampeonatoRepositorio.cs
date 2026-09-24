using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
namespace JiuManager.Repositories;

public class InscricaoCampeonatoRepositorio(FabricaConexao conexoes, ContextoEquipe contexto, CatalogoModulos catalogo) : Repositorio<InscricaoCampeonato>(conexoes, contexto, catalogo), IInscricaoCampeonatoRepositorio
{
}

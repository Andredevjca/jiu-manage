using JiuManager.Repositories;
using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
namespace JiuManager.Services;

public class CampeonatoServico(ICampeonatoRepositorio repositorio, ConsultaRepositorio consultas, CatalogoModulos catalogo) : CadastroServico<Campeonato>(repositorio, consultas, catalogo)
{
}

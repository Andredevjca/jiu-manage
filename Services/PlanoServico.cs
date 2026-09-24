using JiuManager.Repositories;
using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
namespace JiuManager.Services;

public class PlanoServico(IPlanoRepositorio repositorio, ConsultaRepositorio consultas, CatalogoModulos catalogo) : CadastroServico<Plano>(repositorio, consultas, catalogo)
{
}

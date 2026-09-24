using JiuManager.Repositories;
using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
namespace JiuManager.Services;

public class UnidadeServico(IUnidadeRepositorio repositorio, ConsultaRepositorio consultas, CatalogoModulos catalogo) : CadastroServico<Unidade>(repositorio, consultas, catalogo)
{
}

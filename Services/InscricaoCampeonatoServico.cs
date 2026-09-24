using JiuManager.Repositories;
using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
namespace JiuManager.Services;

public class InscricaoCampeonatoServico(IInscricaoCampeonatoRepositorio repositorio, ConsultaRepositorio consultas, CatalogoModulos catalogo) : CadastroServico<InscricaoCampeonato>(repositorio, consultas, catalogo)
{
}

using JiuManager.Models;
using JiuManager.Services;
using JiuManager.Configuracoes;
namespace JiuManager.Controllers;

public class UnidadesController(UnidadeServico servico, TelaServico telas, CatalogoModulos catalogo, ImagemServico imagens) : CadastroController<Unidade>(servico, telas, catalogo, imagens)
{
}

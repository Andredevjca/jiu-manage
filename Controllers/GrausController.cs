using JiuManager.Models;
using JiuManager.Services;
using JiuManager.Configuracoes;
namespace JiuManager.Controllers;

public class GrausController(GrauServico servico, TelaServico telas, CatalogoModulos catalogo, ImagemServico imagens) : CadastroController<Grau>(servico, telas, catalogo, imagens)
{
}

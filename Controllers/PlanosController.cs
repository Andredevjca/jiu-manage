using JiuManager.Models;
using JiuManager.Services;
using JiuManager.Configuracoes;
namespace JiuManager.Controllers;

public class PlanosController(PlanoServico servico, TelaServico telas, CatalogoModulos catalogo, ImagemServico imagens) : CadastroController<Plano>(servico, telas, catalogo, imagens)
{
}

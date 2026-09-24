using JiuManager.Models;
using JiuManager.Services;
using JiuManager.Configuracoes;
namespace JiuManager.Controllers;

public class CampeonatosController(CampeonatoServico servico, TelaServico telas, CatalogoModulos catalogo, ImagemServico imagens) : CadastroController<Campeonato>(servico, telas, catalogo, imagens)
{
}

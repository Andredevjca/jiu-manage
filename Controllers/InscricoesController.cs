using JiuManager.Models;
using JiuManager.Services;
using JiuManager.Configuracoes;
namespace JiuManager.Controllers;

public class InscricoesController(InscricaoCampeonatoServico servico, TelaServico telas, CatalogoModulos catalogo, ImagemServico imagens) : CadastroController<InscricaoCampeonato>(servico, telas, catalogo, imagens)
{
}

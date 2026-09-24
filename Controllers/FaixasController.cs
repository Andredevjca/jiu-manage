using JiuManager.Models;
using JiuManager.Services;
using JiuManager.Configuracoes;
namespace JiuManager.Controllers;

public class FaixasController(FaixaServico servico, TelaServico telas, CatalogoModulos catalogo, ImagemServico imagens) : CadastroController<Faixa>(servico, telas, catalogo, imagens)
{
}

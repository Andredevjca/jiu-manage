using JiuManager.Models;
using JiuManager.Services;
using JiuManager.Configuracoes;
namespace JiuManager.Controllers;

public class GraduacoesController(GraduacaoServico servico, TelaServico telas, CatalogoModulos catalogo, ImagemServico imagens) : CadastroController<Graduacao>(servico, telas, catalogo, imagens)
{
}

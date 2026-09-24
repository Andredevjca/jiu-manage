using JiuManager.Models;
using JiuManager.Services;
using JiuManager.Configuracoes;
namespace JiuManager.Controllers;

public class EquipesController(EquipeServico servico, TelaServico telas, CatalogoModulos catalogo, ImagemServico imagens) : CadastroController<Equipe>(servico, telas, catalogo, imagens)
{
}

using JiuManager.Models;
using JiuManager.Services;
using JiuManager.Configuracoes;
namespace JiuManager.Controllers;

public class TurmasController(TurmaServico servico, TelaServico telas, CatalogoModulos catalogo, ImagemServico imagens) : CadastroController<Turma>(servico, telas, catalogo, imagens)
{
}

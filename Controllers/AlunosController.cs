using JiuManager.Models;
using JiuManager.Services;
using JiuManager.Configuracoes;
namespace JiuManager.Controllers;

public class AlunosController(AlunoServico servico, TelaServico telas, CatalogoModulos catalogo, ImagemServico imagens) : CadastroController<Aluno>(servico, telas, catalogo, imagens)
{
}

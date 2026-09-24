using JiuManager.Models;
using JiuManager.Services;
using JiuManager.Configuracoes;
namespace JiuManager.Controllers;

public class MatriculasController(MatriculaServico servico, TelaServico telas, CatalogoModulos catalogo, ImagemServico imagens) : CadastroController<Matricula>(servico, telas, catalogo, imagens)
{
}

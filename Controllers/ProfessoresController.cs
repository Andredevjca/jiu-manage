using JiuManager.Models;
using JiuManager.Services;
using JiuManager.Configuracoes;
namespace JiuManager.Controllers;

public class ProfessoresController(ProfessorServico servico, TelaServico telas, CatalogoModulos catalogo, ImagemServico imagens) : CadastroController<Professor>(servico, telas, catalogo, imagens)
{
}

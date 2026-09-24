using JiuManager.Models;
using JiuManager.Services;
using JiuManager.Configuracoes;
namespace JiuManager.Controllers;

public class UsuariosController(UsuarioServico servico, TelaServico telas, CatalogoModulos catalogo, ImagemServico imagens) : CadastroController<Usuario>(servico, telas, catalogo, imagens)
{
}

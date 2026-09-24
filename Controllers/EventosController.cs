using JiuManager.Models;
using JiuManager.Services;
using JiuManager.Configuracoes;
namespace JiuManager.Controllers;

public class EventosController(EventoServico servico, TelaServico telas, CatalogoModulos catalogo, ImagemServico imagens) : CadastroController<Evento>(servico, telas, catalogo, imagens)
{
}

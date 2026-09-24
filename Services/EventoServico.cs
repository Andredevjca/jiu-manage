using JiuManager.Repositories;
using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
namespace JiuManager.Services;

public class EventoServico(IEventoRepositorio repositorio, ConsultaRepositorio consultas, CatalogoModulos catalogo) : CadastroServico<Evento>(repositorio, consultas, catalogo)
{
}

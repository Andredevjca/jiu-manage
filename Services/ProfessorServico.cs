using JiuManager.Repositories;
using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
namespace JiuManager.Services;

public class ProfessorServico(IProfessorRepositorio repositorio, ConsultaRepositorio consultas, CatalogoModulos catalogo) : CadastroServico<Professor>(repositorio, consultas, catalogo)
{
}

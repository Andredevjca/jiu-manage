using JiuManager.Models;
using JiuManager.Interfaces;
using JiuManager.Configuracoes;
using JiuManager.Repositories;
namespace JiuManager.Services;

public class AlunoServico(IAlunoRepositorio repositorio, ConsultaRepositorio consultas, CatalogoModulos catalogo) : CadastroServico<Aluno>(repositorio, consultas, catalogo)
{
    public override async Task<int> SalvarAsync(Aluno aluno)
    {
        if (aluno.DataNascimento > DateTime.Today || aluno.DataInicio > DateTime.Today)
            throw new RegraNegocioException("As datas de nascimento e início não podem estar no futuro.");
        if (aluno.Id > 0)
        {
            var anterior = await ObterAsync(aluno.Id) ?? throw new RegraNegocioException("Aluno não encontrado.");
            aluno.CodigoPublico = anterior.CodigoPublico;
            aluno.FaixaId = anterior.FaixaId;
            aluno.GrauId = anterior.GrauId;
            aluno.DataUltimaGraduacao = anterior.DataUltimaGraduacao;
        }
        return await base.SalvarAsync(aluno);
    }
}

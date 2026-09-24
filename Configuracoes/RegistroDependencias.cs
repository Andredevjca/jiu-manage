using JiuManager.Interfaces;
using JiuManager.Repositories;
using JiuManager.Services;
namespace JiuManager.Configuracoes;

public static class RegistroDependencias
{
    public static void AdicionarModulos(this IServiceCollection servicos)
    {
        servicos.AddScoped<IEquipeRepositorio, EquipeRepositorio>();
        servicos.AddScoped<EquipeServico>();
        servicos.AddScoped<IFaixaRepositorio, FaixaRepositorio>();
        servicos.AddScoped<FaixaServico>();
        servicos.AddScoped<IGrauRepositorio, GrauRepositorio>();
        servicos.AddScoped<GrauServico>();
        servicos.AddScoped<IUnidadeRepositorio, UnidadeRepositorio>();
        servicos.AddScoped<UnidadeServico>();
        servicos.AddScoped<IProfessorRepositorio, ProfessorRepositorio>();
        servicos.AddScoped<ProfessorServico>();
        servicos.AddScoped<IPlanoRepositorio, PlanoRepositorio>();
        servicos.AddScoped<PlanoServico>();
        servicos.AddScoped<IAlunoRepositorio, AlunoRepositorio>();
        servicos.AddScoped<AlunoServico>();
        servicos.AddScoped<ITurmaRepositorio, TurmaRepositorio>();
        servicos.AddScoped<TurmaServico>();
        servicos.AddScoped<IMatriculaRepositorio, MatriculaRepositorio>();
        servicos.AddScoped<MatriculaServico>();
        servicos.AddScoped<IGraduacaoRepositorio, GraduacaoRepositorio>();
        servicos.AddScoped<GraduacaoServico>();
        servicos.AddScoped<IFrequenciaRepositorio, FrequenciaRepositorio>();
        servicos.AddScoped<FrequenciaServico>();
        servicos.AddScoped<ICampeonatoRepositorio, CampeonatoRepositorio>();
        servicos.AddScoped<CampeonatoServico>();
        servicos.AddScoped<IInscricaoCampeonatoRepositorio, InscricaoCampeonatoRepositorio>();
        servicos.AddScoped<InscricaoCampeonatoServico>();
        servicos.AddScoped<IEventoRepositorio, EventoRepositorio>();
        servicos.AddScoped<EventoServico>();
        servicos.AddScoped<IMensalidadeRepositorio, MensalidadeRepositorio>();
        servicos.AddScoped<MensalidadeServico>();
        servicos.AddScoped<IRecebimentoRepositorio, RecebimentoRepositorio>();
        servicos.AddScoped<RecebimentoServico>();
        servicos.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
        servicos.AddScoped<UsuarioServico>();
    }
}

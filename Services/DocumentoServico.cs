using JiuManager.Repositories;
using JiuManager.Utils;
namespace JiuManager.Services;

public class DocumentoServico(DocumentoRepositorio repositorio, IConfiguration configuracao)
{
    public async Task<Dictionary<string, object?>?> CarteirinhaAsync(int id)
    {
        var aluno = await repositorio.AlunoAsync(id);
        if (aluno != null)
        {
            var endereco = (configuracao["Publico:UrlBase"] ?? "http://localhost:5080").TrimEnd('/') + "/validar/aluno/" + aluno["CodigoPublico"];
            aluno["QrCode"] = GeradorQrCode.Gerar(endereco);
            aluno["EnderecoValidacao"] = endereco;
        }
        return aluno;
    }
    public Task<Dictionary<string, object?>?> ValidarAsync(string codigo) => repositorio.AlunoAsync(codigo: codigo); public Task<Dictionary<string, object?>?> CertificadoAsync(int id) => repositorio.CertificadoAsync(id); public Task EmitirAsync(int id) => repositorio.EmitirAsync(id);
}

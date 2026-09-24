using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JiuManager.Models;
using JiuManager.Models.ViewModels;
using JiuManager.Services;
using JiuManager.Configuracoes;
using JiuManager.Helpers;
namespace JiuManager.Controllers;

[Authorize]
public abstract class CadastroController<T>(CadastroServico<T> servico, TelaServico telas, CatalogoModulos catalogo, ImagemServico imagens) : Controller where T : Registro, new()
{
    protected readonly CadastroServico<T> Servico = servico; protected readonly TelaServico Telas = telas; protected readonly Modulo Modulo = catalogo.Obter<T>();
    public async Task<IActionResult> Index(string? busca, int pagina = 1, string? status = null) => View(await Telas.ListaAsync(Modulo.Plural, busca, pagina, status));
    public virtual async Task<IActionResult> Criar() => View(new FormularioModelo { Modulo = Modulo, Registro = new T(), Opcoes = await Telas.OpcoesAsync(Modulo) });
    public async Task<IActionResult> Editar(int id)
    {
        if (Modulo.Imutavel)
            return Forbid();
        var registro = await Servico.ObterAsync(id);
        return registro == null ? NotFound() : View(new FormularioModelo { Modulo = Modulo, Registro = registro, Opcoes = await Telas.OpcoesAsync(Modulo) });
    }
    public async Task<IActionResult> Detalhes(int id)
    {
        var registro = await Servico.ObterAsync(id);
        return registro == null ? NotFound() : View(await Telas.DetalhesAsync(registro, Modulo));
    }
    [HttpPost]
    public async Task<IActionResult> Salvar(T registro, IFormFile? imagem, string? novaSenha)
    {
        try
        {
            if (!ModelState.IsValid)
                throw new RegraNegocioException(string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => string.IsNullOrEmpty(e.ErrorMessage) ? "Verifique os campos informados." : e.ErrorMessage)));
            foreach (var campo in Modulo.Campos.Where(c => c.Entrada == "foto"))
            {
                var propriedade = typeof(T).GetProperty(campo.Nome)!;
                var anterior = registro.Id > 0 ? await Servico.ObterAsync(registro.Id) : null;
                propriedade.SetValue(registro, anterior == null ? "" : propriedade.GetValue(anterior));
                if (imagem != null)
                    propriedade.SetValue(registro, await imagens.SalvarAsync(imagem));
            }
            if (registro is Usuario usuario && Servico is UsuarioServico usuarios)
                return RedirectToAction(nameof(Detalhes), new
                {
                    id = await usuarios.SalvarComSenhaAsync(usuario, novaSenha)
                });
            var id = await Servico.SalvarAsync(registro);
            TempData["Sucesso"] = "Registro salvo com sucesso.";
            return RedirectToAction(nameof(Detalhes), new
            {
                id
            });
        }
        catch (RegraNegocioException erro) { ModelState.AddModelError("", erro.Message); }
        catch (MySqlConnector.MySqlException erro) when (erro.Number == 1062 || erro.Number == 1452) { ModelState.AddModelError("", "Já existe um registro com esses dados ou o vínculo selecionado não está disponível."); }
        return View(registro.Id == 0 ? "Criar" : "Editar", new FormularioModelo { Modulo = Modulo, Registro = registro, Opcoes = await Telas.OpcoesAsync(Modulo) });
    }
    [HttpPost]
    public async Task<IActionResult> Excluir(int id)
    {
        try
        {
            await Servico.ExcluirAsync(id);
            TempData["Sucesso"] = "Registro excluído.";
        }
        catch (RegraNegocioException erro) { TempData["Erro"] = erro.Message; }
        catch (MySqlConnector.MySqlException erro) when (erro.Number == 1451) { TempData["Erro"] = "Este registro possui vínculos. Altere o status para inativo para preservar o histórico."; }
        return RedirectToAction(nameof(Index));
    }
}

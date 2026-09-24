using System.ComponentModel.DataAnnotations;
using JiuManager.Configuracoes;
using JiuManager.Interfaces;
using JiuManager.Models;
using JiuManager.Repositories;
namespace JiuManager.Services;

public class CadastroServico<T>(IRepositorio<T> repositorio, ConsultaRepositorio consultas, CatalogoModulos catalogo) where T : Registro, new()
{
    protected readonly IRepositorio<T> Repositorio = repositorio; protected readonly ConsultaRepositorio Consultas = consultas; protected readonly Modulo Modulo = catalogo.Obter<T>();
    public Task<List<T>> ListarAsync() => Repositorio.ListarAsync(); public Task<T?> ObterAsync(int id) => Repositorio.ObterAsync(id);
    protected async Task ValidarAsync(T registro)
    {
        foreach (var propriedade in typeof(T).GetProperties().Where(p => p.PropertyType == typeof(string)))
            if (propriedade.GetValue(registro) == null)
                propriedade.SetValue(registro, "");
        var erros = new List<ValidationResult>();
        if (!Validator.TryValidateObject(registro, new ValidationContext(registro), erros, true))
            throw new RegraNegocioException(erros[0].ErrorMessage!);
        foreach (var campo in Modulo.Campos)
        {
            var valor = typeof(T).GetProperty(campo.Nome)!.GetValue(registro);
            if (campo.Referencia != null && valor is int id && !await Consultas.ExisteAsync(campo.Referencia, id))
                throw new RegraNegocioException($"Selecione um valor válido para {campo.Rotulo.ToLower()}.");
            if (campo.Opcoes != null && !campo.Opcoes.Contains(valor?.ToString()))
                throw new RegraNegocioException($"{campo.Rotulo} inválido.");
            if (campo.Nome == "Cor" && !System.Text.RegularExpressions.Regex.IsMatch(valor?.ToString() ?? "", "^#[0-9a-fA-F]{6}$"))
                throw new RegraNegocioException("Selecione uma cor válida.");
            if (campo.Nome == "Cpf" && valor is string cpf && !string.IsNullOrWhiteSpace(cpf) && !Utils.ValidacaoUtil.CpfValido(cpf))
                throw new RegraNegocioException("CPF inválido.");
        }
    }
    public virtual async Task<int> SalvarAsync(T registro)
    {
        await ValidarAsync(registro);
        if (Modulo.Imutavel && registro.Id != 0)
            throw new RegraNegocioException("O histórico não pode ser alterado.");
        if (registro.Id > 0 && await ObterAsync(registro.Id) == null)
            throw new RegraNegocioException("Registro não encontrado.");
        return await Repositorio.SalvarAsync(registro);
    }
    public virtual async Task ExcluirAsync(int id)
    {
        if (Modulo.Imutavel)
            throw new RegraNegocioException("O histórico não pode ser excluído.");
        await Repositorio.ExcluirAsync(id);
    }
}

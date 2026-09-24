using JiuManager.Models;
namespace JiuManager.Interfaces;

public interface IRepositorio<T> where T : Registro
{
    Task<List<T>> ListarAsync(); Task<T?> ObterAsync(int id); Task<int> SalvarAsync(T registro); Task ExcluirAsync(int id);
}

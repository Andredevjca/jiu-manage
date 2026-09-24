namespace JiuManager.Models;

public abstract class Registro
{
    public int Id
    {
        get; set;
    }
    public int EquipeId
    {
        get; set;
    }
    public DateTime CriadoEm
    {
        get; set;
    }
    public DateTime AtualizadoEm
    {
        get; set;
    }
}



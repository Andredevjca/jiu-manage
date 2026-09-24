namespace JiuManager.Utils;

public static class ValidacaoUtil
{
    public static bool CpfValido(string valor)
    {
        var cpf = new string(valor.Where(char.IsDigit).ToArray());
        if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
            return false;
        for (int tamanho = 9; tamanho <= 10; tamanho++)
        {
            int soma = 0;
            for (int i = 0; i < tamanho; i++)
                soma += (cpf[i] - '0') * (tamanho + 1 - i);
            int digito = (soma * 10) % 11;
            if (digito == 10)
                digito = 0;
            if (digito != cpf[tamanho] - '0')
                return false;
        }
        return true;
    }
}

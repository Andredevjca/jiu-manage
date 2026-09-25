namespace JiuManager.Helpers;

public static class BadgeFaixa
{
    public static string Estilo(string? cor)
    {
        if (!System.Text.RegularExpressions.Regex.IsMatch(cor ?? "", "^#[0-9a-fA-F]{6}$"))
            cor = "#e2e8f0";
        var rgb = Convert.ToInt32(cor![1..], 16);
        static double Linear(int componente)
        {
            var valor = componente / 255d;
            return valor <= .04045 ? valor / 12.92 : Math.Pow((valor + .055) / 1.055, 2.4);
        }
        var luminancia = .2126 * Linear((rgb >> 16) & 255) + .7152 * Linear((rgb >> 8) & 255) + .0722 * Linear(rgb & 255);
        var texto = luminancia > .179 ? "#000000" : "#ffffff";
        return $"--faixa-cor:{cor};--faixa-texto:{texto}";
    }
}

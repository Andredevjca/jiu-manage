using QRCoder;
namespace JiuManager.Utils;

public static class GeradorQrCode
{
    public static string Gerar(string conteudo)
    {
        using var gerador = new QRCodeGenerator();
        using var dados = gerador.CreateQrCode(conteudo, QRCodeGenerator.ECCLevel.Q);
        using var codigo = new PngByteQRCode(dados);
        return "data:image/png;base64," + Convert.ToBase64String(codigo.GetGraphic(8));
    }
}

using JiuManager.Models;
namespace JiuManager.Services;

public class ImagemServico(IWebHostEnvironment ambiente)
{
    public async Task<string> SalvarAsync(IFormFile arquivo)
    {
        if (arquivo.Length == 0 || arquivo.Length > 3 * 1024 * 1024)
            throw new RegraNegocioException("Envie uma imagem de até 3 MB.");
        await using var fluxo = arquivo.OpenReadStream();
        var cabecalho = new byte[12];
        if (await fluxo.ReadAsync(cabecalho) < 12)
            throw new RegraNegocioException("Imagem inválida.");
        var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
        bool valida = extensao switch
        {
            ".jpg" or ".jpeg" => cabecalho[0] == 255 && cabecalho[1] == 216 && cabecalho[2] == 255,
            ".png" => cabecalho.Take(8).SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }),
            ".webp" => System.Text.Encoding.ASCII.GetString(cabecalho, 0, 4) == "RIFF" && System.Text.Encoding.ASCII.GetString(cabecalho, 8, 4) == "WEBP",
            _ => false
        };
        if (!valida)
            throw new RegraNegocioException("Use imagens JPG, PNG ou WebP válidas.");
        var nome = Guid.NewGuid().ToString("N") + extensao;
        var pasta = Path.Combine(ambiente.WebRootPath, "uploads");
        Directory.CreateDirectory(pasta);
        await using var destino = File.Create(Path.Combine(pasta, nome));
        fluxo.Position = 0;
        await fluxo.CopyToAsync(destino);
        return "/uploads/" + nome;
    }
}

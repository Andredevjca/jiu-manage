using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace JiuManager.Helpers;

public class ProvedorDecimal : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext contexto) => contexto.Metadata.ModelType == typeof(decimal) ? new VinculadorDecimal() : null;
}
public class VinculadorDecimal : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext contexto)
    {
        var valor = contexto.ValueProvider.GetValue(contexto.ModelName);
        if (valor == ValueProviderResult.None)
            return Task.CompletedTask;
        contexto.ModelState.SetModelValue(contexto.ModelName, valor);
        var texto = valor.FirstValue?.Replace(',', '.');
        if (decimal.TryParse(texto, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var numero))
            contexto.Result = ModelBindingResult.Success(numero);
        else
            contexto.ModelState.TryAddModelError(contexto.ModelName, "Informe um valor numérico válido.");
        return Task.CompletedTask;
    }
}

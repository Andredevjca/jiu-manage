using System.ComponentModel.DataAnnotations;
namespace JiuManager.Utils;

public sealed class EmailValidoAttribute : ValidationAttribute
{
    public override bool IsValid(object? valor) => string.IsNullOrWhiteSpace(valor?.ToString()) || new EmailAddressAttribute().IsValid(valor);
}

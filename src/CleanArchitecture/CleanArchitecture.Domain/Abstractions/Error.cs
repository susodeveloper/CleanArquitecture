namespace CleanArchitecture.Domain.Abstractions;

public record Error(string Code, string Message)
{
    public static Error None = new Error(String.Empty, String.Empty);
    public static Error NullValue = new Error("Error.NullValue", "Un valor null fue ingresado");
    
}
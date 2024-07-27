using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Alquileres;

public static class AlquilerErrors
{
    public static Error NotFound = new Error("Alquiler.NotFound", "Alquiler no encontrado");
    public static Error Overlap = new Error("Alquiler.Overlap", "Alquiler se superpone con otro alquiler");
    public static Error NotReseved = new Error("Alquiler.NotReserved", "Alquiler no reservado");
    public static Error NotConfirmed = new Error("Alquiler.NotConfirmed", "Alquiler no confirmado");
    public static Error AlreadyStarted = new Error("Alquiler.AlreadyStarted", "Alquiler ya ha comenzado");

}
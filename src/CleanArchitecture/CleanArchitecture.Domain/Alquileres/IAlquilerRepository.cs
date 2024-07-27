using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Domain.Alquileres;

public interface IAlquilerRepository
{
    Task<Alquiler?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> IsOverlappingAsync(Vehiculo vehiculo, DateRange duracion, CancellationToken cancellationToken);
    void Add(Alquiler alquiler);
}
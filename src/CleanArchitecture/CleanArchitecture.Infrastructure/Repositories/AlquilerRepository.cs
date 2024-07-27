using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;
internal sealed class AlquilerRepository : Repository<Alquiler>, IAlquilerRepository
{
    private static readonly AlquilerStatus[] ActiveStatuses = [AlquilerStatus.Reservado, AlquilerStatus.Confirmado, AlquilerStatus.Completado];

    public AlquilerRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsOverlappingAsync(Vehiculo vehiculo, DateRange duracion, CancellationToken cancellationToken)
    {
        return await DbContext.Set<Alquiler>()
            .AnyAsync(a => a.VehiculoId == vehiculo.Id && 
                a.Duracion!.Inicio <= duracion.Fin &&
                a.Duracion!.Fin >= duracion.Inicio &&
                ActiveStatuses.Contains(a.AlquilerStatus)
            , cancellationToken);
    }
}
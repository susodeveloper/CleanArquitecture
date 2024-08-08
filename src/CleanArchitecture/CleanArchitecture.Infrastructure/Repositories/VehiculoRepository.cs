using System.Linq.Expressions;
using CleanArchitecture.Application.Paginations;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore.Query;

namespace CleanArchitecture.Infrastructure.Repositories;

#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
internal sealed class VehiculoRepository : Repository<Vehiculo, VehiculoId>, IVehiculoRepository, IPaginationVehiculoRepository
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
{
    public VehiculoRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }    
}
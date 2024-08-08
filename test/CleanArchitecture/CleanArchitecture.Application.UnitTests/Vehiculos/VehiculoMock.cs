using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Application.UnitTests.Vehiculos;

internal static class VehiculoMock
{
    public static Vehiculo Create() => new(
            new VehiculoId(Guid.NewGuid()),
            new Modelo("Civic"),
            new Vin("1HGBH41JXMN109186"),
            new Moneda(150.0m, TipoMoneda.Usd),
            Moneda.Zero(TipoMoneda.Usd),
            DateTime.UtcNow,
            [],
            new Direccion("USA", "Texas", "Laredo", "Limon", "Av. El Placer")
        );
}

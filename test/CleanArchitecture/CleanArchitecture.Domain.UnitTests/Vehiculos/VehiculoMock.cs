using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Domain.UnitTests.Vehiculos;
internal static class VehiculoMock
{
    public static Vehiculo Create(Moneda precio, Moneda? manteniminento = null)
    {
        return new(
            VehiculoId.New(),
            new Modelo("Civic"),
            new Vin("45dsdfds5444"),
            precio,
            manteniminento ?? Moneda.Zero(),
            DateTime.UtcNow.AddYears(-1),
            [],
            new Direccion("USA", "Texas", "Lorenz", "El Paso", "Av. El Alamo")
        );
    }
}

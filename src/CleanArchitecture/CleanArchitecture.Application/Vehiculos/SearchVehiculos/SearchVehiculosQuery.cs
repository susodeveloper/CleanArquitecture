using CleanArchitecture.Application.Abstractions.Messaging;

namespace  CleanArchitecture.Application.Vehiculos.SearchVehiculos;

public sealed record SearchVehiculosQuery(
    DateOnly fechainicio,
    DateOnly fechafin
) : IQuery<IReadOnlyList<VehiculoResponse>>;
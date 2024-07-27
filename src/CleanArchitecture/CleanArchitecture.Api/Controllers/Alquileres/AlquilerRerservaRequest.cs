namespace CleanArchitecture.Api.Controllers.Alquileres;

public sealed record AlquilerReservaRequest(
    Guid VehivuloId,
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate
);
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using Dapper;

namespace CleanArchitecture.Application.Vehiculos.SearchVehiculos;

internal sealed class SearchVehiculosQueryHandler
: IQueryHandler<SearchVehiculosQuery, IReadOnlyList<VehiculoResponse>>
{
    private static readonly int[] ActiveAlquilerStatuses = 
    {
        (int)AlquilerStatus.Reservado,
        (int)AlquilerStatus.Confirmado,
        (int)AlquilerStatus.Completado
    };
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public SearchVehiculosQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<VehiculoResponse>>> Handle(SearchVehiculosQuery request, CancellationToken cancellationToken)
    {
        if(request.fechainicio > request.fechafin) return new List<VehiculoResponse>();
        
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """ 
            SELECT 
                v.id as Id,
                v.modelo as Modelo,
                v.vin as Vin,
                v.precio_monto as Precio,
                v.precio_tipo_moneda as TipoMoneda,
                v.direccion_pais as Pais,
                v.direccion_departamento as Departamento,
                v.direccion_provincia as Provincia,
                v.direccion_ciudad as Ciudad,
                v.direccion_calle as Calle
            FROM vehiculos AS v WHERE NOT EXISTS
            (
                SELECT 1 FROM alquileres AS a
                WHERE a.vehiculo_id = v.id
                AND a.duracion_inicio <= @fechafin
                AND a.duracion_fin >= @fechainicio
                AND a.status = ANY(@ActiveAlquilerStatuses)
            )
        """;

        var vehiculos = await connection.QueryAsync<VehiculoResponse, DireccionResponse, VehiculoResponse>(
            sql,
            (vehiculo, direccion) =>
            {
                vehiculo.Direccion = direccion;
                return vehiculo;
            },
            new
            {
                request.fechainicio,
                request.fechafin,
                ActiveAlquilerStatuses
            },
            splitOn: "Pais"
        );

        return vehiculos.ToList();
    }
}
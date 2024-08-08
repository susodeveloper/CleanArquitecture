using System.Linq.Expressions;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Domain.Vehiculos;

public sealed class Vehiculo : Entity<VehiculoId>
{
    public static Expression<Func<Vehiculo, Modelo>> ValueExpression = u => u.Modelo!;

    public static Expression<Func<Vehiculo, bool>> IsActiveSpecification() => x => true;
    public static Expression<Func<Vehiculo, bool>> SearchTermSpecification(string key) => x =>
         x.Modelo!.Value!.Contains(key, StringComparison.OrdinalIgnoreCase);
    private Vehiculo() { }
    public Vehiculo(
        VehiculoId id,
        Modelo modelo,
        Vin vin,
        Moneda precio,
        Moneda mantenimiento,
        DateTime? fechaUltimoAlquiler,
        List<Accesorio> accesorios,
        Direccion? direccion
    ) : base(id)
    {
        Modelo = modelo;
        Vin = vin;
        Precio = precio;
        Mantenimiento = mantenimiento;
        FechaUltimoAlquiler = fechaUltimoAlquiler;
        Accesorios = accesorios;
        Direccion = direccion;
    }
    public Modelo? Modelo { get; private set; }
    public Vin? Vin { get; private set; }
    public Direccion? Direccion { get; private set; }
    public Moneda? Precio { get; private set; }
    public Moneda? Mantenimiento { get; private set; }
    public DateTime? FechaUltimoAlquiler { get; internal set; }
    public List<Accesorio> Accesorios { get; private set; } = new();

}
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Configurations;

internal sealed class VehiculoConfiguration : IEntityTypeConfiguration<Vehiculo>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Vehiculo> builder)
    {
        builder.ToTable("vehiculos");
        builder.HasKey(x => x.Id);

        builder.OwnsOne(v => v.Direccion);
        builder.Property(v => v.Modelo)
            .HasMaxLength(200)
            .HasConversion(m => m!.Value, value => new Modelo(value));

        builder.Property(v => v.Vin)
            .HasMaxLength(500)
            .HasConversion(v => v!.Value, value => new Vin(value));

        builder.OwnsOne(v => v.Precio, priceBuilder =>{
            priceBuilder.Property(m => m.TipoMoneda)
                .HasConversion(tm => tm.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
        });

        builder.OwnsOne(v=> v.Mantenimiento, priceBuilder=>{
            priceBuilder.Property(m => m.TipoMoneda)
                .HasConversion(tm => tm.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
        });

        builder.Property<uint>("Version").IsRowVersion();
    }
}
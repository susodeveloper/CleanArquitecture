using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;
internal class AlquilerConfiguration : IEntityTypeConfiguration<Alquiler>
{
    public void Configure(EntityTypeBuilder<Alquiler> builder)
    {
        builder.ToTable("alquileres");
        builder.HasKey(x => x.Id);

        builder.Property(alquiler => alquiler.Id)
            .HasConversion(alquilerID => alquilerID!.Value, id => new AlquilerId(id));

        builder.OwnsOne(a => a.PrecioPorPeriodo, priceBuilder =>
        {
            priceBuilder.Property(m => m.TipoMoneda)
                .HasConversion(tm => tm.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
        });

        builder.OwnsOne(a => a.Mantenimiento, priceBuilder =>
        {
            priceBuilder.Property(m => m.TipoMoneda)
                .HasConversion(tm => tm.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
        });

        builder.OwnsOne(a => a.Accesorios, priceBuilder =>
        {
            priceBuilder.Property(m => m.TipoMoneda)
                .HasConversion(tm => tm.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
        });

        builder.OwnsOne(a => a.PrecioTotal, priceBuilder =>
        {
            priceBuilder.Property(m => m.TipoMoneda)
                .HasConversion(tm => tm.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
        });

        builder.OwnsOne(a => a.Duracion);

        builder.HasOne<Vehiculo>()
            .WithMany()
            .HasForeignKey(a => a.VehiculoId);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(a => a.UserId);

    }
}

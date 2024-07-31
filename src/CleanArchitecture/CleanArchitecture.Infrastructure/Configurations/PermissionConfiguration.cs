using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Domain.Permisions;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
        .HasConversion(p => p!.Value, p => new PermissionId(p));

        builder.Property(p => p.Nombre)
        .HasConversion(p => p!.Value, p => new Nombre(p));

        IEnumerable<Permission> permissions = Enum.GetValues<PermissionEnum>()
        .Select( p => new Permission(
            new PermissionId((int)p), 
            new Nombre(p.ToString())
        ));

        builder.HasData(permissions);

    }
}
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Reviews;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;
internal class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");
        builder.HasKey(r => r.Id);

         builder.Property(reviewId => reviewId.Id)
            .HasConversion(reviewID => reviewID!.Value, id => new ReviewId(id));

        builder.Property(r => r.Rating)
            .HasConversion(r => r!.Value, value => Rating.Create(value).Value);

        builder.Property(r => r.Comentario)
            .HasMaxLength(200)
            .HasConversion(r => r!.Value, value => new Comentario(value));

        builder.HasOne<Vehiculo>()
            .WithMany()
            .HasForeignKey(r => r.VeiculoId);

        builder.HasOne<Alquiler>()
            .WithMany()
            .HasForeignKey(r => r.AlquilerId);
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(r => r.UserId);
        
    }
}

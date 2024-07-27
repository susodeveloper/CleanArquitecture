using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Reviews.Events;

namespace CleanArchitecture.Domain.Reviews;

public sealed class Review : Entity
{
    private Review(
        Guid id,
        Guid vehiculoId,
        Guid alquilerId,
        Guid userId,
        Rating rating,
        Comentario comentario,
        DateTime? fechaCreacion
    ) : base(id) 
    {
        VeiculoId = vehiculoId;
        AlquilerId = alquilerId;
        UserId = userId;
        Rating = rating;
        Comentario = comentario;
        FechaCreacion = fechaCreacion;
    }

    public Guid VeiculoId { get; private set; }
    public Guid AlquilerId { get; private set; }
    public Guid UserId { get; private set; }

    public Rating Rating { get; private set; }
    public Comentario Comentario { get; private set; }
    public DateTime? FechaCreacion { get; private set; }

    public static Result<Review> Create(
        Alquiler alquiler,
        Rating rating,
        Comentario comentario,
        DateTime? fechaCreacion
    )
    {
        if(alquiler.AlquilerStatus != AlquilerStatus.Completado)
        {
            return Result.Failure<Review>(ReviewErrors.NotElegible);
        }

        var review = new Review(
            Guid.NewGuid(),
            alquiler.VehiculoId,
            alquiler.Id,
            alquiler.UserId,
            rating,
            comentario,
            fechaCreacion
        );

        review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id));

        return review;
    }

}
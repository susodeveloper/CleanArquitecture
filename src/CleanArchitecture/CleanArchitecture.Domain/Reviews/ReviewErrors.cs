using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Reviews
{
    public static class ReviewErrors
    {
        public static readonly Error NotElegible = new("Review.NotElegible", "Esta review y calificación no es elegible para este alquiler");
    }
}
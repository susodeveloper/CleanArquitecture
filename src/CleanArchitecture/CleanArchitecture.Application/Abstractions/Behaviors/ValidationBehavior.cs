using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Application.Exceptions;
using FluentValidation;
using MediatR;

namespace CleanArchitecture.Application.Abstractions.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
: IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(!_validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationErrors = _validators
        .Select(v => v.Validate(context))
        .Where(result => result.Errors.Any())
        .SelectMany(result => result.Errors)
        .Select(validtionFailuer => 
            new ValidationError(validtionFailuer.PropertyName, validtionFailuer.ErrorMessage)).ToList();

        if(validationErrors.Any())
        {
            throw new Exceptions.ValidationException(validationErrors);
        }

        return await next();

    }
}
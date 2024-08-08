using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace CleanArchitecture.Application.Abstractions.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
where TRequest : IBaseRequest
where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var name = request.GetType().Name;
        try
        {
            _logger.LogInformation("Handling command {CommandName} ({@Command})", name, request);
            var result = await next();
            if(result.IsSuccess)
                _logger.LogInformation("Command {CommandName} handled - success", name);
            else
                using(LogContext.PushProperty("Error", result.Error))
                    _logger.LogError("Command {CommandName} handled - failure", name);
                
            _logger.LogInformation("Command {CommandName} handled - response: {@Response}", name, result);

            return result;
        }
        catch (Exception exException)
        {
            _logger.LogError(exException, "Command {CommandName} failed", name);
            throw;
        }
    }
}

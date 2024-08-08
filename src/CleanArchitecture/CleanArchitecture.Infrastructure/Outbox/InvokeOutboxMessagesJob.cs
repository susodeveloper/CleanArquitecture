using System.Data;
using CleanArchitecture.Application.Abstractions.Clock;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Domain.Abstractions;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace CleanArchitecture.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class InvokeOutboxMessagesJob : IJob
{

    private static readonly JsonSerializerSettings _jsonSerializerSettings = new ()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IPublisher _publisher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly OutboxOptions _outboxOptions;
    private readonly ILogger<InvokeOutboxMessagesJob> _logger;

    public InvokeOutboxMessagesJob(ISqlConnectionFactory sqlConnectionFactory, IPublisher publisher, IDateTimeProvider dateTimeProvider, IOptions<OutboxOptions> outboxOptions, ILogger<InvokeOutboxMessagesJob> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _publisher = publisher;
        _dateTimeProvider = dateTimeProvider;
        _outboxOptions = outboxOptions.Value;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("InvokeOutboxMessagesJob is running");
        
        using var connetion = _sqlConnectionFactory.CreateConnection();
        using var transaction = connetion.BeginTransaction();

        var sql = $@" 
            SELECT 
                id, content
            FROM outbox_messages
            WHERE processed_on_utc IS NULL
            ORDER BY ocurred_on_utc
            LIMIT {_outboxOptions.BatchSize}
            FOR UPDATE
        ";

        var records = (await connetion.QueryAsync<OutboxMessageData>(sql, transaction: transaction)).ToList();

        foreach (var record in records)
        {
            Exception? exception = null;
            try
            {
                var message = JsonConvert.DeserializeObject<IDomainEvent>(record.Content, _jsonSerializerSettings)!;
                await _publisher.Publish(message, context.CancellationToken);    
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error processing outbox message with id {id}", record.id);
                exception = ex;
            }
            
            await UpdateOutboxMessage(connetion, transaction, record, exception);
        }

        transaction.Commit();
        
        _logger.LogInformation("InvokeOutboxMessagesJob is done");
        
    }

    private async Task UpdateOutboxMessage(
        IDbConnection connetion,
        IDbTransaction transaction,
        OutboxMessageData record,
        Exception? exception)
    {
        const string sql = @"
            UPDATE outbox_messages
            SET processed_on_utc = @processedOnUtc, error = @error
            WHERE id = @id";

        await connetion.ExecuteAsync(sql, new
        {
            record.id,
            ProcessedOnUtc = _dateTimeProvider.currentTime,
            error = exception?.ToString()
        }, 
        transaction: transaction);



    }
}

public record OutboxMessageData(Guid id, string Content);
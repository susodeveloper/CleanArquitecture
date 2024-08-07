using Microsoft.Extensions.Options;
using Quartz;

namespace CleanArchitecture.Infrastructure.Outbox;

public class ProcessOutboxMessageSetup : IConfigureOptions<QuartzOptions>
{
    private readonly OutboxOptions _outboxOptions;

    public ProcessOutboxMessageSetup(IOptions<OutboxOptions> outboxOptions)
    {
        _outboxOptions = outboxOptions.Value;
    }

    public void Configure(QuartzOptions options)
    {
        const string jobName = nameof(InvokeOutboxMessagesJob);
        options
        .AddJob<InvokeOutboxMessagesJob>(job => job.WithIdentity(jobName))
        .AddTrigger(t => t.ForJob(jobName)
                .WithSimpleSchedule(s => s.WithIntervalInSeconds(_outboxOptions.IntervalInSeconds).RepeatForever()));

    }
}
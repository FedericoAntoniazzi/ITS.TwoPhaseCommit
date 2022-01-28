using ITS.TwoPhaseCommit.Contracts;
using MassTransit;

namespace ITS.TwoPhaseCommit.OrderService.Consumers;

public class CommitConsumer : IConsumer<Commit>
{
    private readonly ILogger<CommitConsumer> _logger;

    public CommitConsumer(ILogger<CommitConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<Commit> context)
    {
        // All conditions are ok
        // TODO: Commit the transaction

        // Transaction has been committed successfully
        await context.Publish(
            new Ack(
                context.Message.TransactionId,
                ParticipantIdentity.OrderService
            )
        );
        _logger.LogInformation(
            "{}: Committed successfully (Ack)",
            context.Message.TransactionId
        );
    }
}
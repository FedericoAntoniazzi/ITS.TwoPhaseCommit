using ITS.TwoPhaseCommit.Contracts;
using MassTransit;

namespace ITS.TwoPhaseCommit.OrderService.Consumers;

public class ProposeOrderCreateConsumer : IConsumer<ProposeOrderCreate>
{
    private readonly ILogger<ProposeOrderCreateConsumer> _logger;

    public ProposeOrderCreateConsumer(ILogger<ProposeOrderCreateConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProposeOrderCreate> context)
    {
        // All resources are locked
        // Transaction can be marked as ready
        await context.Publish(
            new ProposeResponse(
                context.Message.TransactionId,
                ParticipantIdentity.OrderService,
                true // Can commit
            )
        );
        _logger.LogInformation(
            "{}: Ready to commit",
            context.Message.TransactionId
        );
    }
}
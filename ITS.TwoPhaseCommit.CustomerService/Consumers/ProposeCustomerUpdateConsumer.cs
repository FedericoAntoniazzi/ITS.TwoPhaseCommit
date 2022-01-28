using ITS.TwoPhaseCommit.Contracts;
using ITS.TwoPhaseCommit.Coordinator.Consumers;
using MassTransit;

namespace ITS.TwoPhaseCommit.CustomerService.Consumers;

public class ProposeCustomerUpdateConsumer : IConsumer<ProposeCustomerUpdate>
{
    private readonly ILogger<ProposeCustomerUpdateConsumer> _logger;

    public ProposeCustomerUpdateConsumer(
        ILogger<ProposeCustomerUpdateConsumer> logger
    )
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProposeCustomerUpdate> context)
    {
        // All resources are locked
        // Transaction can be marked as ready
        await context.Publish(
            new ProposeResponse(
                context.Message.TransactionId,
                ParticipantIdentity.CustomerService,
                true // Can commit
            )
        );
        _logger.LogInformation(
            "{}: Ready to commit",
            context.Message.TransactionId
        );
    }
}
using ITS.TwoPhaseCommit.Contracts;
using ITS.TwoPhaseCommit.Coordinator.Models;
using ITS.TwoPhaseCommit.Coordinator.Services;
using ITS.TwoPhaseCommit.Models;
using MassTransit;

namespace ITS.TwoPhaseCommit.Coordinator.Consumers;

public class ProposeResponseConsumer : IConsumer<ProposeResponse>
{
    private readonly ITransactionManager<CustomerOrder> _transactionManager;
    private readonly ILogger<ProposeResponseConsumer> _logger;

    public ProposeResponseConsumer(
        ITransactionManager<CustomerOrder> transactionManager,
        ILogger<ProposeResponseConsumer> logger
    )
    {
        _transactionManager = transactionManager;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProposeResponse> context)
    {
        // The service has locked all the resources and it's ready to commit
        if (context.Message.CanCommit)
        {
            _transactionManager.SetState(
                context.Message.TransactionId,
                new TransactionState(
                    context.Message.Participant,
                    ParticipantState.Ready
                )
            );
            _logger.LogInformation(
                "{}: {} is ready to commit",
                context.Message.TransactionId,
                context.Message.Participant.ToString()
            );

            await context.Publish(
                new Commit(
                    context.Message.TransactionId
                )
            );
        }
        // Otherwise abort the transaction
        else
        {
            _transactionManager.SetState(
                context.Message.TransactionId,
                new TransactionState(
                    context.Message.Participant,
                    ParticipantState.Abort
                )
            );
            _logger.LogInformation(
                "{}: {} ABORT",
                context.Message.TransactionId,
                context.Message.Participant.ToString()
            );

            await context.Publish(
                new Abort(
                    context.Message.TransactionId
                )
            );
        }
    }
}
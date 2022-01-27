using ITS.TwoPhaseCommit.Contracts;
using ITS.TwoPhaseCommit.Coordinator.Models;
using ITS.TwoPhaseCommit.Coordinator.Services;
using ITS.TwoPhaseCommit.Models;
using MassTransit;

namespace ITS.TwoPhaseCommit.Coordinator.Consumers;

public class AckConsumer : IConsumer<Ack>
{
    private readonly ITransactionManager<CustomerOrder> _transactionManager;
    private readonly ILogger<AckConsumer> _logger;

    public AckConsumer(
        ITransactionManager<CustomerOrder> transactionManager,
        ILogger<AckConsumer> logger
    )
    {
        _transactionManager = transactionManager;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<Ack> context)
    {
        _transactionManager.SetState(
            context.Message.TransactionId,
            new TransactionState(
                context.Message.Participant,
                ParticipantState.Ack
            )
        );
        _logger.LogInformation(
            "{}: Received ack from {}",
            context.Message.TransactionId,
            context.Message.Participant.ToString()
        );
    }
}
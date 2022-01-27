using ITS.TwoPhaseCommit.Contracts;
using ITS.TwoPhaseCommit.Coordinator.Models;
using ITS.TwoPhaseCommit.Models;
using MassTransit;

namespace ITS.TwoPhaseCommit.Coordinator.Services;

public class CustomerOrderTransactionManager : ITransactionManager<CustomerOrder>
{
    private readonly ILogger<CustomerOrderTransactionManager> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Dictionary<Guid, Transaction> _transactionsMap;

    public CustomerOrderTransactionManager(
        ILogger<CustomerOrderTransactionManager> logger,
        IServiceScopeFactory scopeFactory
    )
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _transactionsMap = new Dictionary<Guid, Transaction>();
    }

    public async Task BeginTransaction(CustomerOrder customerOrder)
    {
        // https://stackoverflow.com/questions/51618406/
        using var scope = _scopeFactory.CreateScope();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        // Define new transaction
        var uuid = Guid.NewGuid();
        _logger.LogInformation("{}: Beginning transaction", uuid);

        // Services involved in this transaction
        var participants = new List<ParticipantIdentity>
        {
            ParticipantIdentity.CustomerService,
            ParticipantIdentity.OrderService
        };

        // Register this transaction
        Register(uuid,
            new Transaction(
                uuid,
                participants,
                new List<TransactionState>()
            )
        );
        _logger.LogInformation("{}: Registered transaction", uuid);

        // Propose to update a customer
        // Can he pay?
        await publishEndpoint.Publish(
            new ProposeCustomerUpdate(
                uuid,
                customerOrder.CustomerId,
                customerOrder.Price
            )
        );
        SetState(uuid,
            new TransactionState(
                ParticipantIdentity.CustomerService,
                ParticipantState.Propose
            )
        );
        _logger.LogInformation("{}: Proposed to customer service", uuid);

        // Propose to create a new order
        // Do we have enough items in the warehouse?
        await publishEndpoint.Publish(
            new ProposeOrderCreate(
                uuid,
                customerOrder.Products
            )
        );
        SetState(uuid,
            new TransactionState(
                ParticipantIdentity.OrderService,
                ParticipantState.Propose
            )
        );
        _logger.LogInformation("{}: Proposed to order service", uuid);
    }

    public void SetState(Guid transactionId, TransactionState state)
    {
        lock (_transactionsMap)
        {
            if (_transactionsMap.ContainsKey(transactionId))
            {
                _transactionsMap[transactionId].States.Add(state);
            }
        }
    }

    public void Register(Guid transactionId, Transaction transaction)
    {
        lock (_transactionsMap)
        {
            _transactionsMap.Add(
                transactionId,
                transaction
            );
        }
    }

    public void Remove(Guid transactionId)
    {
        lock (_transactionsMap)
        {
            _transactionsMap.Remove(transactionId);
        }
    }
}
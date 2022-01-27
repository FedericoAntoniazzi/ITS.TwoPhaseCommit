using ITS.TwoPhaseCommit.Coordinator.Models;

namespace ITS.TwoPhaseCommit.Coordinator.Services;

public interface ITransactionManager<T>
{
    Task BeginTransaction(T customerOrder);
    void SetState(Guid transactionId, TransactionState state);
    void Register(Guid transactionId, Transaction transaction);
    void Remove(Guid transactionId);
}
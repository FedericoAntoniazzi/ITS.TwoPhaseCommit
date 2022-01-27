namespace ITS.TwoPhaseCommit.Contracts;

public class Commit
{
    public Commit(Guid transactionId)
    {
        TransactionId = transactionId;
    }

    public Guid TransactionId { get; }
}
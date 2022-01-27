namespace ITS.TwoPhaseCommit.Contracts;

public class Abort
{
    public Abort(Guid transactionId)
    {
        TransactionId = transactionId;
    }

    public Guid TransactionId { get; }
}
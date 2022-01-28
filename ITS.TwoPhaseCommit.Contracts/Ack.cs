namespace ITS.TwoPhaseCommit.Contracts;

public class Ack
{
    public Ack(Guid transactionId, ParticipantIdentity participant)
    {
        Participant = participant;
        TransactionId = transactionId;
    }

    public Guid TransactionId { get; }
    public ParticipantIdentity Participant { get; }
}
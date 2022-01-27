namespace ITS.TwoPhaseCommit.Contracts;

public class Ack
{
    public Ack(ParticipantIdentity participant, Guid transactionId)
    {
        Participant = participant;
        TransactionId = transactionId;
    }

    public Guid TransactionId { get; }
    public ParticipantIdentity Participant { get; }
}
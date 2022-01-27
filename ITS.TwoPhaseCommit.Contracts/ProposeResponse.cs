namespace ITS.TwoPhaseCommit.Contracts;

public class ProposeResponse
{
    public ProposeResponse(Guid transactionId, ParticipantIdentity participant, bool canCommit)
    {
        TransactionId = transactionId;
        Participant = participant;
        CanCommit = canCommit;
    }

    public Guid TransactionId { get; }
    public ParticipantIdentity Participant { get; }
    public bool CanCommit { get; }
}
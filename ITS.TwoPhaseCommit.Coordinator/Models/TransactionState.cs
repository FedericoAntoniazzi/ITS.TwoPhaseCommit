using ITS.TwoPhaseCommit.Contracts;

namespace ITS.TwoPhaseCommit.Coordinator.Models;

public class TransactionState
{
    public TransactionState(ParticipantIdentity participant, ParticipantState state)
    {
        Participant = participant;
        State = state;
    }

    public ParticipantIdentity Participant { get; }
    public ParticipantState State { get; }
}
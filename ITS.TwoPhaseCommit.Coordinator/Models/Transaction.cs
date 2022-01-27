using System.Transactions;
using ITS.TwoPhaseCommit.Contracts;

namespace ITS.TwoPhaseCommit.Coordinator.Models;

public class Transaction
{
    public Transaction(Guid id, List<ParticipantIdentity> participants, List<TransactionState> states)
    {
        Id = id;
        Participants = participants;
        States = states;
    }

    public Guid Id { get; }
    public List<ParticipantIdentity> Participants { get; }
    public List<TransactionState> States { get; }
}
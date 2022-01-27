namespace ITS.TwoPhaseCommit.Coordinator.Models;

public enum ParticipantState
{
    // The service still has to elaborate the transaction proposal
    Propose,
    // The service has locked all resources and it's ready to commit
    Ready,
    // The transaction has been successfully committed
    Commit,
    // The transaction has to be aborted
    Abort,
    // Final acknowledge
    // The transaction has ended
    Ack
}
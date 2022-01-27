namespace ITS.TwoPhaseCommit.Contracts;

public class ProposeCustomerUpdate
{
    public ProposeCustomerUpdate(Guid transactionId, Guid customerId, decimal amountToPay)
    {
        TransactionId = transactionId;
        CustomerId = customerId;
        AmountToPay = amountToPay;
    }

    public Guid TransactionId { get; }
    public Guid CustomerId { get; }
    public decimal AmountToPay { get; }
}
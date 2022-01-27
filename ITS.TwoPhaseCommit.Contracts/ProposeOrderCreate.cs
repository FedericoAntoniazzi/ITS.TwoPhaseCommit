using ITS.TwoPhaseCommit.Models;

namespace ITS.TwoPhaseCommit.Contracts;

public class ProposeOrderCreate
{
    public ProposeOrderCreate(Guid transactionId, List<OrderedProduct> products)
    {
        TransactionId = transactionId;
        Products = products;
    }

    public Guid TransactionId { get; }
    public List<OrderedProduct> Products { get; }
}
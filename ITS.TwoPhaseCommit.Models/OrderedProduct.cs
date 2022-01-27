namespace ITS.TwoPhaseCommit.Models;

public class OrderedProduct
{
    public OrderedProduct(Guid id, int quantity)
    {
        Id = id;
        Quantity = quantity;
    }

    public Guid Id { get; }
    public int Quantity { get; }
}
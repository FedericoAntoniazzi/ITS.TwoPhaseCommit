namespace ITS.TwoPhaseCommit.Models;

public class CustomerOrder
{
    public CustomerOrder(Guid customerId, decimal price, List<OrderedProduct> products)
    {
        CustomerId = customerId;
        Price = price;
        Products = products;
    }

    public Guid CustomerId { get; }
    public decimal Price { get; }
    public List<OrderedProduct> Products { get; }
}
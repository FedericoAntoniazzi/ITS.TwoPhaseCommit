namespace ITS.TwoPhaseCommit.Models;

public class NewOrderProductInput
{
    public NewOrderProductInput(Guid id, decimal price, int quantity)
    {
        Id = id;
        Price = price;
        Quantity = quantity;
    }

    public Guid Id { get; }
    public decimal Price { get; }
    public int Quantity { get; }
}
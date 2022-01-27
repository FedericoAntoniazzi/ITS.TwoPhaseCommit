namespace ITS.TwoPhaseCommit.Models;

public class NewOrderInput
{
    public NewOrderInput(Guid userId, string address, List<NewOrderProductInput> products)
    {
        UserId = userId;
        Address = address;
        Products = products;
    }

    public Guid UserId { get; }
    public string Address { get; }
    public List<NewOrderProductInput> Products { get; }
}
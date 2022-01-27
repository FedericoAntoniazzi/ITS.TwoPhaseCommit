using ITS.TwoPhaseCommit.Coordinator.Services;
using ITS.TwoPhaseCommit.Models;
using Microsoft.AspNetCore.Mvc;

namespace ITS.TwoPhaseCommit.Coordinator.Controllers;

[ApiController]
[Route("orders")]
public class OrderController : Controller
{
    private readonly ILogger<OrderController> _logger;
    private readonly ITransactionManager<CustomerOrder> _transactionManager;

    public OrderController(ILogger<OrderController> logger, ITransactionManager<CustomerOrder> transactionManager)
    {
        _logger = logger;
        _transactionManager = transactionManager;
    }

    [HttpPost]
    public async Task<ActionResult> NewOrder([FromBody] NewOrderInput input)
    {
        _logger.LogInformation("Received new order");

        var customerOrder = new CustomerOrder(
            input.UserId, // CustomerId
            input.Products.Sum(p => p.Price * p.Quantity), // Total price / Amount to pay
            input.Products.Select(p => new OrderedProduct(p.Id, p.Quantity)).ToList() // Ordered Products
        );

        await _transactionManager
            .BeginTransaction(customerOrder)
            .ConfigureAwait(false);

        return Ok();
    }
}
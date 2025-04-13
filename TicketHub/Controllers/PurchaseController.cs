using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TicketHub.Models;

[ApiController]
[Route("api/[controller]")]
public class PurchaseController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public PurchaseController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TicketPurchase purchase)
    {
        // No manual if-checks needed — model binding handles validation

        string? connectionString = _configuration["AzureStorageConnectionString"];
        if (string.IsNullOrEmpty(connectionString))
            return BadRequest("Storage connection string is missing.");

        var queueClient = new QueueClient(connectionString, "tickethub");
        await queueClient.CreateIfNotExistsAsync();

        string message = JsonSerializer.Serialize(purchase);
        await queueClient.SendMessageAsync(message);

        return Ok("Ticket purchase queued.");
    }
}

using Microsoft.AspNetCore.Mvc;
using TicketHub.Models;
using Azure.Storage.Queues;
using System.Text.Json;

namespace TicketHub.Controllers
{
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
            //Simple validation
            if (purchase == null)
                return BadRequest("Purchase data is required.");

            if (string.IsNullOrWhiteSpace(purchase.Email) || !purchase.Email.Contains("@"))
                return BadRequest("A valid email is required.");

            if (string.IsNullOrWhiteSpace(purchase.Name))
                return BadRequest("Name is required.");

            if (purchase.Quantity <= 0)
                return BadRequest("At least one ticket must be purchased.");

            if (string.IsNullOrWhiteSpace(purchase.CreditCard) || purchase.CreditCard.Length < 13)
                return BadRequest("A valid credit card number is required.");

            if (string.IsNullOrWhiteSpace(purchase.Expiration))
                return BadRequest("Expiration date is required.");

            if (string.IsNullOrWhiteSpace(purchase.SecurityCode) || purchase.SecurityCode.Length < 3)
                return BadRequest("Security code is required.");

            // Get connection string from secrets
            string? connectionString = _configuration["AzureStorageConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
                return BadRequest("Storage connection string is missing.");

            //Queue logic
            string queueName = "tickethub";
            var queueClient = new QueueClient(connectionString, queueName);

            await queueClient.CreateIfNotExistsAsync();

            string message = JsonSerializer.Serialize(purchase);
            await queueClient.SendMessageAsync(message);

            return Ok("Ticket purchase queued.");
        }
    }
}

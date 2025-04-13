using System.ComponentModel.DataAnnotations;

namespace TicketHub.Models
{
    public class TicketPurchase
    {
        [Required]
        public int ConcertId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required, Phone]
        public string Phone { get; set; } = string.Empty;

        [Required, Range(1, 20, ErrorMessage = "You must purchase at least 1 ticket.")]
        public int Quantity { get; set; }

        [Required, CreditCard]
        public string CreditCard { get; set; } = string.Empty;

        [Required]
        public string Expiration { get; set; } = string.Empty;

        [Required, StringLength(4, MinimumLength = 3)]
        public string SecurityCode { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Province { get; set; } = string.Empty;

        [Required]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Order
    {
        [Required]
        public int orderId { get; set; }

        [Required]
        public int customerId { get; set; }

        [Required]
        public string productId { get; set; }

        [Required]
        public DateTime orderDate { get; set; }

        [Required]
        public int quantity { get; set; }
    }
}

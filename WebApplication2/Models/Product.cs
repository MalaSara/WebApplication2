using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Product
    {
        [Required]
        public string productId { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string category { get; set; }

        [Required]
        public decimal price { get; set; }
    }
}

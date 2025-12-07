using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Customer
    {

        [Required]
        public int customerId { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public DateTime registrationDate { get; set; }
    }
}
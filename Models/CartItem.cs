using System;
using System.ComponentModel.DataAnnotations;

namespace EasyFarmers.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        // Navigation property
        public Product Product { get; set; }
    }
}
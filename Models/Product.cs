using System;
using System.ComponentModel.DataAnnotations;

namespace EasyFarmers.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public string ImageUrl { get; set; }
        
        [Required]
        public int StockQuantity { get; set; }
    }
}
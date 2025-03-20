using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyFarmers.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        [Required]
        public DateTime OrderDate { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        
        [Required]
        public string ShippingAddress { get; set; }
        
        [Required]
        public string RecipientName { get; set; }
        
        [Required]
        [EmailAddress]
        public string RecipientEmail { get; set; }
        
        [Required]
        public string RecipientPhone { get; set; }
        
        [Required]
        public string PaymentMethod { get; set; }
        
        [Required]
        public string OrderStatus { get; set; }
        
        public string Notes { get; set; }
        
        // Navigation property
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        
        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
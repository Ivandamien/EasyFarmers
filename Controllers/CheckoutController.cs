using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyFarmers.Models;
using EasyFarmers.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EasyFarmers.Controllers
{
    [Authorize] // Ensure users are logged in to access checkout
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Checkout
        public async Task<IActionResult> Index()
        {
            // Get the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // Retrieve the cart items for the current user
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            // If cart is empty, redirect to cart page
            if (!cartItems.Any())
            {
                TempData["Message"] = "Your cart is empty. Please add items before proceeding to checkout.";
                return RedirectToAction("Index", "Cart");
            }

            return View(cartItems);
        }

        // POST: Checkout/PlaceOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(ShippingDetails shippingDetails, string paymentMethod, string mpesaNumber = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!ModelState.IsValid)
            {
                // If the model is invalid, return to checkout page
                var cartItems = await _context.CartItems
                    .Include(c => c.Product)
                    .Where(c => c.UserId == userId)
                    .ToListAsync();
                
                return View("Index", cartItems);
            }

            // Validate payment details (for MPESA only)
            if (paymentMethod == "MPESA" && string.IsNullOrEmpty(mpesaNumber))
            {
                TempData["ErrorMessage"] = "M-PESA phone number is required for M-PESA payments.";
                return RedirectToAction("Index");
            }
            
            // Begin transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // Simulate payment processing
                var paymentResult = await ProcessPaymentAsync(paymentMethod, mpesaNumber);
                
                if (!paymentResult.Success)
                {
                    TempData["ErrorMessage"] = paymentResult.Message;
                    return RedirectToAction("Index");
                }

                // Create a new order
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    ShippingAddress = $"{shippingDetails.Address}, {shippingDetails.City}, {shippingDetails.PostalCode}, {shippingDetails.Country}",
                    RecipientName = $"{shippingDetails.FirstName} {shippingDetails.LastName}",
                    RecipientEmail = shippingDetails.Email,
                    RecipientPhone = shippingDetails.PhoneNumber,
                    PaymentMethod = paymentMethod,
                    OrderStatus = "Paid", // Changed from "Pending" since we're simulating successful payment
                    Notes = shippingDetails.Notes
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Get cart items
                var cartItems = await _context.CartItems
                    .Include(c => c.Product)
                    .Where(c => c.UserId == userId)
                    .ToListAsync();

                decimal orderTotal = 0;

                // Create order items from cart items
                foreach (var item in cartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Product.Price
                    };

                    orderTotal += (item.Product.Price * item.Quantity);
                    _context.OrderItems.Add(orderItem);
                }

                // Update order with total
                order.TotalAmount = orderTotal;
                _context.Update(order);

                // Clear the cart
                _context.CartItems.RemoveRange(cartItems);
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();

                // Redirect to order confirmation
                return RedirectToAction("Confirmation", new { id = order.Id });
            }
            catch (Exception ex)
            {
                // If there's an error, roll back the transaction
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = "An error occurred while processing your order: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // Simulated payment processing
        private async Task<PaymentResult> ProcessPaymentAsync(string paymentMethod, string mpesaNumber = null)
        {
            // In a real application, this would connect to payment gateways
            // For demo purposes, we'll simulate successful payments with a small delay
            
            await Task.Delay(1000); // Simulate network delay
            
            // For demo purposes, all payments succeed
            // In a real application, we would validate payment details and handle failures
            
            var result = new PaymentResult
            {
                Success = true,
                TransactionId = $"TX-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8)}",
                Message = "Payment processed successfully"
            };
            
            // You could simulate payment failures for testing:
            // Example: M-PESA numbers starting with "000" could simulate failure
            if (paymentMethod == "MPESA" && !string.IsNullOrEmpty(mpesaNumber) && mpesaNumber.StartsWith("000"))
            {
                result.Success = false;
                result.Message = "M-PESA payment failed. Please try again with a different number.";
            }
            
            return result;
        }

        // GET: Checkout/Confirmation/5
        public async Task<IActionResult> Confirmation(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }

    // ViewModel for shipping details
    public class ShippingDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Notes { get; set; }
    }
    
    // Class to represent payment processing result
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }
    }
}
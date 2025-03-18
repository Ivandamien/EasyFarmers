using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EasyFarmers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyFarmers.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Create admin role if it doesn't exist
            string adminRoleName = "Admin";
            IdentityResult roleResult;

            var roleExist = await roleManager.RoleExistsAsync(adminRoleName);
            if (!roleExist)
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole(adminRoleName));
            }

            // Create admin user if it doesn't exist
            var adminUser = new IdentityUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                EmailConfirmed = true
            };

            var user = await userManager.FindByNameAsync(adminUser.UserName);
            if (user == null)
            {
                var createPowerUser = await userManager.CreateAsync(adminUser, "Admin123!");
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRoleName);
                }
            }
 // Seed products if none exist
            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    
                    
                    new Product
                    {
                        Name = "Soil pH Testing Kit",
                        Description = "Professional-grade soil testing kit to measure pH levels and nutrient content. Includes 50 test strips and digital reader.",
                        Price = 35.50m,
                        StockQuantity = 120,
                        ImageUrl = "https://images.pexels.com/photos/5797899/pexels-photo-5797899.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                    },
                    new Product
                    {
                        Name = "Heavy-Duty Garden Tiller",
                        Description = "Gas-powered tiller with 4-cycle engine. 16-inch tilling width and adjustable depth settings for all soil types.",
                        Price = 349.99m,
                        StockQuantity = 25,
                        ImageUrl = "https://images.pexels.com/photos/2252089/pexels-photo-2252089.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                    },
                    new Product
                    {
                        Name = "Organic Plant Food Concentrate",
                        Description = "OMRI-certified liquid fertilizer for vegetables and flowers. 32oz bottle makes 64 gallons of plant food.",
                        Price = 19.95m,
                        StockQuantity = 200,
                        ImageUrl = "https://images.pexels.com/photos/6231733/pexels-photo-6231733.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                    },
                    new Product
                    {
                        Name = "Premium Grafting Kit",
                        Description = "Complete set for fruit tree grafting including grafting knife, pruner, wax, and tapes. Includes illustrated guide.",
                        Price = 45.75m,
                        StockQuantity = 60,
                        ImageUrl = "https://images.pexels.com/photos/2132250/pexels-photo-2132250.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                    },
                    new Product
                    {
                        Name = "Greenhouse Starter Kit",
                        Description = "10'x12' walk-in greenhouse with heavy duty steel frame, transparent PE cover, and ground stakes. Easy assembly.",
                        Price = 179.99m,
                        StockQuantity = 35,
                        ImageUrl = "https://images.pexels.com/photos/927414/pexels-photo-927414.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                    },
                    new Product
                    {
                        Name = "Heirloom Tomato Collection",
                        Description = "Set of 8 rare heirloom tomato varieties with diverse colors and flavors. Non-GMO, sustainably grown seeds.",
                        Price = 18.50m,
                        StockQuantity = 100,
                        ImageUrl = "https://images.pexels.com/photos/533280/pexels-photo-533280.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                    },
                    new Product
                    {
                        Name = "Ergonomic Garden Tool Set",
                        Description = "5-piece set of high-quality stainless steel tools with soft-grip handles. Includes trowel, cultivator, weeder, and pruners.",
                        Price = 49.99m,
                        StockQuantity = 85,
                        ImageUrl = "https://images.pexels.com/photos/1301856/pexels-photo-1301856.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                    },
                    new Product
                    {
                        Name = "Compost Tumbler",
                        Description = "Dual-chamber compost tumbler with 80-gallon capacity. Creates rich compost in weeks instead of months.",
                        Price = 129.95m,
                        StockQuantity = 40,
                        ImageUrl = "https://images.pexels.com/photos/5016966/pexels-photo-5016966.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                    },
                    new Product
                    {
                        Name = "Weather Station for Farmers",
                        Description = "Digital weather monitoring system with solar power. Tracks rainfall, temperature, humidity, and wind patterns.",
                        Price = 159.99m,
                        StockQuantity = 50,
                        ImageUrl = "https://images.pexels.com/photos/1118873/pexels-photo-1118873.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                    },
                    new Product
                    {
                        Name = "Natural Pest Control Kit",
                        Description = "Chemical-free pest management system with beneficial insects, traps, and organic repellents for 1/2 acre coverage.",
                        Price = 67.50m,
                        StockQuantity = 70,
                        ImageUrl = "https://images.pexels.com/photos/7728082/pexels-photo-7728082.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                    },
                    new Product
                    {
                        Name = "High-Yield Berry Bushes Set",
                        Description = "Collection of 4 berry plants: blueberry, raspberry, blackberry, and strawberry. Disease-resistant varieties.",
                        Price = 59.95m,
                        StockQuantity = 55,
                        ImageUrl = "https://images.pexels.com/photos/892808/pexels-photo-892808.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                    },
                    new Product
                    {
                        Name = "Rainwater Collection System",
                        Description = "Complete 60-gallon rainwater harvesting system with filters, diverter, and spigot. FDA-approved food-grade materials.",
                        Price = 149.99m,
                        StockQuantity = 30,
                        ImageUrl = "https://images.pexels.com/photos/3912408/pexels-photo-3912408.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                    },
                    new Product
                    {
                        Name = "Premium Garden Gloves Set",
                        Description = "Pack of 3 pairs of gardening gloves for different tasks. Water-resistant, puncture-proof, and breathable materials.",
                        Price = 29.95m,
                        StockQuantity = 150,
                        ImageUrl = "https://images.pexels.com/photos/6157208/pexels-photo-6157208.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                    }
                };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
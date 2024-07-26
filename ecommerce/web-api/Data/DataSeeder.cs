using ecommapp.Data;
using ecommapp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ecommapp.Data
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;

        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            if (!_context.Products.Any() && !_context.Suppliers.Any() && !_context.Orders.Any() && !_context.OrderItems.Any())
            {
                // Seed initial data for Suppliers and Product Categories first
                var supplier1 = new Supplier { Name = "Supplier 1" };
                var supplier2 = new Supplier { Name = "Supplier 2" };
                _context.Suppliers.AddRange(supplier1, supplier2);

                var category1 = new ProductCategory { Name = "Category 1" };
                var category2 = new ProductCategory { Name = "Category 2" };
                _context.ProductCategories.AddRange(category1, category2);

                await _context.SaveChangesAsync(); // Ensure IDs are generated

                // Seed Products
                var product1 = new Product { Name = "Product 1", ProductCategoryId = category1.Id, SupplierId = supplier1.Id, Description = "Description 1", Price = 20 };
                var product2 = new Product { Name = "Product 2", ProductCategoryId = category2.Id, SupplierId = supplier2.Id, Description = "Description 2", Price = 10000000 };
                _context.Products.AddRange(product1, product2);

                await _context.SaveChangesAsync(); // Ensure IDs are generated

                // Seed Customers
                var customer1 = new Customer { Name = "Customer 1", Address = "Address 1", PhoneNumber = "1234567890" };
                _context.Customers.Add(customer1);

                await _context.SaveChangesAsync(); // Ensure IDs are generated

                // Seed Orders
                var order1 = new Order { Customer = customer1, OrderDate = DateTime.UtcNow };
                _context.Orders.Add(order1);

                await _context.SaveChangesAsync(); // Ensure IDs are generated

                // Seed OrderItems
                var orderItem1 = new OrderItem { OrderId = order1.Id, ProductId = product1.Id, Quantity = 2 };
                _context.OrderItems.Add(orderItem1);

                await _context.SaveChangesAsync();
            }
        }
    }
}





// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using ecommapp.Models;

// namespace ecommapp.Data
// {
//     public class DataSeeder
//     {
//         private readonly AppDbContext _context;

//         public DataSeeder(AppDbContext context)
//         {
//             _context = context;
//         }

//         public async Task SeedDataAsync()
//         {
//             if (!_context.Customers.Any())
//             {
//                 var customers = GetPreconfiguredCustomers();
//                 _context.Customers.AddRange(customers);

//                 var orders = GetPreconfiguredOrders();
//                 _context.Orders.AddRange(orders);

//                 var orderItems = GetPreconfiguredOrderItems();
//                 _context.OrderItems.AddRange(orderItems);

//                 var products = GetPreconfiguredProducts();
//                 _context.Products.AddRange(products);

//                 await _context.SaveChangesAsync();
//             }
//         }

//         private List<Customer> GetPreconfiguredCustomers()
//         {
//             return new List<Customer>
//             {
//                 new Customer { Id = 1, Name = "John Doe", Address = "123 Main St", PhoneNumber = "555-1234" },
//                 new Customer { Id = 2, Name = "Tristan Fry", Address = "18423 raven shore", PhoneNumber = "281-682" },
//             };
//         }

//         private List<Order> GetPreconfiguredOrders()
//         {
//             return new List<Order>
//             {
//                 new Order { Id = 1, OrderDate = DateTime.Now, CustomerId = 1 },
//                 new Order { Id = 2, OrderDate = DateTime.Now, CustomerId = 2 },
//             };
//         }

//         private List<OrderItem> GetPreconfiguredOrderItems()
//         {
//             return new List<OrderItem>
//             {
//                 new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 2 },
//                 new OrderItem { Id = 2, OrderId = 2, ProductId = 2, Quantity = 2 },

//             };
//         }
//         private List<Product> GetPreconfiguredProducts()
//         {
//             return new List<Product>
//             {
//                 new Product { Id = 1, Name = "Product 1", ProductCategoryId = 1, SupplierId = 1, Description = "TESTING" },
//                 new Product { Id = 2, Name = "Product 2", ProductCategoryId = 2, SupplierId = 2, Description = "TESTING 2" },
//             };
//         }
//     }
// }

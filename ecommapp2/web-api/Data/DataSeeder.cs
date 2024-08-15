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


                // Seed OrderItems
                var orderItem1 = new OrderItem { ProductId = product1.Id, Quantity = 2 };
                _context.OrderItems.Add(orderItem1);

                // await _context.SaveChangesAsync(); // Ensure IDs are generated

                // Seed Orders
                var order1 = new Order { Customer = customer1, OrderDate = DateTime.UtcNow, OrderItems = new List<OrderItem> { orderItem1 } };
                _context.Orders.Add(order1);


                await _context.SaveChangesAsync();
            }
        }
    }
}
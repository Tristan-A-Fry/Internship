using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecommapp.Models;
using Microsoft.Identity.Client;

namespace ecommapp.Services
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public ReceiptRepository(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task<string> GenerateReceiptAsync(Order order)
        {
            var orderItems = await _orderItemRepository.GetOrderItemsAsync(order.Id, 0, int.MaxValue);

            var receipt = new StringBuilder();
            receipt.AppendLine($"Receipt for Order #{order.Id}");
            receipt.AppendLine($"Customer: {order.Customer.Name}");
            receipt.AppendLine($"Order Date: {order.OrderDate}");
            receipt.AppendLine("Items:");

            decimal total = 0;
            foreach (var item in orderItems)
            {
                decimal itemTotal = item.Quantity * item.UnitPrice;
                receipt.AppendLine($"Product: {item.ProductName}, Supplier: {item.SupplierName}, Quantity: {item.Quantity}, Unit Price: {item.UnitPrice:C}, Item Total: {itemTotal:C}");
                total += itemTotal;
            }

            receipt.AppendLine($"Total: {total:C}");
            return receipt.ToString();
        }
        // public async Task<string> GenerateReceiptAsync(Order order)
        // {
        //     var orderItems = await _orderItemRepository.GetOrderItemsAsync(order.Id, 0, int.MaxValue);

        //     var receipt = new StringBuilder();
        //     receipt.AppendLine($"Receipt for Order #{order.Id}");
        //     receipt.AppendLine($"Customer: {order.Customer.Name}");
        //     receipt.AppendLine($"Order Date: {order.OrderDate}");
        //     receipt.AppendLine("Items:");

        //     decimal total = 0;
        //     foreach (var item in orderItems)
        //     {
        //         var product = item.Product;
        //         if (product != null)
        //         {
        //             var supplier = product.Supplier;
        //             decimal itemTotal = item.Quantity * product.Price;
        //             receipt.AppendLine($"Product ID: {product.Id}, Product Name: {product.Name}, Supplier Id: {supplier.Name} Quantity: {item.Quantity}, Unit Price: {product.Price:C}, Item Total: {itemTotal:C}");
        //             total += itemTotal;
        //         }
        //         else
        //         {
        //             receipt.AppendLine($"Product details for item ID {item.Id} not found.");
        //         }
        //     }

        //     receipt.AppendLine($"Total: {total:C}");
        //     return receipt.ToString();
        // }
    }
}




// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using ecommapp.Data;
// using ecommapp.Models;
// using Microsoft.EntityFrameworkCore;

// namespace ecommapp.Services
// {
//     public class ReceiptRepository : IReceiptRepository
//     {
//         private readonly AppDbContext _context;

//         public ReceiptRepository(AppDbContext context)
//         {
//             _context = context;
//         }

//             public async Task<string> GenerateReceiptAsync(Order order)
//             {
//                 var orderItems = await _context.OrderItems
//                                                 .Where(item => item.OrderId == order.Id)
//                                                 .Include(item => item.Product)
//                                                 .ToListAsync();

//                 var receipt = new StringBuilder();
//                 receipt.AppendLine($"Receipt for Order #{order.Id}");
//                 receipt.AppendLine($"Customer: {order.Customer.Name}");
//                 receipt.AppendLine($"Order Date: {order.OrderDate}");
//                 receipt.AppendLine("Items:");

//                 decimal total = 0;
//                 foreach (var item in orderItems)
//                 {
//                     var product = item.Product;
//                     receipt.AppendLine($"Product info: {product.Id}, {product.Description}");
//                     receipt.AppendLine($"Price: {product.Price}");
//                     decimal itemTotal = item.Quantity * product.Price;
//                     receipt.AppendLine($"{product.Name} - {item.Quantity} x {product.Price:C} = {itemTotal:C}");
//                     total += itemTotal;
//                 }

//                 receipt.AppendLine($"Total: {total:C}");
//                 return receipt.ToString();
//             }
//        }
//     }
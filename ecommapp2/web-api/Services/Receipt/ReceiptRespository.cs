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

        public async Task<string> GenerateReceiptAsync(IEnumerable<Order> orders, string folderPath)
        {
            var receipt = new StringBuilder();
            decimal grandTotal = 0;

            foreach (var order in orders)
            {
                var orderItems = await _orderItemRepository.GetOrderItemsAsync(order.Id, 0, int.MaxValue);

                receipt.AppendLine($"Receipt for Order #{order.Id}");
                receipt.AppendLine($"Customer: {order.Customer.Name}");
                receipt.AppendLine($"Order Date: {order.OrderDate}");
                receipt.AppendLine("Items:");

                decimal orderTotal = 0;
                foreach (var item in orderItems)
                {
                    decimal itemTotal = item.Quantity * item.UnitPrice;
                    receipt.AppendLine($"Product: {item.ProductName}, Supplier: {item.SupplierName}, Quantity: {item.Quantity}, Unit Price: {item.UnitPrice:C}, Item Total: {itemTotal:C}");
                    orderTotal += itemTotal;
                }

                receipt.AppendLine($"Order Total: {orderTotal:C}");
                receipt.AppendLine(); // Blank line between orders
                grandTotal += orderTotal;
            }

            receipt.AppendLine($"Grand Total: {grandTotal:C}");

            var receiptContent = receipt.ToString();
            var fileName = $"Receipt_Customer_{orders.First().Customer.Id}.txt";
            var filePath = Path.Combine(folderPath, fileName);

            await File.WriteAllTextAsync(filePath, receiptContent);

            return filePath;
        }
    }
}

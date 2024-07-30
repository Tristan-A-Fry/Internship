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
    }
}

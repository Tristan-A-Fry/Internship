using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommapp.Models;
using ecommapp.Services;
using System.Text;
using ecommapp.Data;
using ecommapp.DTO;

namespace ecommapp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly AppDbContext _context;

        public OrderController(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IReceiptRepository receiptRespository, AppDbContext context)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _receiptRepository = receiptRespository;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders([FromQuery] int skip = 0, int take = 10, [FromQuery] int? customerId = null)
        {
            var orders = await _orderRepository.GetOrdersAsync(skip, take, customerId);

            var savedOrder = await _orderRepository.GetOrderWithCustomerAsync(customerId.Value);
            var receipt = await _receiptRepository.GenerateReceiptAsync(savedOrder);
            var bytes = Encoding.UTF8.GetBytes(receipt);
            var fileName = $"Receipt_Order_{savedOrder.Id}.txt";

            var response = new OrderResponseDto
            {
                Order = savedOrder,
                ReceiptFile = bytes,
                ReceiptFileName = fileName
            };
            // return Ok(response); //Switch to this once front end set up for a button to download file
            return new MultipartActionResult(response, bytes, fileName);

        }

        [HttpPut]
        public async Task<ActionResult<Order>> UpsertOrder([FromBody] putOrderDto orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest("Order cannot be null.");
            }

            // Map OrderDto to Order entity
            var order = new Order
            {
                Id = orderDto.Id,
                OrderDate = orderDto.OrderDate,
                Customer = await _context.Customers.FindAsync(orderDto.CustomerId),
                OrderItems = orderDto.OrderItems.Select(oi => new OrderItem
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity
                }).ToList()
            };

            if (order.Customer == null)
            {
                return BadRequest("Customer not found.");
            }

            foreach (var orderItem in order.OrderItems)
            {
                var product = await _context.Products.FindAsync(orderItem.ProductId);
                if (product == null)
                {
                    return BadRequest($"Product with ID {orderItem.ProductId} not found.");
                }
                orderItem.Product = product; //Link OrderItem to the Product
                orderItem.Order = order; // Link the OrderItem to the Order
            }
            var upsertedOrder = await _orderRepository.UpsertOrderAsync(order);
            var savedOrder = await _orderRepository.GetOrderWithCustomerAsync(upsertedOrder.Id);
            var receipt = await _receiptRepository.GenerateReceiptAsync(upsertedOrder);
            var bytes = Encoding.UTF8.GetBytes(receipt);
            var fileName = $"Receipt_Order_{savedOrder.Id}.txt";

            return File(bytes, "text/plain", fileName);


        }

        [HttpGet("{id}/items")]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderItems(int id, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var items = await _orderItemRepository.GetOrderItemsAsync(id, skip, take);
            return Ok(items);
        }

        [HttpPut("{id}/items")]
        public async Task<ActionResult<OrderItem>> UpsertOrderItems(int id, [FromBody] OrderItem orderItem)
        {
            if (orderItem == null)
            {
                return BadRequest("OrderItem cannot be null.");
            }

            orderItem.OrderId = id; // Ensure the OrderId is correctly set
            var upsertedOrderItem = await _orderItemRepository.UpsertOrderItemsAsync(orderItem);
            return Ok(upsertedOrderItem);
        }
    }
}












// public async Task<IActionResult> UpsertOrders([FromBody] OrderDto orderDto)
// {
//     if (!ModelState.IsValid)
//     {
//         return BadRequest(ModelState);
//     }

//     if (orderDto == null)
//     {
//         return BadRequest("Order cannot be null");
//     }

//     // Map OrderDto to Order entity
//     var order = new Order
//     {
//         Id = orderDto.Id,
//         OrderDate = orderDto.OrderDate,
//         CustomerId = orderDto.CustomerId,
//         OrderItems = orderDto.OrderItems.Select(oi => new OrderItemDto
//         {
//             Id = oi.Id,
//             ProductId = oi.ProductId,
//             Quantity = oi.Quantity,
//             OrderId = orderDto.Id
//         }).ToList()
//     };

//     var upsertedOrder = await _orderRepository.UpsertOrderAsync(order);

//     // Fetch the saved order including the customer details
//     var savedOrder = await _orderRepository.GetOrderWithCustomerAsync(upsertedOrder.Id);

//     // Generate the receipt
//     var receipt = await _receiptRepository.GenerateReceiptAsync(savedOrder);

//     // Create a byte array from the receipt string
//     var bytes = Encoding.UTF8.GetBytes(receipt);
//     var fileName = $"Receipt_Order_{savedOrder.Id}.txt";

//     // Return the receipt as a downloadable text file
//     return File(bytes, "text/plain", fileName);
// }
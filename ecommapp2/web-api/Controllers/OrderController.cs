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
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrders([FromQuery] int skip = 0, int take = 10, [FromQuery] int? customerId = null)
        {
            if (customerId == null)
            {
                return BadRequest("CustomerId is required");
            }

            var ordersDto = await _orderRepository.GetOrdersAsync(skip, take, customerId);

            if (ordersDto == null || !ordersDto.Any())
            {
                return NotFound("No orders found for the specified customer");
            }

            var orders = ordersDto.Select(orderDto => new Order
            {
                Id = orderDto.Id,
                Customer = new Customer
                {
                    Id = orderDto.Customer.Id,
                    Name = orderDto.CustomerName
                },
                OrderDate = orderDto.OrderDate,
            }).ToList();

            var receiptFolderPath = "C:\\Receipts"; 

            if (!Directory.Exists(receiptFolderPath))
            {
                Directory.CreateDirectory(receiptFolderPath);
            }

            var filePath = await _receiptRepository.GenerateReceiptAsync(orders, receiptFolderPath);

            var orderResponseList = ordersDto.Select(async orderDto => new OrderResponseDto
            {
                Order = orderDto,
                ReceiptFile = await System.IO.File.ReadAllBytesAsync(filePath),
                ReceiptFileName = Path.GetFileName(filePath)
            }).ToList();

            return Ok(orderResponseList);
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

            return Ok(savedOrder);


        }

        [HttpGet("{id}/items")]
        public async Task<ActionResult<IEnumerable<GetOrderItemDto>>> GetOrderItems(int id, [FromQuery] int skip = 0, [FromQuery] int take = 10)
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

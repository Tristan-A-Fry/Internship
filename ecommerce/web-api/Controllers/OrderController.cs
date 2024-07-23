using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommapp.Models;
using ecommapp.Services;

namespace ecommapp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderRepository _orderRepository;

        public OrderController(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders([FromQuery] int skip = 0, int take = 10, [FromQuery] int? customerId = null)
        {
            var orders = await _orderRepository.GetOrdersWithCustomerNamesAsync(skip, take, customerId);
            return Ok(orders);
        }

        [HttpPut]
        public async Task<ActionResult<Order>> UpsertOrders([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order cannot be null");
            }

            var upsertedOrder = await _orderRepository.UpsertOrderAsync(order);

            // Fetch the saved order including the customer details
            var savedOrder = await _orderRepository.GetOrderWithCustomerAsync(upsertedOrder.Id);

            return Ok(savedOrder);
        }

        [HttpGet("{id}/items")]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems(int id, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var items = await _orderRepository.GetOrderItemsWithDetailsAsync(id, skip, take);
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
            var upsertedOrderItem = await _orderRepository.UpsertOrderItemAsync(orderItem);
            return Ok(upsertedOrderItem);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using exercise1prob2.Models;
using exercise1prob2.Services;
using System.Drawing;

namespace exercise1prob2.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
       private readonly IRepository<Order> _orderRepository;
       private readonly IRepository<OrderItem> _orderItemRepository;

       public OrderController(IRepository<Order> orderRepository, IRepository<OrderItem> orderItemRepository)
       {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
       }

       [HttpGet] 
       public async Task<ActionResult<IEnumerable<Order>>> GetOrders([FromQuery]int skip = 0 , int take = 10, [FromQuery] int? customerId = null)
       {
            //return specific
            if(customerId.HasValue)
            {
                var orders = await _orderRepository.GetAllAsync(skip, take);
                return Ok(orders.Where(o => o.CustomerId == customerId.Value));
            }
            //return all orders
            else
            {
               var orders = await _orderItemRepository.GetAllAsync(skip,take);
               return Ok(orders);
            }
       }

       [HttpPut]
       public async Task<ActionResult<Order>> UpsertOrders([FromBody] Order orders)
       {
            var upsertedOrders = await _orderRepository.UpsertAsync(orders);
            return Ok(upsertedOrders);
       }

       [HttpGet("{id}/items")]
       public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems(int id, [FromQuery] int skip, [FromQuery] int take)
       {
            var items = await _orderItemRepository.GetAllAsync(skip,take);
            return Ok(items.Where(i => i.OrderId == id));
       }

       [HttpPut("{id}/items")]
       public async Task<ActionResult<OrderItem>> UpsertOrderItems(int id, [FromBody] OrderItem orderItem)
       {
            var upsertedOrderItem = await _orderItemRepository.UpsertAsync(orderItem);
            return Ok(upsertedOrderItem);
       }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ecommapp.Data;
using ecommapp.Models;

namespace ecommapp.Services
{
    public class OrderRepository : Repository<Order>
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersWithCustomerNamesAsync(int skip, int take, int? customerId = null)
        {
            var query = _context.Orders.AsQueryable();

            if (customerId.HasValue)
            {
                query = query.Where(o => o.CustomerId == customerId.Value);
            }

            var orders = await query
                .Skip(skip)
                .Take(take)
                .Include(o => o.Customer)
                .ToListAsync();

            return orders;
        }

        public async Task<Order> GetOrderWithCustomerAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsWithDetailsAsync(int orderId, int skip, int take)
        {
            return await _context.OrderItems
                .Where(i => i.OrderId == orderId)
                .Skip(skip)
                .Take(take)
                .Include(i => i.Product)
                .Include(i => i.Order)
                .ThenInclude(o => o.Customer)
                .ToListAsync();
        }

        public async Task<Order> UpsertOrderAsync(Order order)
        {
            // Ensure the Customer is tracked by the context
            if (order.Customer != null)
            {
                if (order.Customer.Id == 0)
                {
                    _context.Entry(order.Customer).State = EntityState.Added;
                }
                else
                {
                    _context.Entry(order.Customer).State = EntityState.Modified;
                }
            }

            _context.Entry(order).State = order.Id == 0 ? EntityState.Added : EntityState.Modified;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<OrderItem> UpsertOrderItemAsync(OrderItem orderItem)
        {
            _context.Entry(orderItem).State = orderItem.Id == 0 ? EntityState.Added : EntityState.Modified;
            await _context.SaveChangesAsync();
            return orderItem;
        }
    }
}


// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using ecommapp.Data;
// using ecommapp.Models;

// namespace ecommapp.Services
// {
//     public class OrderRepository : Repository<Order>
//     {
//         private readonly AppDbContext _context;

//         public OrderRepository(AppDbContext context) : base(context)
//         {
//             _context = context;
//         }

//         public async Task<IEnumerable<Order>> GetOrdersWithCustomerNamesAsync(int skip, int take, int? customerId = null)
//         {
//             var query = _context.Orders.AsQueryable();

//             if (customerId.HasValue)
//             {
//                 query = query.Where(o => o.CustomerId == customerId.Value);
//             }

//             var orders = await query
//                 .Skip(skip)
//                 .Take(take)
//                 .Include(o => o.Customer)
//                 .ToListAsync();

//             return orders;
//         }

//         public async Task<Order> GetOrderWithCustomerAsync(int orderId)
//         {
//             return await _context.Orders
//                 .Include(o => o.Customer)
//                 .FirstOrDefaultAsync(o => o.Id == orderId);
//         }

//         public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync(int orderId, int skip, int take)
//         {
//             return await _context.OrderItems
//                 .Where(i => i.OrderId == orderId)
//                 .Skip(skip)
//                 .Take(take)
//                 .ToListAsync();
//         }

//         public async Task<Order> UpsertOrderAsync(Order order)
//         {
//             // Ensure the Customer is tracked by the context
//             if (order.Customer != null)
//             {
//                 if (order.Customer.Id == 0)
//                 {
//                     _context.Entry(order.Customer).State = EntityState.Added;
//                 }
//                 else
//                 {
//                     _context.Entry(order.Customer).State = EntityState.Modified;
//                 }
//             }

//             _context.Entry(order).State = order.Id == 0 ? EntityState.Added : EntityState.Modified;
//             await _context.SaveChangesAsync();
//             return order;
//         }

//         public async Task<OrderItem> UpsertOrderItemAsync(OrderItem orderItem)
//         {
//             _context.Entry(orderItem).State = orderItem.Id == 0 ? EntityState.Added : EntityState.Modified;
//             await _context.SaveChangesAsync();
//             return orderItem;
//         }
        
//     }
// }
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ecommapp.Data;
using ecommapp.Models;
using ecommapp.DTO;

namespace ecommapp.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync(int skip, int take, int? customerId = null)
        {
            var query = _context.Orders.AsQueryable();

            if (customerId.HasValue)
            {
                query = query.Where(o => o.Customer.Id == customerId.Value);
            }

            var orders = await query
                .Include(o => o.Customer)
                .Skip(skip)
                .Take(take)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    CustomerName = o.Customer.Name,
                    Customer = o.Customer
                })
                .ToListAsync();

            return orders;
        }

        public async Task<Order> GetOrderWithCustomerAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
        public async Task<Order> UpsertOrderAsync(Order order)
        {
            if (order.Customer == null)
            {
                throw new InvalidOperationException("Customer must be provided.");
            }

            foreach (var orderItem in order.OrderItems)
            {
                var product = await _context.Products.FindAsync(orderItem.ProductId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {orderItem.ProductId} not found.");
                }
                orderItem.Product = product;
                orderItem.Order = order; // Link the order item to the order
            }

            if (order.Id == 0)
            {
                _context.Orders.Add(order);
            }
            else
            {
                _context.Entry(order).State = EntityState.Modified;
                foreach (var orderItem in order.OrderItems)
                {
                    if (orderItem.Id == 0)
                    {
                        _context.Entry(orderItem).State = EntityState.Added;
                    }
                    else
                    {
                        _context.Entry(orderItem).State = EntityState.Modified;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return order;
        }
        // public async Task<Order> UpsertOrderAsync(Order order)
        // {
        //     // Ensure the Customer is tracked by the context
        //     if (order.Customer.Id!= 0)
        //     {
        //         var customer = await _context.Customers.FindAsync(order.Customer.Id);
        //         if (customer != null)
        //         {
        //             order.Customer = customer;
        //             order.Customer.Name = customer.Name;
        //         }
        //         else
        //         {
        //             throw new KeyNotFoundException($"Customer with ID {order.Customer.Id} not found.");
        //         }
        //     }

        //     if (order.Id == 0)
        //     {
        //         _context.Orders.Add(order);
        //     }
        //     else
        //     {
        //         _context.Entry(order).State = EntityState.Modified;
        //     }

        //     if (order.OrderItems != null && order.OrderItems.Any())
        //     {
        //         foreach (var orderItem in order.OrderItems)
        //         {
        //             if (orderItem.ProductId != 0)
        //             {
        //                 var product = await _context.Products.FindAsync(orderItem.ProductId);
        //                 if (product != null)
        //                 {
        //                     orderItem.Product = product;
        //                 }
        //                 else
        //                 {
        //                     throw new KeyNotFoundException($"Product with ID {orderItem.ProductId} not found.");
        //                 }
        //             }
        //             orderItem.Order = order; // Link the OrderItem to the Order

        //             if (orderItem.Id == 0)
        //             {
        //                 _context.Entry(orderItem).State = EntityState.Added;
        //             }
        //             else
        //             {
        //                 _context.Entry(orderItem).State = EntityState.Modified;
        //             }
        //         }
        //     }

        //     await _context.SaveChangesAsync();
        //     return order;
        // }
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
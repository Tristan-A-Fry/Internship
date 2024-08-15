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

        public async Task<IEnumerable<GetOrderDto>> GetOrdersAsync(int skip, int take, int? customerId = null)
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
                .Select(o => new GetOrderDto
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
                orderItem.Order = order;
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
    }
}
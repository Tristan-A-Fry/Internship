using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommapp.Data;
using ecommapp.DTO;
using ecommapp.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommapp.Services
{
    public class OrderItemRepository : IOrderItemRepository
    {
       private readonly AppDbContext _context;

       public OrderItemRepository(AppDbContext context)
       {
            _context = context;
       }
        public async Task<IEnumerable<GetOrderItemDto>> GetOrderItemsAsync(int orderId, int skip, int take)
        {
            return await _context.OrderItems
                .Where(i => i.OrderId == orderId)
                .Skip(skip)
                .Take(take)
                .Include(i => i.Product)
                .ThenInclude(p => p.Supplier)
                .Select(i => new GetOrderItemDto
                {
                    Id = i.Id,
                    OrderId = i.OrderId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.Product.Price,
                    TotalPrice = i.Quantity * i.Product.Price,
                    SupplierName = i.Product.Supplier.Name
                })
                .ToListAsync();
        }

       public async Task<OrderItem> UpsertOrderItemsAsync(OrderItem orderItem)
       {
            _context.Entry(orderItem).State = orderItem.Id == 0 ? EntityState.Added : EntityState.Modified;
            await _context.SaveChangesAsync();
            return orderItem;
       }
    }
}
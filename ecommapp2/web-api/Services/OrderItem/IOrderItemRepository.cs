using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommapp.DTO;
using ecommapp.Models;

namespace ecommapp.Services
{
    public interface IOrderItemRepository
    {
       public Task<IEnumerable<GetOrderItemDto>> GetOrderItemsAsync(int orderId, int skip, int take);
       public Task<OrderItem> UpsertOrderItemsAsync(OrderItem orderItem);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommapp.DTO;
using ecommapp.Models;

namespace ecommapp.Services
{
    public interface IOrderRepository
    {
       public  Task<IEnumerable<GetOrderDto>> GetOrdersAsync(int skip, int take, int? customerId);
       public Task<Order> GetOrderWithCustomerAsync(int orderId);
       public Task<Order> UpsertOrderAsync(Order order);
    }
}
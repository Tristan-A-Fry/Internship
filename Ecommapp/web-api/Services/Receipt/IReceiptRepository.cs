using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommapp.Models;

namespace ecommapp.Services
{
    public interface IReceiptRepository
    {
        Task<string> GenerateReceiptAsync(Order order);
    }
}
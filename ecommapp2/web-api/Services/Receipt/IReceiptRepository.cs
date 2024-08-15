using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommapp.Models;

namespace ecommapp.Services
{
    public interface IReceiptRepository
    {
        Task<string> GenerateReceiptAsync(IEnumerable<Order> ordes, string folderPath);
    }
}
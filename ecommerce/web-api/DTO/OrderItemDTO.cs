using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommapp.DTO
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int OrderId {get;set;}
        public string ProductName { get; set;}
        public int Quantity { get; set; }
        public decimal UnitPrice{get; set;}
        public decimal TotalPrice {get; set;}
        public string SupplierName{get;set;}
    }
}
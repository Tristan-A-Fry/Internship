using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exercise1prob2.Models
{
    public class OrderItem
    {
        public int Id {get; set;}
        public int OrderId {get; set;}
        public int ProductId {get; set;}
        public int Quantity {get; set;}

        public Order Order {get; set;}
        public Product Product {get; set;}
    }
}
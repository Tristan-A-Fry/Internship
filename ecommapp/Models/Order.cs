using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommapp.Models
{
    public class Order
    {
        public string Id {get; set;}
        public int CustomerId {get; set;} 
        public DateTime OrderDate{get; set;}

        public Customer Customer{get; set;}
        public List<OrderItem> OrderItems {get; set;}
    }
}
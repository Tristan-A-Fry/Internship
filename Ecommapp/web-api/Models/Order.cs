using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ecommapp.Models
{
    public class Order
    {
        public int Id {get; set;}
        public DateTime OrderDate{get; set;}

        public  Customer Customer{get; set;}

        public ICollection<OrderItem> OrderItems {get; set;} = new List<OrderItem>();


    }
}
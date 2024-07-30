using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ecommapp.Models
{
    public class OrderItem
    {
        public int Id {get; set;}

        

        [Required]
        public Order Order {get; set;}
        public int OrderId {get; set;}

        [Required]
        public Product Product {get; set;}

        [Required]
        public int ProductId {get; set;}
        public int Quantity {get; set;}
    }

}
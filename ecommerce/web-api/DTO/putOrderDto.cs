using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommapp.DTO
{
    public class putOrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public List<putOrderItemDto> OrderItems { get; set; } 
    }
}
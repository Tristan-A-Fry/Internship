using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommapp.DTO
{
    public class ProductDTO
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public decimal Price {get; set;}
        public string Description {get; set;}
        public string SupplierName {get; set;}
    }
}
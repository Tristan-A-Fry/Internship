using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ecommapp.Models
{
    public class Product
    {
       public int Id {get; set;} 
       public string Name{get; set;}
       public string Description {get; set;}
       public decimal Price {get; set;}

       public int SupplierId{get; set;}
       public Supplier Supplier {get; set;}

       public int ProductCategoryId{get; set;}
       public ProductCategory ProductCategory{get; set;} 
    }
}

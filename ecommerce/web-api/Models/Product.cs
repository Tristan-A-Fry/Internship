using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommapp.Models
{
    public class Product
    {
       public int Id {get; set;} 
       public string Name{get; set;}
       public int ProductCategoryId{get; set;}
       public int SupplierId{get; set;}
       public string Description {get; set;}
    }
}

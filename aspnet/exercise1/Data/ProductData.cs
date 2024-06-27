using exercise1.Models;

namespace exercise1.Data
{
    public static class ProductData
    {
        public static List<Product> GetProducts()
        {
            return new List<Product>()
            {
                new Product
                {
					Id = 1,
                    Category = "Electronics",
                    Name = "PlayStation 5",
                    Price = 599.9M
                },
                new Product
                {
					Id = 2,
                    Category = "Electronics",
                    Name = "Latitude 5540 Laptop",
                    Price = 1289.0M
                },
                new Product
                {
					Id = 3,
                    Category = "Home & Kitchen",
                    Name = "Dyson V15 Detect Absolute",
                    Price = 749.0M
                },
                new Product
                {
					Id = 4,
                    Category = "Home & Kitchen",
                    Name = "Ninja Professional Plus 72 fl. oz. Blender with Auto-iQ, Black/Gray",
                    Price = 116.99M
                },
                new Product
                {
					Id = 5,
                    Category = "Personal Care",
                    Name = "Johnson & Johnson",
                    Price = 5.90M
                },
                new Product
                {
					Id =6,
                    Category = "Personal Care",
                    Name = "Gillette Sensor3 Comfort Disposable Razors for Men",
                    Price = 11.97M
                }
            };
        }
    }

    // public class Product
    // {
    //     public string Category { get; set; }
    //     public string Name { get; set; }
    //     public decimal Price { get; set; }
    // }
}

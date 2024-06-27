using exercise1.Data;
using exercise1.Models;
using System.Collections.Generic;
using System.Linq;

namespace exercise1.Services
{
    public class ProductService : IProductService
    {
        private List<Product> _products;

        public ProductService()
        {
            _products = ProductData.GetProducts();
        }

        public List<Product> GetProducts(int skip, int take)
        {
            return _products.Skip(skip).Take(take).ToList();
        }

        public Product SaveProduct(Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Category = product.Category;
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
            }
            else
            {
                product.Id = _products.Max(p => p.Id) + 1;
                _products.Add(product);
            }
            return product;
        }

        public void DeleteProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}


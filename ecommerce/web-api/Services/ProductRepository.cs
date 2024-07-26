using ecommapp.Data;
using ecommapp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommapp.DTO;

namespace ecommapp.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDTO> GetProductAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Supplier)
                .Where(p => p.Id == id)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    SupplierName = p.Supplier.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsAsync(int skip, int take)
        {
            return await _context.Products
                .Include(p => p.Supplier)
                .Skip(skip)
                .Take(take)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    SupplierName = p.Supplier.Name
                })
                .ToListAsync();
        }

        public async Task<ProductDTO> UpsertProductAsync(Product product)
        {
            if (product.Id == 0)
            {
                _context.Products.Add(product);
            }
            else
            {
                _context.Entry(product).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            var updatedProduct = await _context.Products
                .Include(p => p.Supplier)
                .Where(p => p.Id == product.Id)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    SupplierName = p.Supplier.Name
                })
                .FirstOrDefaultAsync();

            return updatedProduct;
        }
    }
}





// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using ecommapp.Data;
// using ecommapp.Models;
// using Microsoft.EntityFrameworkCore;

// namespace ecommapp.Services
// {
//     public class ProductRepository : IProductRepository
//     {
//         private readonly AppDbContext _context;

//         public ProductRepository(AppDbContext context)
//         {
//             _context = context;
//         }

//         public async Task<Product> GetProductAsync(int id)
//         {
//             return await _context.Products
//                 .Include(p => p.Supplier)
//                 .Include(p => p.ProductCategory)
//                 .FirstOrDefaultAsync(p => p.Id == id);
//         }

//         public async Task<IEnumerable<Product>> GetProductsAsync(int skip, int take)
//         {
//             return await _context.Products
//                 .Include(p => p.Supplier)
//                 .Include(p => p.ProductCategory)
//                 .Skip(skip)
//                 .Take(take)
//                 .ToListAsync();
//         }

//         public async Task<Product> UpsertProductAsync(Product product)
//         {
//             if(product.Id == 0)
//             {
//                 _context.Products.Add(product);
//             }
//             else
//             {
//                 _context.Entry(product).State = EntityState.Modified;
//             }

//             await _context.SaveChangesAsync();
//             return product;
//         }
//     }

// }
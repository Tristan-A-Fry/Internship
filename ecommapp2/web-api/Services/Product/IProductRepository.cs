using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommapp.Models;
using ecommapp.DTO;

namespace ecommapp.Services
{
    public interface IProductRepository
    {
        Task<ProductDTO> GetProductAsync(int id);
        Task<IEnumerable<ProductDTO>> GetProductsAsync(int skip, int take);
        Task<ProductDTO> UpsertProductAsync(Product product);
    }
}
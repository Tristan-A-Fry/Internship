using exercise1.Models;


namespace exercise1.Services
{
	public interface IProductService
	{
		List<Product> GetProducts(int skip, int take);

		Product SaveProduct(Product product);

		void DeleteProduct(int id);
	}
}
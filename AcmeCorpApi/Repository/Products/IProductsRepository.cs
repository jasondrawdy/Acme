using System.Collections.Generic;
using System.Threading.Tasks;
using AcmeCorpApi.Models;

namespace AcmeCorpApi.Repository
{
    public interface IProductsRepository
    {     
        Task<List<Product>> GetProductsAsync();

        Task<Product> GetProductAsync(int id);
        
        Task<Product> InsertProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
    }
}
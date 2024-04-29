using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AcmeCorpApi.Models;

namespace AcmeCorpApi.Repository
{
    public class ProductsRepository : IProductsRepository
    {

        private readonly ProductsDbContext _context;
        private readonly ILogger _logger;

        public ProductsRepository(ProductsDbContext context, ILoggerFactory loggerFactory) 
        {
          _context = context;
          _logger = loggerFactory.CreateLogger("ProductsRepository");
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<Product> GetProductAsync(int id)
        {
            return await _context.Products.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Product> InsertProductAsync(Product product)
        {
            _context.Add(product);
            try
            {
              await _context.SaveChangesAsync();
            }
            catch (Exception exp)
            {
                _logger.LogError($"Error in {nameof(InsertProductAsync)}: " + exp.Message);
            }

            return product;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            _context.Products.Attach(product);
            _context.Entry(product).State = EntityState.Modified;
            try
            {
              return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(UpdateProductAsync)}: " + e.Message);
            }
            return false;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(c => c.Id == id);

            if (product is Product) // Avoids a 500 error.
              _context.Remove(product);
            else
              return false;

            try
            {
              return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception e)
            {
              _logger.LogError($"Error in {nameof(DeleteProductAsync)}: " + e.Message);
            }
            return false;
        }

    }
}
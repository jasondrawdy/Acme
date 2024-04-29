using System.Collections.Generic;
using System.Threading.Tasks;
using AcmeCorpApi.Models;

namespace AcmeCorpApi.Repository
{
    public interface IOrdersRepository
    {     
        Task<List<Order>> GetOrdersAsync();

        Task<Order> GetOrderAsync(int id);
        
        Task<Order> InsertOrderAsync(Order order);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int id);
    }
}
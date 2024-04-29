using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AcmeCorpApi.Models;

namespace AcmeCorpApi.Repository
{
    public class OrdersRepository : IOrdersRepository
    {

        private readonly OrdersDbContext _context;
        private readonly ILogger _logger;

        public OrdersRepository(OrdersDbContext context, ILoggerFactory loggerFactory) 
        {
          _context = context;
          _logger = loggerFactory.CreateLogger("OrdersRepository");
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _context.Orders.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            return await _context.Orders.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Order> InsertOrderAsync(Order order)
        {
            _context.Add(order);
            try
            {
              await _context.SaveChangesAsync();
            }
            catch (Exception exp)
            {
                _logger.LogError($"Error in {nameof(InsertOrderAsync)}: " + exp.Message);
            }

            return order;
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _context.Orders.Attach(order);
            _context.Entry(order).State = EntityState.Modified;
            try
            {
              return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(UpdateOrderAsync)}: " + e.Message);
            }
            return false;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(c => c.Id == id);
            
            if (order is Order) // Avoids a 500 error.
              _context.Remove(order);
            else
              return false;

            try
            {
              return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception e)
            {
              _logger.LogError($"Error in {nameof(DeleteOrderAsync)}: " + e.Message);
            }
            return false;
        }

    }
}
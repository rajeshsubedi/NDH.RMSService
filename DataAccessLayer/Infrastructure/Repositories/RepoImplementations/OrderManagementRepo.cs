using DataAccessLayer.Infrastructure.Data;
using DataAccessLayer.Infrastructure.Repositories.RepoInterfaces;
using DomainLayer.Models.DataModels.OrderManagementModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.Repositories.RepoImplementations
{
    public class OrderManagementRepo : IOrderManagementRepo
    {
        private readonly RMSServiceDbContext _context;

        public OrderManagementRepo(RMSServiceDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OrderDetails order)
        {
            await _context.OrderDetails.AddAsync(order);
        }
        public async Task<OrderDetails?> GetByIdAsync(Guid orderId)
        {
            return await _context.OrderDetails
                .Include(o => o.OrderedItems)
                .ThenInclude(oi => oi.FoodItem)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<List<OrderDetails>> GetAllOrdersAsync() 
        {
            return await _context.OrderDetails
                .Include(o => o.OrderedItems)
                .ThenInclude(oi => oi.FoodItem)
                .ToListAsync(); 
        }

        public async Task<List<OrderDetails>> GetOrdersByUserIdAsync(Guid userId)
        {
            // Use EF Core to fetch orders where the UserId matches
            return await _context.OrderDetails
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderedItems)  // Include related order items if needed
                .ThenInclude(oi => oi.FoodItem)
                .Include(o => o.User)            // Include user details if needed
                .ToListAsync();
        }

        public async Task<List<OrderDetails>> GetOrdersByUserIdAndDateAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            var startDateOnly = startDate.Date;
            var endDateOnly = endDate.Date;

            return await _context.OrderDetails
                .Where(o => o.UserId == userId &&
                            o.OrderDate.Date >= startDateOnly &&
                            o.OrderDate.Date <= endDateOnly)
                .Include(o => o.OrderedItems)
                    .ThenInclude(oi => oi.FoodItem)  // Include FoodItem related to each OrderedItem
                .Include(o => o.User)  // Include user details if needed
                .ToListAsync();
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}

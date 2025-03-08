using DomainLayer.Models.DataModels.MenuManagementModels;
using DomainLayer.Models.DataModels.OrderManagementModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.Repositories.RepoInterfaces
{
    public interface IOrderManagementRepo
    {
        Task AddAsync(OrderDetails order);
        Task<List<OrderDetails>> GetAllOrdersAsync();
        Task<OrderDetails> GetByIdAsync(Guid orderId);
        Task<List<OrderDetails>> GetOrdersByUserIdAsync(Guid userId);
        Task<List<OrderDetails>> GetOrdersByUserIdAndDateAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task SaveChangesAsync();
    }
}

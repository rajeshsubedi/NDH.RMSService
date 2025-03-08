using DomainLayer.Models.DataModels.OrderManagementModels;
using DomainLayer.Wrappers.DTO.OrderManagementDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.ServiceInterfaces
{
    public interface IOrderManagementService
    {
        Task<Guid> PlaceOrderAsync(PlaceOrderRequestDTO orderRequest);
        Task<List<OrderDetailsResponseDTO>> GetAllOrdersAsync();
        Task<OrderDetailsResponseDTO?> GetOrderByIdAsync(Guid orderId);
        Task<List<OrderDetailsResponseDTO>> GetOrdersByUserIdAsync(Guid userId);
        Task<List<OrderDetailsResponseDTO>> GetOrdersByUserIdAndDateAsync(Guid userId, DateTime startDate, DateTime endDate);
    }
}

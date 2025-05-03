using AutoMapper;
using DataAccessLayer.Infrastructure.Repositories.RepoImplementations;
using DataAccessLayer.Infrastructure.Repositories.RepoInterfaces;
using DomainLayer.Exceptions;
using DomainLayer.Models.DataModels.OrderManagementModels;
using DomainLayer.Wrappers.DTO.OrderManagementDTO;
using Serilog;
using ServicesLayer.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.ServiceImplementations
{
    public class OrderManagementService: IOrderManagementService
    {
        private readonly IOrderManagementRepo _orderRepository;
        private readonly IMenuManagementRepo _menuManagementRepository;
        private readonly IMapper _mapper;
    
        public OrderManagementService(IOrderManagementRepo orderRepository, IMenuManagementRepo menuManagementRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _menuManagementRepository = menuManagementRepository;
            _mapper = mapper;
        }

        public async Task<Guid> PlaceOrderAsync(PlaceOrderRequestDTO orderRequestDTO)
        {
            try
            {
                var order = new OrderDetails
                {
                    OrderId = Guid.NewGuid(),
                    UserId = orderRequestDTO.UserId,
                    OrderDate = DateTime.UtcNow,
                    OrderStatus = orderRequestDTO.OrderStatus.ToString(), // Using the provided status
                    IsAsSoonAsPossible = orderRequestDTO.IsAsSoonAsPossible,
                    DeliveryDateTime = orderRequestDTO.IsAsSoonAsPossible ? null : orderRequestDTO.DeliveryDateTime, // Set delivery time if not ASAP
                    TotalAmount = 0m, // Initialize the total amount
                    OrderedItems = new List<OrderedItemsDetails>()
                };

                decimal totalAmount = 0;

                foreach (var item in orderRequestDTO.OrderedFoodItems)
                {
                    var foodItem = await _menuManagementRepository.GetFoodItemByIdAsync(item.FoodItemId);
                    if (foodItem == null)
                    {
                        throw new NotFoundException($"Food item with ID {item.FoodItemId} not found.");
                    }

                    var unitPrice = foodItem.Price ?? 0;
                    var totalPrice = unitPrice * item.Quantity;

                    var orderItem = new OrderedItemsDetails
                    {
                        OrderItemId = Guid.NewGuid(),
                        OrderId = order.OrderId,
                        FoodItemId = item.FoodItemId,
                        Quantity = item.Quantity,
                        UnitPrice = unitPrice,
                        TotalPrice = totalPrice,
                        ItemName = foodItem.Name
                    };

                    order.OrderedItems.Add(orderItem);
                    totalAmount += totalPrice;
                }

                order.TotalAmount = totalAmount;

                // Add delivery address details
                if (orderRequestDTO.DeliveryAddress != null)
                {
                    var deliveryAddress = new DeliveryAddressDetails
                    {
                        OrderId = order.OrderId,
                        StreetAddress = orderRequestDTO.DeliveryAddress.Street,
                        City = orderRequestDTO.DeliveryAddress.City,
                        State = orderRequestDTO.DeliveryAddress.State,
                        ZipCode = orderRequestDTO.DeliveryAddress.PostalCode,
                        Country = orderRequestDTO.DeliveryAddress.Country
                    };

                    order.DeliveryAddress = deliveryAddress;
                }

                // Add payment method
                var paymentOption = new PaymentOptionDetails
                {
                    PaymentId = Guid.NewGuid(),
                    OrderId = order.OrderId,
                    PaymentMethod = orderRequestDTO.PaymentMethod.ToString(),
                    PaymentStatus = orderRequestDTO.PaymentStatus.ToString(),
                    PaymentDate = DateTime.Now,
                };
                order.PaymentOption = paymentOption;

                await _orderRepository.AddAsync(order);
                await _orderRepository.SaveChangesAsync();

                return order.OrderId;
            }
            catch (Exception ex)
            {
                Log.Error($"Error while placing the order {ex.Message}");
                throw new CustomInvalidOperationException($"Error while placing the order {ex.Message}");
            }
   
        }

        public async Task<OrderDetailsResponseDTO> GetOrderByIdAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                throw new CustomInvalidOperationException("Invalid Order ID provided.");
            }

            var order = await _orderRepository.GetByIdAsync(orderId)
                         ?? throw new CustomInvalidOperationException("Order not found.");
            var orderDto = _mapper.Map<OrderDetailsResponseDTO>(order);

            return orderDto;
        }

        public async Task<List<OrderDetailsResponseDTO>> GetAllOrdersAsync() // ✅ Return a list
        {
            var orders = await _orderRepository.GetAllOrdersAsync(); // ✅ Fetch all orders

            if (orders == null || !orders.Any())
            {
                return new List<OrderDetailsResponseDTO>(); // ✅ Return an empty list instead of null
            }

            var orderDtos = _mapper.Map<List<OrderDetailsResponseDTO>>(orders); // ✅ Map list to DTOs
            return orderDtos;
        }

        public async Task<List<OrderDetailsResponseDTO>> GetOrdersByUserIdAsync(Guid userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            return _mapper.Map<List<OrderDetailsResponseDTO>>(orders);
        }

        public async Task<List<OrderDetailsResponseDTO>> GetOrdersByUserIdAndDateAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAndDateAsync(userId, startDate, endDate);

            return _mapper.Map<List<OrderDetailsResponseDTO>>(orders);
        }

        public async Task UpdateOrderAsync(Guid orderId, PlaceOrderRequestDTO updatedOrderDto)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(orderId);

            if (existingOrder == null)
                throw new NotFoundException($"Order with ID {orderId} not found.");

            // You can optionally clear and repopulate ordered items
            existingOrder.OrderedItems.Clear();
            decimal totalAmount = 0;

            foreach (var item in updatedOrderDto.OrderedFoodItems)
            {
                var foodItem = await _menuManagementRepository.GetFoodItemByIdAsync(item.FoodItemId);
                if (foodItem == null)
                    throw new NotFoundException($"Food item with ID {item.FoodItemId} not found.");

                var orderItem = new OrderedItemsDetails
                {
                    OrderItemId = Guid.NewGuid(),
                    OrderId = existingOrder.OrderId,
                    FoodItemId = item.FoodItemId,
                    Quantity = item.Quantity,
                    UnitPrice = foodItem.Price ?? 0,
                    TotalPrice = (foodItem.Price ?? 0) * item.Quantity,
                    ItemName = foodItem.Name
                };
                existingOrder.OrderedItems.Add(orderItem);
                totalAmount += orderItem.TotalPrice;
            }

            existingOrder.OrderDate = DateTime.UtcNow;
            existingOrder.OrderStatus = updatedOrderDto.OrderStatus.ToString();
            existingOrder.IsAsSoonAsPossible = updatedOrderDto.IsAsSoonAsPossible;
            existingOrder.DeliveryDateTime = updatedOrderDto.IsAsSoonAsPossible ? null : updatedOrderDto.DeliveryDateTime;
            existingOrder.TotalAmount = totalAmount;

            existingOrder.DeliveryAddress = new DeliveryAddressDetails
            {
                OrderId = orderId,
                StreetAddress = updatedOrderDto.DeliveryAddress.Street,
                City = updatedOrderDto.DeliveryAddress.City,
                State = updatedOrderDto.DeliveryAddress.State,
                ZipCode = updatedOrderDto.DeliveryAddress.PostalCode,
                Country = updatedOrderDto.DeliveryAddress.Country
            };

            existingOrder.PaymentOption = new PaymentOptionDetails
            {
                PaymentId = Guid.NewGuid(),
                OrderId = orderId,
                PaymentMethod = updatedOrderDto.PaymentMethod.ToString(),
                PaymentStatus = updatedOrderDto.PaymentStatus.ToString(),
                PaymentDate = DateTime.Now
            };

            await _orderRepository.UpdateAsync(existingOrder);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(Guid orderId)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(orderId);
            if (existingOrder == null)
                throw new NotFoundException($"Order with ID {orderId} not found.");

            await _orderRepository.DeleteAsync(existingOrder);
            await _orderRepository.SaveChangesAsync();
        }

    }
}

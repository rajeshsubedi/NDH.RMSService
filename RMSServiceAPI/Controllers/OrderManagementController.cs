using DomainLayer.Exceptions;
using DomainLayer.Models.DataModels.OrderManagementModels;
using DomainLayer.Wrappers.DTO.OrderManagementDTO;
using DomainLayer.Wrappers.GlobalResponse;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ServicesLayer.ServiceInterfaces;
using System.Net;

namespace RMSServiceAPI.Controllers
{
    public class OrderManagementController : ControllerBase
    {
        private readonly IOrderManagementService _orderService;

        public OrderManagementController(IOrderManagementService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("place-order")] 
        public async Task<BaseResponse<Guid>> PlaceOrder([FromBody] PlaceOrderRequestDTO orderRequest)
        {

            if (orderRequest == null || !ModelState.IsValid)
            {
                throw new CustomInvalidOperationException("All fields are required and must be valid.");
            }

            // Additional custom validation
            if (orderRequest.PaymentStatus == null || !Enum.IsDefined(typeof(PaymentStatus), orderRequest.PaymentStatus))
            {
                throw new CustomInvalidOperationException("A valid PaymentStatus is required.");
            }

            if (orderRequest.DeliveryAddress == null)
            {
                throw new CustomInvalidOperationException("Delivery address is required.");
            }

            if (orderRequest.OrderedFoodItems == null || !orderRequest.OrderedFoodItems.Any())
            {
                throw new CustomInvalidOperationException("At least one food item must be ordered.");
            }
            // Ensure all ordered food items have a quantity greater than zero
            if (orderRequest.OrderedFoodItems.Any(item => item.Quantity <= 0))
            {
                throw new CustomInvalidOperationException("All ordered food items must have a quantity greater than zero.");
            }
            try
            {
                var orderId = await _orderService.PlaceOrderAsync(orderRequest);
                return new BaseResponse<Guid>(
                    orderId,
                    HttpStatusCode.OK,
                    true,
                    "Order placed successfully."
                );
            }
            catch(CustomInvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Log exception
                Log.Error("An error occurred while placing the order. {ex.Message}");
               throw new CustomInvalidOperationException($"An error occurred while placing the order. {ex.Message}");
            }
        }

        [HttpGet("all")]
        public async Task<BaseResponse<List<OrderDetailsResponseDTO>>> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                if (orders == null || !orders.Any())
                {
                    throw new CustomInvalidOperationException("No orders found.");
                }

                return new BaseResponse<List<OrderDetailsResponseDTO>>(
                    orders,
                    HttpStatusCode.OK,
                    true,
                    "Orders retrieved successfully."
                );
            }
            catch (CustomInvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Log exception
                Log.Error($"An error occurred while retrieving all orders. {ex.Message}");
                throw new CustomInvalidOperationException($"An error occurred while retrieving all orders. {ex.Message}");
            }
        }

        [HttpGet("{orderid}")]
        public async Task<BaseResponse<OrderDetailsResponseDTO>> GetOrder(Guid orderid)
        {
            try
            {
                if (orderid == Guid.Empty)
                {
                    throw new CustomInvalidOperationException("Invalid Order ID provided.");
                }

                var orderDto = await _orderService.GetOrderByIdAsync(orderid);

                return new BaseResponse<OrderDetailsResponseDTO>(
                                orderDto,
                                HttpStatusCode.OK,
                                true,
                                "Order retrieved successfully."
                            );
            }
            catch (CustomInvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Log exception
                Log.Error("An error occurred while retrieving the order. {ex.Message}");
               throw new CustomInvalidOperationException($"An error occurred while retrieving the order. {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<BaseResponse<List<OrderDetailsResponseDTO>>> GetOrdersByUserId(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    throw new CustomInvalidOperationException("Invalid User ID.");
                }

                var ordersDto = await _orderService.GetOrdersByUserIdAsync(userId);
                if (ordersDto == null || ordersDto.Count == 0)
                {
                    return new BaseResponse<List<OrderDetailsResponseDTO>>(
                        new List<OrderDetailsResponseDTO>(),
                        HttpStatusCode.NotFound,
                        false,
                        "No orders found for this user."
                    );
                }

                return new BaseResponse<List<OrderDetailsResponseDTO>>(
                    ordersDto,
                    HttpStatusCode.OK,
                    true,
                    "Orders retrieved successfully."
                );
            }
            catch (CustomInvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Log exception
                Log.Error($"An error occurred while retrieving orders. {ex.Message}");
                throw new CustomInvalidOperationException($"An error occurred while retrieving orders. {ex.Message}");
            }
        }

        [HttpGet("user-date/{userId}")]
        public async Task<BaseResponse<List<OrderDetailsResponseDTO>>> GetOrdersByUserIdAndDate(Guid userId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (userId == Guid.Empty || startDate == default || endDate == default || startDate > endDate)
                {
                    throw new CustomInvalidOperationException("Invalid input parameters.");
                }

                var ordersDto = await _orderService.GetOrdersByUserIdAndDateAsync(userId, startDate, endDate);
                if (ordersDto == null || ordersDto.Count == 0)
                {
                    return new BaseResponse<List<OrderDetailsResponseDTO>>(
                        new List<OrderDetailsResponseDTO>(),
                        HttpStatusCode.NotFound,
                        false,
                        "No orders found for this user in the specified date range."
                    );
                }

                return new BaseResponse<List<OrderDetailsResponseDTO>>(
                    ordersDto,
                    HttpStatusCode.OK,
                    true,
                    "Orders retrieved successfully."
                );
            }
            catch (CustomInvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Log exception
                Log.Error($"An error occurred while retrieving orders. {ex.Message}");
                throw new CustomInvalidOperationException($"An error occurred while retrieving orders. {ex.Message}");
            }
        }

        [HttpPut("update-order/{orderId}")]
        public async Task<BaseResponse<string>> UpdateOrder(Guid orderId, [FromBody] PlaceOrderRequestDTO updatedOrderDto)
        {
            if (updatedOrderDto == null || !ModelState.IsValid)
                throw new CustomInvalidOperationException("Invalid order data.");

            try
            {
                await _orderService.UpdateOrderAsync(orderId, updatedOrderDto);
                return new BaseResponse<string>(
                    "Order updated successfully.",
                    HttpStatusCode.OK,
                    true,
                    "Order updated successfully."
                );
            }
            catch (NotFoundException ex)
            {
                return new BaseResponse<string>(null, HttpStatusCode.NotFound, false, ex.Message);
            }
            catch (CustomInvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating order {orderId}: {ex.Message}");
                throw new CustomInvalidOperationException($"Error updating order: {ex.Message}");
            }
        }

        [HttpDelete("delete-order/{orderId}")]
        public async Task<BaseResponse<string>> DeleteOrder(Guid orderId)
        {
            try
            {
                await _orderService.DeleteOrderAsync(orderId);
                return new BaseResponse<string>("Order deleted successfully.", HttpStatusCode.OK, true, "Order deleted successfully.");
            }
            catch (NotFoundException ex)
            {
                return new BaseResponse<string>(null, HttpStatusCode.NotFound, false, ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting order {orderId}: {ex.Message}");
                throw new CustomInvalidOperationException($"Error deleting order: {ex.Message}");
            }
        }


    }
}

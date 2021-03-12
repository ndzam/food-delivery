using FoodDeliveryWebApi.Filters;
using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using FoodDeliveryWebApi.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Services
{
    public interface IOrderService
    {
        Task<ApiResponse<List<Order>>> GetOrders(string userId, string role, OrderFilter filter);
        Task<ApiResponse<List<Order>>> GetOrdersByRestaurant(string restaurantId, OrderFilter filter);
        Task<ApiResponse<Order>> GetOrder(string orderId);
        Task<ApiResponse<Order>> UpdateOrderStatus(string id, Order req);
        Task<ApiResponse<Order>> CreateOrder(string userId, string restaurantOwnerId, OrderPostRequest req, double total);


    }
}

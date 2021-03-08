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
        List<Order> GetOrders(string userId);
        Order GetOrder(string orderId);
        Order UpdateOrder(string id, OrderPutRequest req);
        Task<ApiResponse<Order>> CreateOrder(string userId, OrderPostRequest req, Decimal total);


    }
}

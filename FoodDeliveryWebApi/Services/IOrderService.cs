using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
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
        

        Order CreateOrder(OrderPostRequest req);
      
    }
}

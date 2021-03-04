using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Services
{
    public class OrderService : IOrderService
    {
        public Order CreateOrder(OrderPostRequest req)
        {
            throw new NotImplementedException();
        }

        public Order GetOrder(string orderId)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetOrders(string userId)
        {
            throw new NotImplementedException();
        }

        public Order UpdateOrder(string id, OrderPutRequest req)
        {
            throw new NotImplementedException();
        }
    }
}

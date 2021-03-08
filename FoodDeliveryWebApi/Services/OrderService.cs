using FoodDeliveryWebApi.Constants;
using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using FoodDeliveryWebApi.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IFirebaseService _firebaseService;
        private IRestaurantService _restaurantService;
        //private readonly string _apiKey;
        public OrderService(IFirebaseService firebaseService, IRestaurantService restaurantService)
        {
            //_apiKey = options.Value.ApiKey;
            _firebaseService = firebaseService;
            _restaurantService = restaurantService;
        }

        public async Task<ApiResponse<Order>> CreateOrder(string userId, OrderPostRequest req, Decimal total)
        {  
            List<StatusHistoryItem> statusHistoryItems = new List<StatusHistoryItem>();
            statusHistoryItems.Add(new StatusHistoryItem
            {
                Status = OrderStatuses.PLACED,
                Date = DateTime.Now
            });
            Order order = new Order
            {
                UserId = userId,
                date = DateTime.Now,
                Items = req.Items,
                RestaurantId = req.RestaurantId,
                Status = new OrderStatus
                {
                    CurrentStatus = OrderStatuses.PLACED,
                    StatusHistory = statusHistoryItems
                },
                TotalPrice = total
            };
            string ser = JsonConvert.SerializeObject(order, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            try
            {
                var queryResult = await _firebaseService.GetFirebaseClient().Child(TableNames.ORDERS).PostAsync(ser);
                var key = queryResult.Key;
                Order result = JsonConvert.DeserializeObject<Order>(queryResult.Object);
                return new ApiResponse<Order>
                {
                    Success = true,
                    Data = result
                };
            } catch
            {
                return new ApiResponse<Order>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
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

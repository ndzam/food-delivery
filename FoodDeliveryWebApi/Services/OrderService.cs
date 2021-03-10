using FoodDeliveryWebApi.Common;
using FoodDeliveryWebApi.Constants;
using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using FoodDeliveryWebApi.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IFirebaseService _firebaseService;
        public OrderService(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        public async Task<ApiResponse<Order>> CreateOrder(string userId, string restaurantOwnerId, OrderPostRequest req, double total)
        {  
            List<StatusHistoryItem> statusHistoryItems = new List<StatusHistoryItem>();
            statusHistoryItems.Add(new StatusHistoryItem
            {
                Status = OrderStatuses.PLACED,
                Date = Utils.GetDateEpoch()
            });
            Order order = new Order
            {
                UserId = userId,
                RestaurantOwnerId = restaurantOwnerId,
                Date = Utils.GetDateEpoch(),
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
            var jsonObject = Utils.Deserialize(ser);
            try
            {
                var queryResult = await _firebaseService.GetFirestoreDb().Collection(TableNames.ORDERS).AddAsync(jsonObject);
                var key = queryResult.Id;
                order.OrderId = key;
                return new ApiResponse<Order>
                {
                    Success = true,
                    Data = order
                };
            } catch (Exception e)
            {
                return new ApiResponse<Order>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
        }

        public async Task<ApiResponse<Order>> GetOrder(string orderId)
        {
            var firebase = _firebaseService.GetFirestoreDb();
            try
            {
                var res = await firebase.Collection(TableNames.ORDERS).Document(orderId).GetSnapshotAsync();
                Dictionary<string, object> dic = res.ToDictionary();
                var json = JsonConvert.SerializeObject(dic, Newtonsoft.Json.Formatting.Indented);
                var order = JsonConvert.DeserializeObject<Order>(json);
                return new ApiResponse<Order>
                {
                    Data = order,
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new ApiResponse<Order>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
        }

        public async Task<ApiResponse<List<Order>>> GetOrders(string userId, string role)
        {
            return await GetOrdersByField(userId, role.Equals(UserRoles.USER) ? TableNames.ORDERS_USER_ID : TableNames.ORDERS_RESTAURANT_OWNER_ID);
        }

        public async Task<ApiResponse<List<Order>>> GetOrdersByRestaurant(string restaurantId)
        {
            return await GetOrdersByField(restaurantId, TableNames.ORDERS_RESTAURANT_ID);
        }

        private async Task<ApiResponse<List<Order>>> GetOrdersByField(string id, string field)
        {
            var firebase = _firebaseService.GetFirestoreDb();
            try
            {
                var res = await firebase.Collection(TableNames.ORDERS).WhereEqualTo(field, id).OrderByDescending(TableNames.ORDERS_DATE).GetSnapshotAsync();
                List<Order> list = new List<Order>();
                foreach (var v in res)
                {
                    Dictionary<string, object> dic = v.ToDictionary();
                    var json = JsonConvert.SerializeObject(dic, Newtonsoft.Json.Formatting.Indented);
                    var r = JsonConvert.DeserializeObject<Order>(json);
                    r.OrderId = v.Id;
                    list.Add(r);
                }
                return new ApiResponse<List<Order>>
                {
                    Data = list,
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new ApiResponse<List<Order>>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
        }

        public async Task<ApiResponse<Order>> UpdateOrderStatus(string id, Order req)
        {
            
            string ser = JsonConvert.SerializeObject(req, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var jsonObject = Utils.Deserialize(ser);
            var firebase = _firebaseService.GetFirestoreDb();
            try
            {
                await firebase.Collection(TableNames.ORDERS).Document(id).SetAsync(jsonObject);

                return new ApiResponse<Order>
                {
                    Success = true,
                    Data = req
                };
            }
            catch
            {
                return new ApiResponse<Order>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
        }



    }
}

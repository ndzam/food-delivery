using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using FoodDeliveryWebApi.Constants;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Firebase.Database.Query;
using FoodDeliveryWebApi.Response;
using Firebase.Database;

namespace FoodDeliveryWebApi.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IFirebaseService _firebaseService;

        public RestaurantService(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
            
        }

        public async Task<ApiResponse<Restaurant>> CreateRestaurantAsync(string userId, RestaurantPostRequest res)
        {

            Restaurant r = new Restaurant
            {

                UserId = userId,
                Name = res.Name,
                Description = res.Description
            };
            string ser = JsonConvert.SerializeObject(r, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var firebase = _firebaseService.GetFirebaseClient();
            try { 
                var queryResult = await firebase.Child(TableNames.RESTAURANTS).PostAsync(ser);
                var key = queryResult.Key;
                Restaurant result = JsonConvert.DeserializeObject<Restaurant>(queryResult.Object);
                result.Id = key;
                return new ApiResponse<Restaurant>
                {
                    Success = true,
                    Data = result
                };
            }
            /*catch (FirebaseException ex)
            {
                return new ApiResponse<Restaurant>
                {
                    Success = false,
                    ErrorCode = ex.InnerException.Message
                };
            }*/

            catch
            {
                return new ApiResponse<Restaurant>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
        }

        public async Task<ApiResponse<Food>> CreateFood(string restaurantId, FoodPostRequest food)
        {
            Food f = new Food
            {
                Name = food.Name,
                Description = food.Description,
                Price = food.Price
            };
            
            string ser = JsonConvert.SerializeObject(f, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var firebase = _firebaseService.GetFirebaseClient();
            try
            {
                var queryResult = await firebase.Child(TableNames.RESTAURANTS).Child(restaurantId).Child(TableNames.FOODS).PostAsync(ser);
                var key = queryResult.Key;
                Food result = JsonConvert.DeserializeObject<Food>(queryResult.Object);
                result.Id = key;
                return new ApiResponse<Food>
                {
                    Success = true,
                    Data = result
                };
            }
            /*catch (FirebaseException ex)
            {
                return new ApiResponse<Restaurant>
                {
                    Success = false,
                    ErrorCode = ex.InnerException.Message
                };
            }*/

            catch
            {
                return new ApiResponse<Food>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
        }

        public async Task<ApiResponse> DeleteRestaurantAsync(string id)
        {
            var firebase = _firebaseService.GetFirebaseClient();
            try {
                await firebase.Child(TableNames.RESTAURANTS).Child(id).DeleteAsync();
                return new ApiResponse
                {
                    Success = true
                };
            } catch
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
        }

        public async Task<ApiResponse> DeleteFood(string restaurantId, string id)
        {
            var firebase = _firebaseService.GetFirebaseClient();
            try
            {
                await firebase.Child(TableNames.RESTAURANTS).Child(id).Child(TableNames.FOODS).Child(id).DeleteAsync();
                return new ApiResponse
                {
                    Success = true
                };
            }
            catch
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
        }

        public async Task<ApiResponse<Restaurant>> GetRestaurant(string id)
        {
            var firebase = _firebaseService.GetFirebaseClient();
            try 
            {
                var res = await firebase.Child(TableNames.RESTAURANTS).Child(id).OnceSingleAsync<Restaurant>();
                return new ApiResponse<Restaurant>
                {
                    Data = res,
                    Success = true
                };
            } catch (Exception e)
            {
                return new ApiResponse<Restaurant>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
        }

        public async Task<ApiResponse<Food>> GetFood(string restaurantId, string id)
        {
            var firebase = _firebaseService.GetFirebaseClient();
            try
            {
                var res = await firebase.Child(TableNames.RESTAURANTS).Child(restaurantId).Child(TableNames.FOODS).Child(id).OnceSingleAsync<Food>();
                return new ApiResponse<Food>
                {
                    Data = res,
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new ApiResponse<Food>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
        }

        public async Task<ApiResponse<List<Restaurant>>> GetAllRestaurant()
        {
            var firebase = _firebaseService.GetFirebaseClient();
            try
            {
                var res = await firebase.Child(TableNames.RESTAURANTS).OnceAsync<Restaurant>();
                List<Restaurant> list = new List<Restaurant>();
                foreach(var v in res){
                    Restaurant r = v.Object;
                    r.Id = v.Key;
                    list.Add(r);
                }
                return new ApiResponse<List<Restaurant>>
                {
                    Data = list,
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new ApiResponse<List<Restaurant>>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
        }

        public async Task<ApiResponse<List<Food>>> GetAllFood(string restaurantId)
        {
            var firebase = _firebaseService.GetFirebaseClient();
            try
            {
                var res = await firebase.Child(TableNames.RESTAURANTS).Child(restaurantId).Child(TableNames.FOODS).OnceAsync<Food>();
                List<Food> list = new List<Food>();
                foreach (var v in res)
                {
                    Food r = v.Object;
                    r.Id = v.Key;
                    list.Add(r);
                }
                return new ApiResponse<List<Food>>
                {
                    Data = list,
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new ApiResponse<List<Food>>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
        }

        public async Task<ApiResponse<Restaurant>> UpdateRestaurant(string id, RestaurantPutRequest res)
        {
            Restaurant r = new Restaurant
            {
                Name = res.Name,
                Description = res.Description
            };
            string ser = JsonConvert.SerializeObject(r, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var firebase = _firebaseService.GetFirebaseClient();
            try
            {
                await firebase.Child(TableNames.RESTAURANTS).Child(id).PatchAsync(ser);
                
                return new ApiResponse<Restaurant>
                {
                    Success = true,
                    Data = r
                };
            }
            catch
            {
                return new ApiResponse<Restaurant>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
        }

        public async Task<ApiResponse<Food>> UpdateFood(string restaurantId, string id, FoodPutRequest req)
        {
            Food food = new Food
            {
                Name = req.Name,
                Description = req.Description,
                Price = req.Price
            };
            string ser = JsonConvert.SerializeObject(food, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var firebase = _firebaseService.GetFirebaseClient();
            try
            {
                await firebase.Child(TableNames.RESTAURANTS).Child(restaurantId).Child(TableNames.FOODS).Child(id).PatchAsync(ser);
                return new ApiResponse<Food>
                {
                    Success = true,
                    Data = food
                };
            }
            catch
            {
                return new ApiResponse<Food>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
        }
    }
}

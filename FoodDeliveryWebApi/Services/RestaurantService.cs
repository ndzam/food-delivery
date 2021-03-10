using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using System;
using System.Collections.Generic;
using FoodDeliveryWebApi.Constants;
using System.Threading.Tasks;
using FoodDeliveryWebApi.Response;

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
                OwnerId = userId,
                Name = res.Name,
                Description = res.Description
            };
            var firebase = _firebaseService.GetFirestoreDb();
            try
            {
                var queryResult = await firebase.Collection(TableNames.RESTAURANTS).AddAsync(r);
                r.Id = queryResult.Id;
                return new ApiResponse<Restaurant>
                {
                    Success = true,
                    Data = r
                };
            }
            catch (Exception e)
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
            var firebase = _firebaseService.GetFirestoreDb();
            try
            {
                var queryResult = await firebase.Collection(TableNames.RESTAURANTS).Document(restaurantId).Collection(TableNames.FOODS).AddAsync(f);
                return new ApiResponse<Food>
                {
                    Success = true,
                    Data = f
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

        public async Task<ApiResponse> DeleteRestaurantAsync(string id)
        {
            var firebase = _firebaseService.GetFirestoreDb();
            try
            {
                await firebase.Collection(TableNames.RESTAURANTS).Document(id).DeleteAsync();
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

        public async Task<ApiResponse> DeleteFood(string restaurantId, string id)
        {
            var firebase = _firebaseService.GetFirestoreDb();
            try
            {
                var res = await firebase.Collection(TableNames.RESTAURANTS).Document(restaurantId).Collection(TableNames.FOODS).Document(id).DeleteAsync();
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
            var firebase = _firebaseService.GetFirestoreDb();
            try
            {
                var res = await firebase.Collection(TableNames.RESTAURANTS).Document(id).GetSnapshotAsync();
                Restaurant r = res.ConvertTo<Restaurant>();
                return new ApiResponse<Restaurant>
                {
                    Data = r,
                    Success = true
                };
            }
            catch (Exception e)
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
            var firebase = _firebaseService.GetFirestoreDb();
            try
            {
                var res = await firebase.Collection(TableNames.RESTAURANTS).Document(restaurantId).Collection(TableNames.FOODS).Document(id).GetSnapshotAsync();
                Food f = res.ConvertTo<Food>();
                return new ApiResponse<Food>
                {
                    Data = f,
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
            var firebase = _firebaseService.GetFirestoreDb();
            try
            {
                var res = await firebase.Collection(TableNames.RESTAURANTS).GetSnapshotAsync();
                List<Restaurant> list = new List<Restaurant>();
                foreach (var v in res)
                {
                    Restaurant r = v.ConvertTo<Restaurant>();
                    r.Id = v.Id;
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
            var firebase = _firebaseService.GetFirestoreDb();
            try
            {
                var res = await firebase.Collection(TableNames.RESTAURANTS).Document(restaurantId).Collection(TableNames.FOODS).GetSnapshotAsync();
                List<Food> list = new List<Food>();
                foreach (var v in res)
                {
                    Food r = v.ConvertTo<Food>();
                    r.Id = v.Id;
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

        public async Task<ApiResponse<Restaurant>> UpdateRestaurant(string id, Restaurant res)
        {
            res.Id = id;
            var firebase = _firebaseService.GetFirestoreDb();
            try
            {
                await firebase.Collection(TableNames.RESTAURANTS).Document(id).SetAsync(res);

                return new ApiResponse<Restaurant>
                {
                    Success = true,
                    Data = res
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
            var firebase = _firebaseService.GetFirestoreDb();
            try
            {
                await firebase.Collection(TableNames.RESTAURANTS).Document(restaurantId).Collection(TableNames.FOODS).Document(id).SetAsync(food);
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

using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using System;
using System.Collections.Generic;
using FoodDeliveryWebApi.Constants;
using System.Threading.Tasks;
using FoodDeliveryWebApi.Response;
using Google.Cloud.Firestore;
using FoodDeliveryWebApi.Filters;

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
                Price = (double)food.Price
            };
            var firebase = _firebaseService.GetFirestoreDb();
            try
            {
                var queryResult = await firebase.Collection(TableNames.RESTAURANTS).Document(restaurantId).Collection(TableNames.FOODS).AddAsync(f);
                f.Id = queryResult.Id;
                return new ApiResponse<Food>
                {
                    Success = true,
                    Data = f
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

        public async Task<ApiResponse<Block>> CreateBlock(string restaurantId, BlockPostRequest req)
        {
            Block b = new Block
            {
                UserId = req.UserId
            };
            var firebase = _firebaseService.GetFirestoreDb();
            try
            {
                var queryResult = await firebase.Collection(TableNames.RESTAURANTS).Document(restaurantId).Collection(TableNames.BLOCKS).AddAsync(b);
                b.Id = queryResult.Id;
                return new ApiResponse<Block>
                {
                    Success = true,
                    Data = b
                };
            }
            catch
            {
                return new ApiResponse<Block>
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
                var collectionReference = firebase.Collection(TableNames.RESTAURANTS).Document(id).Collection(TableNames.FOODS);
                DeleteCollection(collectionReference);
                collectionReference = firebase.Collection(TableNames.RESTAURANTS).Document(id).Collection(TableNames.BLOCKS);
                DeleteCollection(collectionReference);
                var res = await firebase.Collection(TableNames.RESTAURANTS).Document(id).DeleteAsync();
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

        private async void DeleteCollection(CollectionReference collectionReference)
        {
            var snapshot = await collectionReference.GetSnapshotAsync();
            IReadOnlyList<DocumentSnapshot> documents = snapshot.Documents;
            while (documents.Count > 0)
            {
                foreach (DocumentSnapshot document in documents)
                {
                    await document.Reference.DeleteAsync();
                }
                snapshot = await collectionReference.GetSnapshotAsync();
                documents = snapshot.Documents;
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
                if(r != null)
                {
                    r.Id = id;
                }
                return new ApiResponse<Restaurant>
                {
                    Data = r,
                    Success = true
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

        public async Task<ApiResponse<Food>> GetFood(string restaurantId, string id)
        {
            var firebase = _firebaseService.GetFirestoreDb();
            try
            {
                var res = await firebase.Collection(TableNames.RESTAURANTS).Document(restaurantId).Collection(TableNames.FOODS).Document(id).GetSnapshotAsync();
                Food f = res.ConvertTo<Food>();
                if (f != null) 
                { 
                    f.Id = id;
                }
                return new ApiResponse<Food>
                {
                    Data = f,
                    Success = true
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

        private Query getAllRestaurantsReference(string userId, string role)
        {
            var firebase = _firebaseService.GetFirestoreDb();
            if (role.Equals(UserRoles.OWNER))
            {
                return firebase.Collection(TableNames.RESTAURANTS).WhereEqualTo(TableNames.RESTAURANTS_OWNER_ID, userId);
            }
            return firebase.Collection(TableNames.RESTAURANTS);
        }

        public async Task<ApiResponse<List<Restaurant>>> GetAllRestaurant(string userId, string role, RestaurantFilter filter)
        {
            List<Restaurant> list = new List<Restaurant>();
            try
            {
                DocumentSnapshot last = null;
                if (filter.LastId != null && !filter.LastId.Equals(""))
                {
                    last = await _firebaseService.GetFirestoreDb().Collection(TableNames.RESTAURANTS).Document(filter.LastId).GetSnapshotAsync();
                }
                while (true)
                {
                    Query query = getAllRestaurantsReference(userId, role);
                    query = query.OrderBy(TableNames.RESTAURANTS_NAME);
                    if(filter.Name != null && !filter.Name.Equals(""))
                    {
                        query = query.WhereGreaterThanOrEqualTo(TableNames.RESTAURANTS_NAME, filter.Name).WhereLessThanOrEqualTo(TableNames.RESTAURANTS_NAME, filter.Name + Constants.Unicodes.HIGHEST);
                    }
                    if (last != null)
                    {
                        query = query.StartAfter(last);
                    }
                    query = query.Limit(filter.Limit);
                    var res = await query.GetSnapshotAsync();
                    int k = res.Count;
                    if(res.Count == 0)
                    {
                        //boloa
                        break;
                    }
                    foreach (var v in res)
                    {
                        last = v;
                        if (UserRoles.USER.Equals(role))
                        {
                            var blocked = await IsBlocked(userId, v.Id);
                            if (blocked.Data)
                            {
                                continue;
                            }
                        }
                        Restaurant r = v.ConvertTo<Restaurant>();
                        r.Id = v.Id;
                        list.Add(r);
                        if (list.Count == filter.Limit) break;
                    }
                    //boloa
                    if (k < filter.Limit) break;
                    if (UserRoles.OWNER.Equals(role))
                    {
                        break;
                    }
                    if (list.Count == filter.Limit) break;

                }
                return new ApiResponse<List<Restaurant>>
                {
                    Data = list,
                    Success = true
                };
            }
            catch
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
            catch
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
                Id = id,
                Name = req.Name,
                Description = req.Description,
                Price = (double)req.Price
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

        public async Task<ApiResponse<bool>> IsBlocked(string userId, string restaurantID)
        {
            var fireBase = _firebaseService.GetFirestoreDb();
            try
            {
                var blocksRef = fireBase.Collection(TableNames.RESTAURANTS).Document(restaurantID).Collection(TableNames.BLOCKS);
                var res = await blocksRef.WhereEqualTo(TableNames.BLOCKS_USER_ID, userId).GetSnapshotAsync();
                if (res.Count == 0) return new ApiResponse<bool>
                {
                    Success = true,
                    Data = false
                };
                return new ApiResponse<bool>
                {
                    Success = true,
                    Data = true
                };
            } catch
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
        }
    }
}

using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Services
{
    public interface IRestaurantService
    {
        List<Restaurant> GetAllRestaurant();
        Restaurant GetRestaurant(string id);

        Restaurant CreateRestaurant(RestaurantPostRequest res);

        Restaurant UpdateRestaurant(string id, RestaurantPutRequest res);

        bool DeleteRestaurant(string id);

        List<Food> GetAllFood(string restaurantId);
        Food GetFood(string restaurantId, string id);

        Food CreateFood(string restaurantId, FoodPostRequest food);

        Food UpdateFood(string restaurantId, string id, FoodPutRequest food);

        bool DeleteFood(string restaurantId, string id);
    }
}

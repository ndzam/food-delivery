﻿using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using FoodDeliveryWebApi.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Services
{
    public interface IRestaurantService
    {
        Task<ApiResponse<List<Restaurant>>> GetAllRestaurant();
        Task<ApiResponse<Restaurant>> GetRestaurant(string id);

        Task<ApiResponse<Restaurant>> CreateRestaurantAsync(string useId, RestaurantPostRequest res);

        Task<ApiResponse<Restaurant>> UpdateRestaurant(string id, RestaurantPutRequest res);

        Task<ApiResponse> DeleteRestaurantAsync(string id);

        Task<ApiResponse<List<Food>>> GetAllFood(string restaurantId);
        Task<ApiResponse<Food>> GetFood(string restaurantId, string id);

        Task<ApiResponse<Food>> CreateFood(string restaurantId, FoodPostRequest food);

        Task<ApiResponse<Food>> UpdateFood(string restaurantId, string id, FoodPutRequest food);

        Task<ApiResponse> DeleteFood(string restaurantId, string id);
    }
}

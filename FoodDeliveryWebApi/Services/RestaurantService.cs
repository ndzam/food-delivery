using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Services
{
    public class RestaurantService : IRestaurantService
    {
        static List<Restaurant> restaurants;
        static List<Food> foods;

        public RestaurantService()
        {
            if(restaurants == null) 
            {
                restaurants = new List<Restaurant>();
                Restaurant r = new Restaurant
                {
                    Id = "1",
                    Name = "a",
                    Description = "asfsdad"
                };
                restaurants.Add(r);
                r = new Restaurant
                {
                    Id = "2",
                    Name = "b",
                    Description = "gjgf"
                };
                restaurants.Add(r);
                r = new Restaurant
                {
                    Id = "3",
                    Name = "c",
                    Description = "hfdkjds"
                };
                restaurants.Add(r);

            }
            if(foods == null)
            {
                foods = new List<Food>();
                Food f = new Food
                {
                    RestaurantId = "1",
                    Id = "1",
                    Name = "pizza",
                    Description = "fhsdkj",
                    Price = 5.7M
                };
                foods.Add(f);
                f = new Food
                {
                    RestaurantId = "2",
                    Id = "2",
                    Name = "burger",
                    Description = "fsd",
                    Price = 3.2M
                };
                foods.Add(f);
                f = new Food
                {
                    RestaurantId = "2",
                    Id = "3",
                    Name = "salad",
                    Description = "fjdkgf",
                    Price = 1M
                };
                foods.Add(f);
                f = new Food
                {
                    RestaurantId = "3",
                    Id = "4",
                    Name = "gk",
                    Description = "khoy",
                    Price = 7.8M
                };
                foods.Add(f);
            }
        }

        public Restaurant CreateRestaurant(RestaurantPostRequest res)
        {
            Restaurant r = new Restaurant
            {
                Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                Name = res.Name,
                Description = res.Description
            };
            restaurants.Add(r);
            return r;
        }

        public Food CreateFood(string restaurantId, FoodPostRequest food)
        {
            Food f = new Food
            {
                Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                RestaurantId = restaurantId,
                Name = food.Name,
                Description = food.Description,
                Price = food.Price
            };
            foods.Add(f);
            return f;
        }

        public bool DeleteRestaurant(string id)
        {
            return restaurants.RemoveAll(x => x.Id == id) > 0;
        }

        public bool DeleteFood(string restaurantId, string id)
        {
            return foods.RemoveAll(x => x.Id == id && x.RestaurantId==restaurantId) > 0;
        }

        public Restaurant GetRestaurant(string id)
        {
            return restaurants.FirstOrDefault(x => x.Id == id);
        }

        public Food GetFood(string restaurantId, string id)
        {
            return foods.FirstOrDefault(x => x.Id == id && x.RestaurantId == restaurantId);
        }

        public List<Restaurant> GetAllRestaurant()
        {
            return restaurants;
        }

        public List<Food> GetAllFood(string restaurantId)
        {
            return foods.FindAll(x => x.RestaurantId == restaurantId);
        }

        public Restaurant UpdateRestaurant(string id, RestaurantPutRequest res)
        {
            Restaurant v = restaurants.FirstOrDefault(x => x.Id == id);
            v.Name = res.Name;
            v.Description = res.Description;
            return v;
        }

        public Food UpdateFood(string restaurantId, string id, FoodPutRequest food)
        {
            Food f = foods.FirstOrDefault(x => x.RestaurantId == restaurantId && x.Id == id);
            f.Name = food.Name;
            f.Price = food.Price;
            f.Description = food.Description;
            return f;
        }
    }
}

using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using FoodDeliveryWebApi.Response;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Services
{
    public interface IUserService
    {
        Task<ApiResponse<User>> Create(UserPostRequest request);
        Task<ApiResponse<User>> SignIn(TokenRequest request);
        ApiResponse<User> Update(UserPutRequest request);
        ApiResponse<User> Get(string id);
        ApiResponse Delete(string id);
    }
}

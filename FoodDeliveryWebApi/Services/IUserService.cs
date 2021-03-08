using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using FoodDeliveryWebApi.Response;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Services
{
    public interface IUserService
    {
        Task<ApiResponse<TokenDto>> Create(UserPostRequest request);
        Task<ApiResponse<TokenDto>> SignIn(TokenRequest request);
        Task<ApiResponse> Update(string token, string id, UserPutRequest request);
        ApiResponse<User> Get(string id);
        ApiResponse Delete(string id);
    }
}

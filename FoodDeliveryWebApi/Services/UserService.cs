using FirebaseAdmin.Auth;
using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using FoodDeliveryWebApi.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FoodDeliveryWebApi.Configs;
using Microsoft.Extensions.Options;

namespace FoodDeliveryWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly string _apiKey;
        public UserService(IOptions<APIConfigs> options)
        {
            _apiKey = options.Value.ApiKey;
        }

        public async Task<ApiResponse<User>> Create(UserPostRequest request)
        {
            UserRecordArgs userRecordArgs = new UserRecordArgs
            {
                Email = request.Email,
                EmailVerified = true,
                Password = request.Password,
                DisplayName = $"{request.Firstname} {request.Lastname}",
                Disabled = false,
            };
            try
            {
                UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userRecordArgs);

                string customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(userRecord.Uid);

                return new ApiResponse<User>
                {
                    Success = true,
                    Data = new User
                    {
                        Id = userRecord.Uid,
                        Name = userRecord.DisplayName,
                        Token = customToken
                    }
                };
            }
            catch (FirebaseAuthException ex)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    ErrorCode = ex.AuthErrorCode.HasValue ? ex.AuthErrorCode.Value.ToString() : ex.ErrorCode.ToString()
                };
            }
            catch (System.ArgumentException ex)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    ErrorCode = ex.Message.ToString()
                };
            }
        }

        public async Task<ApiResponse<User>> SignIn(TokenRequest request)
        {
            try
            {
                HttpClient client = new HttpClient();
                VerifyPasswordResponse verifyPasswordResponse = null;
                try
                {
                    var content = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    });
                    var payload = new StringContent(content, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + _apiKey, payload);
                    string responseJson = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();
                    verifyPasswordResponse = JsonConvert.DeserializeObject<VerifyPasswordResponse>(responseJson);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                if (verifyPasswordResponse == null)
                {
                    return new ApiResponse<User>
                    {
                        Success = false,
                        ErrorCode = "CAN_NOT_SIGN_IN"
                    };
                }

                return new ApiResponse<User>
                {
                    Success = true,
                    Data = new User
                    {
                        Id = verifyPasswordResponse.LocalId,
                        Name = verifyPasswordResponse.Email,
                        Token = verifyPasswordResponse.IdToken
                    }
                };
            }
            catch (FirebaseAuthException ex)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    ErrorCode = ex.AuthErrorCode.HasValue ? ex.AuthErrorCode.Value.ToString() : ex.ErrorCode.ToString()
                };
            }
            catch (System.ArgumentException ex)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    ErrorCode = ex.Message.ToString()
                };
            }
        }

        public ApiResponse Delete(string id)
        {
            throw new System.NotImplementedException();
        }

        public ApiResponse<User> Get(string id)
        {
            throw new System.NotImplementedException();
        }

        public ApiResponse<User> Update(UserPutRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}

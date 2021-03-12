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
using FoodDeliveryWebApi.Constants;
using Firebase.Auth;
using Firebase.Database.Query;
using System.Linq;
using System.Collections.Generic;
using FirebaseAdmin.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FoodDeliveryWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IFirebaseService _firebaseService;
        private readonly string _apiKey;
        public UserService(IOptions<APIConfigs> options, IFirebaseService firebaseService)
        {
            _apiKey = options.Value.ApiKey;
            _firebaseService = firebaseService;
        }

        public async Task<ApiResponse<TokenDto>> Create(UserPostRequest request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return new ApiResponse<TokenDto>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.PASSWORDS_DONT_MATCH
                };
            }
            if(request.Name == null || request.Name.Length < 3)
            {
                return new ApiResponse<TokenDto>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INVALID_NAME
                };
            }
            if(request.Role == null) request.Role = UserRoles.USER;
            else request.Role  = request.Role.ToLowerInvariant();

            if(request.Role!= UserRoles.OWNER && request.Role != UserRoles.USER)
            {
                request.Role = UserRoles.USER;
            }
            
            try
            {
                var auth = await _firebaseService.GetFirebaseAuthProvider().CreateUserWithEmailAndPasswordAsync(request.Email, request.Password, request.Name, false);
                
                var additionalClaims = new Dictionary<string, object>()
                {
                    { Claims.ROLE , request.Role},
                };
                await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(auth.User.LocalId, additionalClaims);
                //var firebase = _firebaseService.GetFirebaseClient(auth.FirebaseToken);
                //var profile = await firebase.Child(TableNames.PROFILE).PostAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new Profile{
                //    UserId = auth.User.LocalId,
                //    Role = request.Role
                //}));
                return new ApiResponse<TokenDto>
                {
                    Success = true,
                    Data = new TokenDto
                    {
                        Token = auth.FirebaseToken,
                        RefreshToken = auth.RefreshToken,
                        Name = auth.User.DisplayName,
                        Email = auth.User.Email,
                        UserId = auth.User.LocalId,
                        ExpiresIn = auth.ExpiresIn,
                        CreatedAt = auth.Created,
                        Role = request.Role
                    }
                };
            }catch (Firebase.Auth.FirebaseAuthException ex)
            {
                return new ApiResponse<TokenDto>
                {
                    Success = false,
                    ErrorCode = _firebaseService.ConvertErrorCode(ex.Reason)
                };
            }catch
            {
                return new ApiResponse<TokenDto>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
        }


        public ApiResponse Delete(string id)
        {
            throw new System.NotImplementedException();
        }

        public ApiResponse<Models.User> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<TokenDto>> SignIn(TokenRequest request)
        {
            try
            {
                var auth = await _firebaseService.GetFirebaseAuthProvider().SignInWithEmailAndPasswordAsync(request.Email, request.Password);
                var tok = auth.FirebaseToken;
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tok);
                var v = token.Claims.FirstOrDefault(x => x.Type.Equals(Claims.ROLE));
                string role = null;
                if (v != null)
                {
                    role = v.Value;
                }
                return new ApiResponse<TokenDto>
                {
                    Success = true,
                    Data = new TokenDto
                    {
                        Token = auth.FirebaseToken,
                        RefreshToken = auth.RefreshToken,
                        Name = auth.User.DisplayName,
                        Email = auth.User.Email,
                        UserId = auth.User.LocalId,
                        ExpiresIn = auth.ExpiresIn,
                        CreatedAt = auth.Created,
                        Role = role
                    }
                };
            }
            catch (Firebase.Auth.FirebaseAuthException ex)
            {
                return new ApiResponse<TokenDto>
                {
                    Success = false,
                    ErrorCode = _firebaseService.ConvertErrorCode(ex.Reason)
                };
            }
        }

        public async Task<ApiResponse> Update(string token, string id, UserPutRequest request)
        {
            //if(request.Role == null)
            //{
            //    return new ApiResponse
            //    {
            //        Success = false,
            //        ErrorCode = ErrorCodes.INVALID_ROLE
            //    };
            //}
            //request.Role = request.Role.ToLowerInvariant();
            //if(request.Role != UserRoles.USER && request.Role != UserRoles.ADMIN && request.Role != UserRoles.OWNER)
            //{
            //    return new ApiResponse
            //    {
            //        Success = false,
            //        ErrorCode = ErrorCodes.INVALID_ROLE
            //    };
            //}
            //try
            //{
            //    var auth = await _firebaseService.GetFirebaseAuthProvider().GetUserAsync(token);
            //    var firebase = _firebaseService.GetFirebaseClient(token);

            //    var queryResult = await firebase
            //        .Child(TableNames.PROFILE)
            //        .OrderBy("UserId")
            //        .StartAt(auth.LocalId)
            //        .LimitToFirst(1)
            //        .OnceAsync<Profile>();

            //    if(queryResult == null || queryResult.Count == 0)
            //    {
            //        return new ApiResponse
            //        {
            //            Success = false,
            //            ErrorCode = ErrorCodes.FORBIDEN
            //        };
            //    }

            //    var profile = queryResult.ToList()[0].Object;
            //    if(profile.Role != UserRoles.ADMIN)
            //    {
            //        return new ApiResponse
            //        {
            //            Success = false,
            //            ErrorCode = ErrorCodes.FORBIDEN
            //        };
            //    }

            //    var queryResult2 = await firebase
            //        .Child(TableNames.PROFILE)
            //        .OrderBy("UserId")
            //        .StartAt(id)
            //        .LimitToFirst(1)
            //        .OnceAsync<Profile>();

            //    if(queryResult2 == null || queryResult2.Count == 0)
            //    {
            //        return new ApiResponse
            //        {
            //            Success = false,
            //            ErrorCode = ErrorCodes.NOT_FOUND
            //        };
            //    }
            //    var queryResultObject = queryResult2.ToList()[0];
            //    var target = queryResultObject.Object;

            //    await firebase
            //    .Child(TableNames.PROFILE)
            //    .Child(queryResultObject.Key)
            //    .PatchAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new Profile{
            //        UserId = id,
            //        Role = request.Role
            //    }));
            //}
            //catch (Firebase.Auth.FirebaseAuthException ex)
            //{
            //    return new ApiResponse
            //    {
            //        Success = false,
            //        ErrorCode = _firebaseService.ConvertErrorCode(ex.Reason)
            //    };
            //}catch  {
            //    return new ApiResponse
            //    {
            //        Success = false,
            //        ErrorCode = ErrorCodes.UNKNOWN_ERROR
            //    };
            //}

            //return new ApiResponse
            //{
            //    Success = true,
            //};
            return null;
        }
    }
}

using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using FoodDeliveryWebApi.Response;
using System;
using System.Threading.Tasks;
using FoodDeliveryWebApi.Configs;
using Microsoft.Extensions.Options;
using FoodDeliveryWebApi.Constants;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using FirebaseAdmin.Auth;

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
                await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(auth.User.LocalId, additionalClaims);
                auth = await _firebaseService.GetFirebaseAuthProvider().SignInWithEmailAndPasswordAsync(request.Email, request.Password);

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

        public async Task<ApiResponse<User>> Get(string id)
        {
            try {
                var res = await FirebaseAuth.DefaultInstance.GetUserAsync(id);
                return new ApiResponse<User>
                {
                    Success = true,
                    Data = new User
                    {
                        Name = res.DisplayName,
                        Email = res.Email
                    }
                };
            } catch(FirebaseAuthException e)
            {
                if(e.ErrorCode.Equals(FirebaseAdmin.ErrorCode.NotFound))
                {
                    return new ApiResponse<User>
                    {
                        Success = true,
                        Data = null
                    };
                }
                return new ApiResponse<User>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            } catch(Exception e)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.UNKNOWN_ERROR
                };
            }
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
    }
}

using FoodDeliveryWebApi.Constants;
using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using FoodDeliveryWebApi.Response;
using FoodDeliveryWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : FoodDeliveryBaseController
    {

        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UserPostRequest request)
        {
            if(request.Name == null || request.Email==null || request.ConfirmPassword==null || request.Password == null || request.Role==null)
            {
                return BadRequest(new ApiResponse
                {
                    ErrorCode = ErrorCodes.MISSING_FIELD,
                    Success = false
                });
            }
            ApiResponse<TokenDto> response = await _userService.Create(request);
            if (response.Success)
            {
                return Ok(response);
            }
            if (response.ErrorCode == ErrorCodes.ACCOUNT_EXISTS)
            {
                return Conflict(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [HttpPost("token")]
        public async Task<IActionResult> SignIn([FromBody] TokenRequest request)
        {
            if(request.Password == null || request.Email == null)
            {
                return BadRequest(new ApiResponse
                {
                    ErrorCode = ErrorCodes.MISSING_FIELD,
                    Success = false
                });
            }
            ApiResponse<TokenDto> response = await _userService.SignIn(request);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if(!Role.Equals(UserRoles.OWNER) && !UserId.Equals(id))
            {
                return Forbid();
            }
            ApiResponse<User> response = await _userService.Get(id);
            if (!response.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if(response.Data == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

    }
}

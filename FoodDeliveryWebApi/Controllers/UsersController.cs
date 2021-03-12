using FoodDeliveryWebApi.Constants;
using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using FoodDeliveryWebApi.Response;
using FoodDeliveryWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //TODO
        private IUserService _userService;

        public UsersController(IUserService userService, IOrderService orderService, IRestaurantService restaurantService)
        {
            _userService = userService;
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UserPostRequest request)
        {
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
        [HttpPost("token")]
        public async Task<IActionResult> SignIn([FromBody] TokenRequest request)
        {
            ApiResponse<TokenDto> response = await _userService.SignIn(request);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        // [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        // [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        // [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        // [HttpGet("{id}")]
        // public IActionResult Get(string id)
        // {
        //     //abrunebs users id-it
            
        //     return null;
        // }
        
    }
}

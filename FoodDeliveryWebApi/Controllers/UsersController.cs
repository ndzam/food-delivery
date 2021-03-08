using FoodDeliveryWebApi.Constants;
using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using FoodDeliveryWebApi.Response;
using FoodDeliveryWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private IOrderService _orderService;
        private IUserService _userService;
        private IRestaurantService _restaurantService;

        public UsersController(IUserService userService, IOrderService orderService, IRestaurantService restaurantService)
        {
            _userService = userService;
            _orderService = orderService;
            _restaurantService = restaurantService;
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
        
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UserPutRequest req)
        {
            if (Request.Headers.ContainsKey("Authorization"))
            {
                var token = Request.Headers["Authorization"];
                ApiResponse response = await _userService.Update(token, id, req);
                if (response.Success)
                {
                    return Ok(response);
                }
                if(response.ErrorCode == ErrorCodes.FORBIDEN)
                {
                    return Forbid();
                }
                if(response.ErrorCode == ErrorCodes.INVALID_TOKEN)
                {
                    return Unauthorized();
                }
                if(response.ErrorCode == ErrorCodes.NOT_FOUND)
                {
                    return NotFound(response);
                }
                return BadRequest(response);
            }
            return Unauthorized();
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


        // [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        // [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        // [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        // [HttpGet("{id}/orders")]
        // public IActionResult GetOrders(string id)
        // {
        //     //xo ar jobia gaiyos? /owners/[id]/orders da /users/[id]/orders
        //     //tu owneria, ubrunebs yvela tavis restoranshi gaketebul orders, tu useria ubrunebs tavis gaketebul orderebs
        //     var res = _orderService.GetOrders(id);
        //     return Ok(res);
        // }

        // [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        // [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        // [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        // [HttpGet("{id}/orders/{orderId}")]
        // public IActionResult GetOrder(string id, string orderId)
        // {
        //     //tu owneria da tavis restoranshi gaketebuli shekveta araa ar unda abrunebdes
        //     //tu useria da tavisi gaketebuli shekveta araa ar unda abrunebdes
        //     var res = _orderService.GetOrder(orderId);
        //     if(res == null)
        //     {
        //         return NotFound();
        //     }
        //     return Ok(res);
        // }

        // [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        // [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        // [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        // [HttpPut("{id}/orders/{orderId}")]
        // public IActionResult PutOrder(string id, string orderId, [FromBody]OrderPutRequest req)
        // {
        //     //unda ecvlebodes mxolod statusi shemdegi principit da mxolod mflobelisgan
        //  /*   public enum Status
        // {
        //     Placed,
        //     Canceled,
        //     Processing,
        //     Route,
        //     Delivered,
        //     Received
        // }*/
        //     /*var res = _.GetRestaurant(id);
        //     if (res == null)
        //     {
        //         return NotFound();
        //     }
        //     return Ok(res);*/
        //     return null;
        // }

         [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
         [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
         [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [HttpPost("{id}/orders")]
         public async Task<IActionResult> PostOrder(string id, [FromBody] OrderPostRequest req)
         {
            //yvela sachmeli unda iyos erti restornidan.
            //aketebs mxolod user aradablokil restoranshi
            var userId = "";
            var role = UserRoles.ADMIN;
            //admin unda iyos? shesamowmebelia dablokiloba.
            if(role.Equals(UserRoles.ADMIN) || (userId.Equals(id) && role.Equals(UserRoles.USER)))
            {
                var res = await _restaurantService.GetRestaurant(req.RestaurantId);
                if(res.Success && res.Data == null)
                {
                    return NotFound();
                } else if(!res.Success)
                {
                    //unknown error
                    return NotFound();
                } else
                {
                    //todo shevamowmo carielze
                    var foodsResponse = await _restaurantService.GetAllFood(req.RestaurantId);
                    var foods = foodsResponse.Data;
                    Decimal total = 0;
                    foreach (OrderItem o in req.Items)
                    {
                        if(o.Quantity <= 0)
                        {
                            return BadRequest();
                        }
                        Food f = foods.FirstOrDefault(x => x.Id == o.FoodId);
                        if (f == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            total += f.Price * o.Quantity;
                        }
                    }
                    var result = await _orderService.CreateOrder(id, req, total);
                    //todo shecdoma?
                    return Ok(result);
                }
            } else
            {
                return Forbid();
            }
         }
    }
}

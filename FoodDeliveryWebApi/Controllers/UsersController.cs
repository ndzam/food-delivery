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

        public UsersController(IUserService userService, IOrderService orderService)
        {
            _userService = userService;
            _orderService = orderService;
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UserPostRequest request)
        {
            ApiResponse<User> response = await _userService.Create(request);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [HttpPost("token")]
        public async Task<IActionResult> SignIn([FromBody] TokenRequest request)
        {
            ApiResponse<User> response = await _userService.SignIn(request);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            //abrunebs users id-it
            
            return null;
        }


        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}/orders")]
        public IActionResult GetOrders(string id)
        {
            //xo ar jobia gaiyos? /owners/[id]/orders da /users/[id]/orders
            //tu owneria, ubrunebs yvela tavis restoranshi gaketebul orders, tu useria ubrunebs tavis gaketebul orderebs
            var res = _orderService.GetOrders(id);
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}/orders/{orderId}")]
        public IActionResult GetOrder(string id, string orderId)
        {
            //tu owneria da tavis restoranshi gaketebuli shekveta araa ar unda abrunebdes
            //tu useria da tavisi gaketebuli shekveta araa ar unda abrunebdes
            var res = _orderService.GetOrder(orderId);
            if(res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}/orders/{orderId}")]
        public IActionResult PutOrder(string id, string orderId, [FromBody]OrderPutRequest req)
        {
            //unda ecvlebodes mxolod statusi shemdegi principit da mxolod mflobelisgan
         /*   public enum Status
        {
            Placed,
            Canceled,
            Processing,
            Route,
            Delivered,
            Received
        }*/
            /*var res = _.GetRestaurant(id);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);*/
            return null;
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] UserPutRequest req)
        {
            //es mgoni araa sachiro
            /*var res = _restaurantService.GetRestaurant(id);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);*/
            return null;
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpPost("{id}/orders")]
        public IActionResult PostOrder(string id, [FromBody] OrderPostRequest req)
        {
            //yvela sachmeli unda iyos erti restornidan.
            //aketebs mxolod user aradablokil restoranshi
            /*var res = _restaurantService.GetRestaurant(id);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);*/
            return null;
        }
    }
}

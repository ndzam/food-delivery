using FoodDeliveryWebApi.Constants;
using FoodDeliveryWebApi.Requests;
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
    public class RestaurantsController : ControllerBase
    {
        private IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            //tu owneria unda daubrunos sxvisi restorani?
            //tu dablokilia ar unda daubrunos
            //unknown error?
            var res = await _restaurantService.GetRestaurant(id);
            if(res.Success && res.Data == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //tu owneria unda wamoigos mxolod tavisi restornebi
            //userma aradablokili
            
            var res = await _restaurantService.GetAllRestaurant();
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]RestaurantPostRequest req)
        {
            //qmnis mxolod owneri
            string userId = "J7nV5vBerlb4NCMY67kUi7cSveN2";

            var res = await _restaurantService.CreateRestaurantAsync(userId, req);
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]RestaurantPutRequest req)
        {
            //tu adminia?
            var userId = "";
            var role = "";
            var restaurant = await _restaurantService.GetRestaurant(id);
            if(restaurant.Success && restaurant.Data == null)
            {
                return NotFound();
            } else if(!role.Equals(UserRoles.ADMIN) && restaurant.Success && !restaurant.Data.UserId.Equals(userId))
            {
                return Forbid();
            } else if (!restaurant.Success)
            {
                //return unknown error.
            }
            var res = await _restaurantService.UpdateRestaurant(id, req);
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var res = await _restaurantService.DeleteRestaurantAsync(id);
            if (!res.Success)
            {
                return NotFound();
            }
            //unda gaaketos mxolod ownerma, romlisi restoranicaa
            return Ok();
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}/foods")]
        public async Task<IActionResult> GetFoods(string id)
        {
            //tu user aris unda shemowmdes dablokili xo araa
            var res = await _restaurantService.GetAllFood(id);
            if (res.Success && res.Data == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}/foods/{foodId}")]
        public async Task<IActionResult> GetFood(string id, string foodId)
        {
            //user tu dablokilia ar unda wamoigos
            //owner tu tavisi araa ar unda wamoigos
            var res = await _restaurantService.GetFood(id, foodId);
            if (res.Success && res.Data == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpPost("{id}/foods")]
        public async Task<IActionResult> PostFood(string id, [FromBody] FoodPostRequest req)
        {
            //user tu dablokilia ar unda wamoigos
            //owner tu tavisi araa ar unda wamoigos
            var res = _restaurantService.GetRestaurant(id);
            if(res == null)
            {
                return NotFound();
            }
            var food = await _restaurantService.CreateFood(id, req);
            return Ok(food);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}/foods/{foodId}")]
        public async Task<IActionResult> PutFood(string id, string foodId, [FromBody] FoodPutRequest req)
        {
            /*//unda sheedzlos mxolod owners tavis restoranze
            if (_restaurantService.GetFood(id, foodId) == null)
            {
                return NotFound();
            }*/
            var res = await _restaurantService.UpdateFood(id, foodId, req);
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}/foods/{foodId}")]
        public async Task<IActionResult> DeleteFood(string id, string foodId)
        {
            //unda sheedzllos mxolod owners tavis restoranze
            var res = await _restaurantService.DeleteFood(id, foodId);
            if (!res.Success)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}

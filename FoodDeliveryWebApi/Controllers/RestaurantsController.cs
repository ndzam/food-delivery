using FoodDeliveryWebApi.Constants;
using FoodDeliveryWebApi.Filters;
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
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            //amovigo tokenidan
            var userRole = UserRoles.USER;
            var userId = "";
            var res = await _restaurantService.GetRestaurant(id);
            if(res.Success && res.Data == null)
            {
                return NotFound();
            }
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (userRole.Equals(UserRoles.OWNER))
            {
                if (!res.Data.OwnerId.Equals(userId))
                {
                    //no permission
                    //return Forbid();
                }
            } else
            {
                var v = await _restaurantService.IsBlocked(userId, id);
                if (v.Success && v.Data)
                {
                    //blocked
                    return Forbid();
                }
                if (!v.Success)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] RestaurantFilter filter)
        {
            //description filtri?
            if(filter.Limit > FilterConstants.MAX_LIMIT || filter.Limit < 1)
            {
                //es unda iyos?
                return BadRequest();
            }
            //tu filtris id ar aris...
            if (filter.LastId != null && !filter.LastId.Equals(""))
            {
                var r = await _restaurantService.GetRestaurant(filter.LastId);
                if (!r.Success)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                if(r.Data == null)
                {
                    //es unda?
                    return NotFound();
                }
            }
            var userId = "J7nV5vBerlb4NCMY67kUi7cSveN2";
            var userRole = UserRoles.USER;
            var res = await _restaurantService.GetAllRestaurant(userId, userRole, filter);
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]RestaurantPostRequest req)
        {
            //amovigo tokenidan
            var role = UserRoles.OWNER;
            string userId = "J7nV5vBerlb4NCMY67kUi7cSveN2";
            if (!role.Equals(UserRoles.OWNER))
            {
                return Forbid();
            }
            var res = await _restaurantService.CreateRestaurantAsync(userId, req);
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            //code 201?
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]RestaurantPutRequest req)
        {
            //amovigo tokenidan
            var userId = "";
            var role = UserRoles.OWNER;
            var restaurant = await _restaurantService.GetRestaurant(id);
            if(restaurant.Success && restaurant.Data == null)
            {
                return NotFound();
            }
            if (!restaurant.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            //movashoro
            else if(!restaurant.Data.OwnerId.Equals(userId) && false)
            {
                return Forbid();
            }
            restaurant.Data.Name = req.Name;
            restaurant.Data.Description = req.Description;
            var res = await _restaurantService.UpdateRestaurant(id, restaurant.Data);
            if (!res.Success)
            {
                new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            //200 unda iyos?
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            //amovigo tokenidan
            var userId = "";
            var res = await _restaurantService.GetRestaurant(id);
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if(res.Data == null)
            {
                return NotFound();
            }
            //movashoro
            if (!userId.Equals(res.Data.OwnerId) && false)
            {
                return Forbid();
            }
            var v = await _restaurantService.DeleteRestaurantAsync(id);
            if (v.Success)
            {
                //204
                return Ok();
            }
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}/foods")]
        public async Task<IActionResult> GetFoods(string id)
        {
            //amovigo tokenidan
            var userRole = UserRoles.USER;
            var userId = "";
            var res = await _restaurantService.GetRestaurant(id);
            if (res.Success && res.Data == null)
            {
                return NotFound();
            }
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (userRole.Equals(UserRoles.OWNER))
            {
                if (!res.Data.OwnerId.Equals(userId))
                {
                    //no permission
                    //return Forbid();
                }
            }
            else
            {
                var v = await _restaurantService.IsBlocked(userId, id);
                if (v.Success && v.Data)
                {
                    return Forbid();
                }
                if (!v.Success)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            var result = await _restaurantService.GetAllFood(id);
            if (!result.Success)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}/foods/{foodId}")]
        public async Task<IActionResult> GetFood(string id, string foodId)
        {
            //amovigo tokenidan
            var userRole = UserRoles.USER;
            var userId = "";
            var res = await _restaurantService.GetRestaurant(id);
            if (res.Success && res.Data == null)
            {
                return NotFound();
            }
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (userRole.Equals(UserRoles.OWNER))
            {
                if (!res.Data.OwnerId.Equals(userId))
                {
                    return Forbid();
                }
            }
            else
            {
                var v = await _restaurantService.IsBlocked(userId, id);
                if (v.Success && v.Data)
                {
                    return Forbid();
                }
                if (!v.Success)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            var result = await _restaurantService.GetFood(id, foodId);
            if (!result.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if(res.Data == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [HttpPost("{id}/foods")]
        public async Task<IActionResult> PostFood(string id, [FromBody] FoodPostRequest req)
        {
            //amovigo tokenidan
            var userRole = UserRoles.OWNER;
            var userId = "";
            if (req.Price < 0)
            {
                return BadRequest();
            }
            if (!userRole.Equals(UserRoles.OWNER))
            {
                return Forbid();
            }
            var res = await _restaurantService.GetRestaurant(id);
            if (res.Success && res.Data == null)
            {
                return NotFound();
            }
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (!res.Data.OwnerId.Equals(userId))
            {
                //no permission
                //return Forbid();
            }
            var result = await _restaurantService.CreateFood(id, req);
            if (!result.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            //201?
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}/foods/{foodId}")]
        public async Task<IActionResult> PutFood(string id, string foodId, [FromBody] FoodPutRequest req)
        {
            //amovigo tokenidan
            var userId = "";
            var role = "";
            var food = await _restaurantService.GetFood(id, foodId);
            if (req.Price < 0)
            {
                return BadRequest();
            }
            if (food.Success && food.Data == null)
            {
                return NotFound();
            }
            else if (!food.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            var restaurant = await _restaurantService.GetRestaurant(id);
            if (!restaurant.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (!restaurant.Data.OwnerId.Equals(userId))
            {
                //no permission
                //return Forbid();
            }
            var res = await _restaurantService.UpdateFood(id, foodId, req);
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            //code?
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}/foods/{foodId}")]
        public async Task<IActionResult> DeleteFood(string id, string foodId)
        {
            //amovigo tokenidan
            var userRole = UserRoles.OWNER;
            var userId = "";
            if (!userRole.Equals(UserRoles.OWNER))
            {
                return Forbid();
            }
            var res = await _restaurantService.GetRestaurant(id);
            if (res.Success && res.Data == null)
            {
                return NotFound();
            }
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (!res.Data.OwnerId.Equals(userId))
            {
                //no permission
                //return Forbid();
            }
            var result = await _restaurantService.DeleteFood(id, foodId);
            if (!result.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [HttpPost("{id}/blocks")]
        public async Task<IActionResult> PostBlock(string id, [FromBody] BlockPostRequest req)
        {
            //ase unda iyos? ra unda davubruno? get block davamato?
            //tu ukve dablokilia?
            var userId = "";
            var role = UserRoles.OWNER;
            if (!role.Equals(UserRoles.OWNER))
            {
                return Forbid();
            }
            var res = await _restaurantService.GetRestaurant(id);
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (res.Data == null)
            {
                return NotFound();
            }
            if (!res.Data.OwnerId.Equals(userId))
            {
                //return Forbid();
            }
            //useris shemowmeba ??
            var block = await _restaurantService.CreateBlock(id, req);
            if (!block.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return Ok(block);
        }
    }
}

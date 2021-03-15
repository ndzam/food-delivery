using FoodDeliveryWebApi.Constants;
using FoodDeliveryWebApi.Filters;
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
    public class RestaurantsController : FoodDeliveryBaseController
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
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var res = await _restaurantService.GetRestaurant(id);
            if(res.Success && res.Data == null)
            {
                return NotFound();
            }
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (Role.Equals(UserRoles.OWNER))
            {
                if (!res.Data.OwnerId.Equals(UserId))
                {
                    return Forbid();
                }
            } else
            {
                var v = await _restaurantService.IsBlocked(UserId, id);
                if (v.Success && v.Data)
                {
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
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] RestaurantFilter filter)
        {
            if(filter.Limit > FilterConstants.MAX_LIMIT || filter.Limit < 1)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INVALID_LIMIT
                });
            }
            if (filter.LastId != null && !filter.LastId.Equals(""))
            {
                var r = await _restaurantService.GetRestaurant(filter.LastId);
                if (!r.Success)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                if(r.Data == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.INVALID_LAST_ID
                    });
                }
            }
            var res = await _restaurantService.GetAllRestaurant(UserId, Role, filter);
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]RestaurantPostRequest req)
        {
            if (req.Name == null || req.Description == null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_FIELD
                });
            }
            if(req.Name.Length == 0 || req.Name.Length > FilterConstants.NAME_MAX_LENGTH)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.RESTAURANT_INVALID_NAME_LENGTH
                }) ;
            }
            if(req.Description.Length == 0 || req.Description.Length > FilterConstants.DESCRIPTION_MAX_LENGTH)
            {
                BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.RESTAURANT_INVALID_DESCRIPTION_LENGTH
                }); ;
            }
            if (!Role.Equals(UserRoles.OWNER))
            {
                return Forbid();
            }
            var res = await _restaurantService.CreateRestaurantAsync(UserId, req);
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtAction(nameof(Get), new { id = res.Data.Id }, res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]RestaurantPutRequest req)
        {
            if(req.Name == null || req.Description == null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_FIELD
                });
            }
            if (req.Name.Length == 0 || req.Name.Length > FilterConstants.NAME_MAX_LENGTH)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.RESTAURANT_INVALID_NAME_LENGTH
                });
            }
            if (req.Description.Length == 0 || req.Description.Length > FilterConstants.DESCRIPTION_MAX_LENGTH)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.RESTAURANT_INVALID_DESCRIPTION_LENGTH
                });
            }
            var restaurant = await _restaurantService.GetRestaurant(id);
            if(restaurant.Success && restaurant.Data == null)
            {
                return NotFound();
            }
            if (!restaurant.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            else if(!restaurant.Data.OwnerId.Equals(UserId))
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
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var res = await _restaurantService.GetRestaurant(id);
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if(res.Data == null)
            {
                return NotFound();
            }
            if (!UserId.Equals(res.Data.OwnerId) && false)
            {
                return Forbid();
            }
            var v = await _restaurantService.DeleteRestaurantAsync(id);
            if (v.Success)
            {
                return NoContent();
            }
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpGet("{id}/foods")]
        public async Task<IActionResult> GetFoods(string id)
        {
            var res = await _restaurantService.GetRestaurant(id);
            if (res.Success && res.Data == null)
            {
                return NotFound();
            }
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (Role.Equals(UserRoles.OWNER))
            {
                if (!res.Data.OwnerId.Equals(UserId))
                {
                    return Forbid();
                }
            }
            else
            {
                var v = await _restaurantService.IsBlocked(UserId, id);
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
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return Ok(result);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpGet("{id}/foods/{foodId}")]
        public async Task<IActionResult> GetFood(string id, string foodId)
        {
            var res = await _restaurantService.GetRestaurant(id);
            if (res.Success && res.Data == null)
            {
                return NotFound();
            }
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (Role.Equals(UserRoles.OWNER))
            {
                if (!res.Data.OwnerId.Equals(UserId))
                {
                    return Forbid();
                }
            }
            else
            {
                var v = await _restaurantService.IsBlocked(UserId, id);
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
        [Authorize]
        [HttpPost("{id}/foods")]
        public async Task<IActionResult> PostFood(string id, [FromBody] FoodPostRequest req)
        {
            if(req.Price == null || req.Description == null || req.Name == null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_FIELD
                });
            }
            if(req.Price <= 0)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.FOOD_INVALID_PRICE
                });
            }
            if(req.Name.Length == 0 || req.Name.Length > FilterConstants.NAME_MAX_LENGTH)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.FOOD_INVALID_NAME_LENGTH
                });
            }
            if (req.Description.Length == 0 || req.Description.Length > FilterConstants.DESCRIPTION_MAX_LENGTH)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.FOOD_INVALID_DESCRIPTION_LENGTH
                });
            }
            if (!Role.Equals(UserRoles.OWNER))
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
            if (!res.Data.OwnerId.Equals(UserId))
            {
                return Forbid();
            }
            var result = await _restaurantService.CreateFood(id, req);
            if (!result.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtAction(nameof(GetFood), new { id = id, foodId = result.Data.Id }, result);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpPut("{id}/foods/{foodId}")]
        public async Task<IActionResult> PutFood(string id, string foodId, [FromBody] FoodPutRequest req)
        {
            if (req.Price == null || req.Description == null || req.Name == null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_FIELD
                });
            }
            if (req.Price <= 0)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.FOOD_INVALID_PRICE
                });
            }
            if (req.Name.Length == 0 || req.Name.Length > FilterConstants.NAME_MAX_LENGTH)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.FOOD_INVALID_NAME_LENGTH
                });
            }
            if (req.Description.Length == 0 || req.Description.Length > FilterConstants.DESCRIPTION_MAX_LENGTH)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.FOOD_INVALID_DESCRIPTION_LENGTH
                });
            }
            var food = await _restaurantService.GetFood(id, foodId);
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
            if (!restaurant.Data.OwnerId.Equals(UserId))
            {
                return Forbid();
            }
            var res = await _restaurantService.UpdateFood(id, foodId, req);
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpDelete("{id}/foods/{foodId}")]
        public async Task<IActionResult> DeleteFood(string id, string foodId)
        {
            if (!Role.Equals(UserRoles.OWNER))
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
            if (!res.Data.OwnerId.Equals(UserId))
            {
                return Forbid();
            }
            var result = await _restaurantService.DeleteFood(id, foodId);
            if (!result.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpPut("{id}/blocks")]
        public async Task<IActionResult> PostBlock(string id, [FromBody] BlockPostRequest req)
        {
            if(req.UserId == null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_FIELD
                });
            }
            //ase unda iyos? ra unda davubruno? get block davamato?
            //tu ukve dablokilia?
            if (!Role.Equals(UserRoles.OWNER))
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
            if (!res.Data.OwnerId.Equals(UserId))
            {
                return Forbid();
            }
            //useris shemowmeba ??
            var block = await _restaurantService.CreateBlock(id, req);
            if (!block.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
    }
}

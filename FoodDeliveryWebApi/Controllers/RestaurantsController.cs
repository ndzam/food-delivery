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
        public IActionResult Get(string id)
        {
            //tu owneria unda daubrunos sxvisi restorani?
            //tu dablokilia ar unda daubrunos
            var res = _restaurantService.GetRestaurant(id);
            if(res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public IActionResult Get()
        {
            //tu owneria unda wamoigos mxolod tavisi restornebi
            //userma aradablokili
            var res = _restaurantService.GetAllRestaurant();
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public IActionResult Post([FromBody]RestaurantPostRequest req)
        {
            //qmnis mxolod owneri
            var res = _restaurantService.CreateRestaurant(req);
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]RestaurantPutRequest req)
        {
            if(_restaurantService.GetRestaurant(id) == null)
            {
                return NotFound();
            }
            //unda gaaketos mxolod ownerma, romlisi restoranicaa
            var res = _restaurantService.UpdateRestaurant(id, req);
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            bool res = _restaurantService.DeleteRestaurant(id);
            if (!res)
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
        public IActionResult GetFoods(string id)
        {
            //tu user aris unda shemowmdes dablokili xo araa
            var res = _restaurantService.GetAllFood(id);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}/foods/{foodId}")]
        public IActionResult GetFood(string id, string foodId)
        {
            //user tu dablokilia ar unda wamoigos
            //owner tu tavisi araa ar unda wamoigos
            var res = _restaurantService.GetFood(id, foodId);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpPost("{id}/foods")]
        public IActionResult PostFood(string id, [FromBody] FoodPostRequest req)
        {
            //user tu dablokilia ar unda wamoigos
            //owner tu tavisi araa ar unda wamoigos
            var res = _restaurantService.GetRestaurant(id);
            if(res == null)
            {
                return NotFound();
            }
            var food = _restaurantService.CreateFood(id, req);
            return Ok(food);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}/foods/{foodId}")]
        public IActionResult PutFood(string id, string foodId, [FromBody] FoodPutRequest req)
        {
            //unda sheedzlos mxolod owners tavis restoranze
            if (_restaurantService.GetFood(id, foodId) == null)
            {
                return NotFound();
            }
            var res = _restaurantService.UpdateFood(id, foodId, req);
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}/foods/{foodId}")]
        public IActionResult DeleteFood(string id, string foodId)
        {
            //unda sheedzllos mxolod owners tavis restoranze
            bool res = _restaurantService.DeleteFood(id, foodId);
            if (!res)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}

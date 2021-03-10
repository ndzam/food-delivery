using FoodDeliveryWebApi.Common;
using FoodDeliveryWebApi.Constants;
using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using FoodDeliveryWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {

        private IOrderService _orderService;
        private IRestaurantService _restaurantService;

        public OrdersController(IOrderService orderService, IRestaurantService restaurantService)
        {
            _orderService = orderService;
            _restaurantService = restaurantService;
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = "fshdsu1";
            var role = "user";
            var res = await _orderService.GetOrders(userId, role);
            
            if (res.Success)
            {
                var order = res.Data;
                
                return Ok(res);
            }
            //unknown error
            return BadRequest();
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(string orderId)
        {
            //amovigo tokenidan
            var userId = "";
            var role = UserRoles.OWNER;
            var res = await _orderService.GetOrder(orderId);
            if (!res.Success)
            {
                //unknown error
            }
            if (res.Success && res.Data == null)
            {
                return NotFound();
            }
            var order = res.Data;
            if (!order.UserId.Equals(userId) && !order.RestaurantOwnerId.Equals(userId))
            {
                //movashoro
                //return Forbid();
            }
            var v = await _restaurantService.GetRestaurant(order.RestaurantId);
            if (!v.Success)
            {
                //unknown error
            }
            res.Data.IsRestaurantDeleted = v.Data == null;
            if (!res.Data.IsRestaurantDeleted)
            {
                var k = await _restaurantService.IsBlocked(res.Data.UserId, res.Data.RestaurantId);
                if (!k.Success)
                {
                    //unknown error
                }
                res.Data.IsUserBlocked = k.Data;
            } else
            {
                res.Data.IsUserBlocked = false;
            }
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> PutOrderStatus(string id, [FromBody]OrderStatusPutRequest req)
        {
            //amovigo tokenidan
            //ase iyos putit? patch?
            var userId = "";
            var role = UserRoles.USER;
            var res = await _orderService.GetOrder(id);
            if (!res.Success)
            {
                //unknown error
                return NotFound();
            }
            if (res.Data == null)
            {
                return NotFound();
            }
            var order = res.Data;
            if(!OrderStatuses.isValidStatusChange(order.Status.CurrentStatus, req.Status, role))
            {
                //es statusi unda?
                return BadRequest();
            }
            if (!order.UserId.Equals(userId) &&  !order.RestaurantOwnerId.Equals(userId) && false)
            {
                return Forbid();
            }
            var v = await _restaurantService.GetRestaurant(order.RestaurantId);
            if (!v.Success)
            {
                //unknown error
                return NotFound();
            }
            if (v.Data == null)
            {
                //washlili yofila restorani, es davabruno?
                return NotFound();
            }
            var k = await _restaurantService.IsBlocked(order.UserId, order.RestaurantId);
            if (!k.Success)
            {
                //unknown error
                return NotFound();
            }
            if (k.Data)
            {
                //dablokilia
                return NotFound();
            }
            order.Status.CurrentStatus = req.Status;
            order.Status.StatusHistory.Add(new StatusHistoryItem
            {
                Date = Utils.GetDateEpoch(),
                Status = req.Status
            });
            var result = await _orderService.UpdateOrderStatus(id, order);
            if (!result.Success)
            {
                //unknown error
                return NotFound();
            }
            //ra davabruno?
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] OrderPostRequest req)
        {
            var userId = "fshdsu1";
            var role = UserRoles.USER;
            if (!role.Equals(UserRoles.USER))
            {
                return Forbid();
            }
            var res = await _restaurantService.GetRestaurant(req.RestaurantId);
            if (!res.Success)
            {
                //unknown error
                return NotFound();
            }
            if (res.Data == null)
            {
                return NotFound();
            }
            var r = await _restaurantService.IsBlocked(userId, req.RestaurantId);
            if (!r.Success)
            {
                //unknown error
                return Forbid();
            }
            if (r.Data)
            {
                //dablokilia
                return Forbid();
            }
            //todo shevamowmo carielze
            var foodsResponse = await _restaurantService.GetAllFood(req.RestaurantId);
            if (!foodsResponse.Success)
            {
                //unknown error
                return Forbid();
            }
            var foods = foodsResponse.Data;
            double total = 0;
            foreach (OrderItem o in req.Items)
            {
                if (o.Quantity <= 0)
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
            var result = await _orderService.CreateOrder(userId, res.Data.OwnerId, req, total);
            if (!result.Success)
            {
                //unknown error
            }
            return Ok(result);
        }
    }
}

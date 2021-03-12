using FoodDeliveryWebApi.Common;
using FoodDeliveryWebApi.Constants;
using FoodDeliveryWebApi.Filters;
using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using FoodDeliveryWebApi.Services;
using Microsoft.AspNetCore.Authorization;
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
    public class OrdersController : FoodDeliveryBaseController
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
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] OrderFilter filter)
        {
            if (filter.Limit > FilterConstants.MAX_LIMIT || filter.Limit < 1)
            {
                //es unda iyos?
                return BadRequest();
            }
            //tu filtris id ar aris...
            if (filter.LastId != null && !filter.LastId.Equals(""))
            {
                var r = await _orderService.GetOrder(filter.LastId);
                if (!r.Success)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                if (r.Data == null)
                {
                    //es unda?
                    return NotFound();
                }
            }
            var res = await _orderService.GetOrders(UserId, Role, filter);
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(string orderId)
        {
            var res = await _orderService.GetOrder(orderId);
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (res.Success && res.Data == null)
            {
                return NotFound();
            }
            var order = res.Data;
            if (!order.UserId.Equals(UserId) && !order.RestaurantOwnerId.Equals(UserId))
            {
                return Forbid();
            }
            var v = await _restaurantService.GetRestaurant(order.RestaurantId);
            if (!v.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            res.Data.IsRestaurantDeleted = v.Data == null;
            if (!res.Data.IsRestaurantDeleted)
            {
                var k = await _restaurantService.IsBlocked(res.Data.UserId, res.Data.RestaurantId);
                if (!k.Success)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                res.Data.IsUserBlocked = k.Data;
            } else
            {
                res.Data.IsUserBlocked = false;
            }
            return Ok(res);
        }

        //todo
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> PutOrderStatus(string id, [FromBody]OrderStatusPutRequest req)
        {
            //ase iyos putit? patch?
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
            if(!OrderStatuses.isValidStatusChange(order.Status.CurrentStatus, req.Status, Role))
            {
                //es statusi unda?
                return BadRequest();
            }
            if (!order.UserId.Equals(UserId) &&  !order.RestaurantOwnerId.Equals(UserId) && false)
            {
                return Forbid();
            }
            var v = await _restaurantService.GetRestaurant(order.RestaurantId);
            if (!v.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (v.Data == null)
            {
                //washlili yofila restorani, es davabruno?
                return NotFound();
            }
            var k = await _restaurantService.IsBlocked(order.UserId, order.RestaurantId);
            if (!k.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
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
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            //ra davabruno?
            return Ok(res);
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] OrderPostRequest req)
        {
            if (!Role.Equals(UserRoles.USER))
            {
                return Forbid();
            }
            var res = await _restaurantService.GetRestaurant(req.RestaurantId);
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (res.Data == null)
            {
                return NotFound();
            }
            var r = await _restaurantService.IsBlocked(UserId, req.RestaurantId);
            if (!r.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (r.Data)
            {
                return Forbid();
            }
            var foodsResponse = await _restaurantService.GetAllFood(req.RestaurantId);
            if (!foodsResponse.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
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
            var result = await _orderService.CreateOrder(UserId, res.Data.OwnerId, req, total);
            if (!result.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            //error code?
            return Ok(result);
        }
    }
}

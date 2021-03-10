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
            var userId = "";
            var res = await _orderService.GetOrder(orderId);
            if (res.Success && res.Data == null)
            {
                return NotFound();
            }
            if (res.Success)
            {
                var order = res.Data;
                if (!order.UserId.Equals(userId) && !order.RestaurantOwnerId.Equals(userId))
                {

                    //todo movashoro
                    //return Forbid();
                }
                return Ok(res);
            }
            //unknown error
            return BadRequest();
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> PutOrderStatus(string id, [FromBody]OrderStatusPutRequest req)
        {
            //unda ecvlebodes mxolod statusi shemdegi principit da mxolod mflobelisgan
            var userId = "";
            var role = UserRoles.USER;
            var res = await _orderService.GetOrder(id);
            if (res.Success && res.Data == null)
            {
                return NotFound();
            }
            if (res.Success)
            {
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
                order.Status.CurrentStatus = req.Status;
                order.Status.StatusHistory.Add(new StatusHistoryItem
                {
                    Date = Utils.GetDateEpoch(),
                    Status = req.Status
                });
                var result = await _orderService.UpdateOrderStatus(id, order);
                if (result.Success)
                {
                    return Ok(res);
                }
                //unknown error
            }
            //unknown error
            return NotFound();
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
            //shesamowmebelia dablokiloba.
            if (!role.Equals(UserRoles.USER))
            {
                return Forbid();
            }
            var res = await _restaurantService.GetRestaurant(req.RestaurantId);
            if (res.Success && res.Data == null)
            {
                return NotFound();
            }
            else if (!res.Success)
            {
                //unknown error
                return NotFound();
            }
            else
            {
                //todo shevamowmo carielze
                var foodsResponse = await _restaurantService.GetAllFood(req.RestaurantId);
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
                //todo shecdoma?
                return Ok(result);
            }
        }
    }
}

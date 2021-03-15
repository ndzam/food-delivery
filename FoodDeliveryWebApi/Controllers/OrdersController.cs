using FoodDeliveryWebApi.Common;
using FoodDeliveryWebApi.Constants;
using FoodDeliveryWebApi.Filters;
using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Requests;
using FoodDeliveryWebApi.Response;
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
        private IUserService _userService;

        public OrdersController(IOrderService orderService, IRestaurantService restaurantService, IUserService userService)
        {
            _orderService = orderService;
            _restaurantService = restaurantService;
            _userService = userService;
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
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INVALID_LIMIT
                });
            }
            if (filter.LastId != null && !filter.LastId.Equals(""))
            {
                var r = await _orderService.GetOrder(filter.LastId);
                if (!r.Success)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                if (r.Data == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.INVALID_LAST_ID
                    });
                }
            }
            var res = await _orderService.GetOrders(UserId, Role, filter);
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            List<OrderDTO> list = new List<OrderDTO>();
            foreach(var v in res.Data)
            {
                
                OrderDTO o = new OrderDTO
                {
                    OrderId = v.OrderId,
                    Status = v.Status,
                    TotalPrice = v.TotalPrice,
                    Date = v.Date
                };
                
                var isBlocked = await _restaurantService.IsBlocked(v.UserId, v.RestaurantId);
                if (!isBlocked.Success)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                o.IsUserBlocked = isBlocked.Data;
                var restaurant = await _restaurantService.GetRestaurant(v.RestaurantId);
                if (!restaurant.Success)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                if (restaurant.Data == null)
                {
                    o.IsRestaurantDeleted = true;
                } else
                {
                    o.IsRestaurantDeleted = false;
                    o.RestaurantName = restaurant.Data.Name;
                }
                list.Add(o);
            }
            return Ok(new ApiResponse<List<OrderDTO>>
            {
                Success = true,
                Data = list
            });
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
            var user = await _userService.Get(res.Data.UserId);
            if (!user.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            List<OrderItemDTO> list = new List<OrderItemDTO>();
            List<Food> foods = null;
            if (!res.Data.IsRestaurantDeleted)
            {
                var f = await _restaurantService.GetAllFood(res.Data.RestaurantId);
                if (!f.Success)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                foods = f.Data;
            }
            foreach (var item in res.Data.Items)
            {
                OrderItemDTO ord = new OrderItemDTO
                {
                    FoodId = item.FoodId,
                    Quantity = item.Quantity
                };
                if(foods != null)
                {
                    var cur = foods.Find(x => x.Id.Equals(item.FoodId));
                    ord.FoodName = cur.Name;
                    ord.Price = cur.Price;
                }
                list.Add(ord);
            }
            OrderDTO o = new OrderDTO
            {
                Date = res.Data.Date,
                IsRestaurantDeleted = res.Data.IsRestaurantDeleted,
                IsUserBlocked = res.Data.IsUserBlocked,
                OrderId = res.Data.OrderId,
                RestaurantId = res.Data.RestaurantId,
                RestaurantOwnerId = res.Data.RestaurantOwnerId,
                Status = res.Data.Status,
                RestaurantName = v.Data == null ? null : v.Data.Name,
                TotalPrice = res.Data.TotalPrice,
                UserId = res.Data.UserId,
                UserName = user.Data.Name,
                Items = list
            };
            return Ok(new ApiResponse<OrderDTO>
            {
                Success = true,
                Data = o
            });
        }

        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> PutOrderStatus(string id, [FromBody]OrderStatusPutRequest req)
        {
            if(req.Status == null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_FIELD,
                });
            }
            var res = await _orderService.GetOrder(id);
            if (!res.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (res.Data == null)
            {
                return NotFound();
            }
            var order = res.Data;
            if(!OrderStatuses.isValidStatusChange(order.Status.CurrentStatus, req.Status, Role))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ORDER_INVALID_STATUS_CHANGE,
                });
            }
            if (!order.UserId.Equals(UserId) &&  !order.RestaurantOwnerId.Equals(UserId))
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
                return Forbid();
            }
            var k = await _restaurantService.IsBlocked(order.UserId, order.RestaurantId);
            if (!k.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (k.Data)
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
            if (!result.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            var user = await _userService.Get(res.Data.UserId);
            if (!user.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            List<OrderItemDTO> list = new List<OrderItemDTO>();
            var f = await _restaurantService.GetAllFood(res.Data.RestaurantId);
            if (!f.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            List<Food> foods = f.Data;
            foreach (var item in res.Data.Items)
            {
                OrderItemDTO ord = new OrderItemDTO
                {
                    FoodId = item.FoodId,
                    Quantity = item.Quantity
                };
                if (foods != null)
                {
                    var cur = foods.Find(x => x.Id.Equals(item.FoodId));
                    ord.FoodName = cur.Name;
                    ord.Price = cur.Price;
                }
                list.Add(ord);
            }
            OrderDTO o = new OrderDTO
            {
                Date = res.Data.Date,
                IsRestaurantDeleted = res.Data.IsRestaurantDeleted,
                IsUserBlocked = res.Data.IsUserBlocked,
                OrderId = res.Data.OrderId,
                RestaurantId = res.Data.RestaurantId,
                RestaurantOwnerId = res.Data.RestaurantOwnerId,
                Status = res.Data.Status,
                RestaurantName = v.Data.Name,
                TotalPrice = res.Data.TotalPrice,
                UserId = res.Data.UserId,
                UserName = user.Data.Name,
                Items = list
            };
            return Ok(new ApiResponse<OrderDTO>
            {
                Success = true,
                Data = o
            });
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
            if(req.RestaurantId==null || req.Items == null || req.Items.Count == 0)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_FIELD,
                });
            }
            foreach (var item in req.Items)
            {
                if(item.FoodId == null || item.Quantity == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.MISSING_FIELD,
                    });
                }
            }
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
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ORDER_INVALID_RESTAURANT_ID,
                });
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
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.ORDER_FOOD_INVALID_QUANTITY,
                    });
                }
                Food f = foods.FirstOrDefault(x => x.Id == o.FoodId);
                if (f == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.ORDER_INVALID_FOOD_ID,
                    });
                }
                else
                {
                    total += f.Price * (int)o.Quantity;
                }
            }
            var result = await _orderService.CreateOrder(UserId, res.Data.OwnerId, req, total);
            if (!result.Success)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtAction(nameof(GetOrder), new { orderId = result.Data.OrderId }, result);
        }
    }
}

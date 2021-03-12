using FoodDeliveryWebApi.Constants;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Controllers
{
    public class FoodDeliveryBaseController : ControllerBase
    {

        public string Role 
        { 
            get {
                var roleClaim = User.Claims.Where(x => x.Type == Claims.ROLE)
                 .FirstOrDefault();
                if(roleClaim == null || roleClaim.Value == null)
                {
                    return null;
                }
                return roleClaim.Value;
            } 
        }

        public string UserId
        {
            get
            {
                
                var roleClaim = User.Claims.Where(x => x.Type == Claims.USER_ID)
                 .FirstOrDefault();
                if (roleClaim == null || roleClaim.Value == null)
                {
                    return null;
                }
                return roleClaim.Value;
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace FoodDeliveryWebApi.Controllers
{
    public class FoodDeliveryBaseController : ControllerBase
    {

        public string Role 
        { 
            get {
                var roleClaim = User.Claims.Where(x => x.Type == ClaimTypes.Role)
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
                
                var roleClaim = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
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

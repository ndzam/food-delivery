using FoodDeliveryWebApi.Constants;
using FoodDeliveryWebApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.AuthorizationHandlers
{
    public class ValidateAuthenticationSchemeOptions
        : AuthenticationSchemeOptions
    { }
    internal class FirebaseAuthenticationHandler : AuthenticationHandler<ValidateAuthenticationSchemeOptions>
    {
        private readonly IFirebaseService _firebaseService;

        public FirebaseAuthenticationHandler(
            IOptionsMonitor<ValidateAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IFirebaseService firebaseService)
            : base(options, logger, encoder, clock)
        {
            _firebaseService = firebaseService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Header Not Found."));
            }
            var tok = Request.Headers["Authorization"].ToString();
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tok);

                var claims = token.Claims;
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, nameof(FirebaseAuthenticationHandler));

                // generate AuthenticationTicket from the Identity
                // and current authentication scheme
                var ticket = new AuthenticationTicket(
                    new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);

                // pass on the ticket to the middleware
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid token"));
            }

        }
    }
}
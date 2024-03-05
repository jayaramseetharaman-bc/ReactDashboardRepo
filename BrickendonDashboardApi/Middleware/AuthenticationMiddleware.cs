using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace BrickendonDashboard.Api.Middleware
{
  public class AuthenticationMiddleware
  {
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
      private readonly IHttpContextAccessor _httpContextAccessor;

      public CustomAuthenticationHandler(
          IOptionsMonitor<AuthenticationSchemeOptions> options,
          ILoggerFactory logger,
          UrlEncoder encoder,
          ISystemClock clock,
          IHttpContextAccessor httpContextAccessor) : base(options, logger, encoder, clock)
      {
        _httpContextAccessor = httpContextAccessor;
      }

      protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
      {
        var request = _httpContextAccessor.HttpContext.Request;

        // Check for API key in header
        var apiKey = request.Headers["X-API-KEY"].FirstOrDefault();
        if (!string.IsNullOrEmpty(apiKey))
        {
          if (IsValidApiKey(apiKey))
          {
            var claims = new[] { new Claim(ClaimTypes.Name, "APIUser") };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
          }
          else
          {
            return AuthenticateResult.Fail("Invalid API key");
          }
        }

        // Check for JWT bearer token
        // var jwtToken = await request.HttpContext.GetTokenAsync("Bearer", "access_token");
        var bearerToken = request.Headers["Authorization"].FirstOrDefault();
        var jwtToken = bearerToken?.Split(' ').Last();
        if (!string.IsNullOrEmpty(jwtToken))
        {
          // Validate and parse JWT token
          if (IsValidJwtToken(jwtToken))
          {
            // Parse claims from JWT token
            var claims = ParseClaimsFromJwtToken(jwtToken);
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
          }
          else
          {
            return AuthenticateResult.Fail("Invalid JWT token");
          }
        }

        // No valid token found
        return AuthenticateResult.NoResult();
      }

      private bool IsValidApiKey(string apiKey)
      {
        // Implement logic to validate API key
        return apiKey == "YOUR_API_KEY";
      }

      private bool IsValidJwtToken(string jwtToken)
      {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
          ValidateAudience = true,
         // ValidAudience = "http://localhost:3000/",
          ValidAudience = "00000003-0000-0000-c000-000000000000",
          ValidateIssuer = false,
          ValidateLifetime = true,
          ValidateSignatureLast =false
        };
        try
        {
          SecurityToken validatedToken;
          var principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out validatedToken);
          return true;
        }
        catch (SecurityTokenExpiredException ex)
        {
          Console.WriteLine(ex.ToString());
          // Token is expired
          return false;
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
          return false;

        }
      }

      private IEnumerable<Claim> ParseClaimsFromJwtToken(string jwtToken)
      {
        // Implement logic to parse claims from JWT token
        // For example, use a JWT library to parse the token
        return new List<Claim>(); // Dummy implementation
      }
    }

  }
}

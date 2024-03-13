using Azure;
using BrickendonDashboard.Domain.Contexts;
using BrickendonDashboard.Domain.Contracts;
using BrickendonDashboard.Domain.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using static System.Net.WebRequestMethods;
using RequestContext = BrickendonDashboard.Domain.Contexts.RequestContext;

namespace BrickendonDashboard.Api.Middleware
{
  public class AuthenticatorMiddleware
  {
     private RequestDelegate _next;
     private readonly ApplicationConfigurationInfo _appConfig;
    private readonly IServiceProvider _serviceProvider;

    public AuthenticatorMiddleware(RequestDelegate next, ApplicationConfigurationInfo appConfig, IServiceProvider serviceProvider)
    {
      _next=next;
      _appConfig=appConfig;
      _serviceProvider=serviceProvider;
    }

    public async Task Invoke (HttpContext context, RequestContext requestContext)
    {
      var apiKey = context.Request.Headers["X-API-Key"];
      var bearerToken = context.Request.Headers["Authorization"].FirstOrDefault();
      requestContext.ApiKey = apiKey;


      if (string.IsNullOrWhiteSpace(bearerToken) && (string.IsNullOrEmpty(apiKey) || (apiKey != _appConfig.ApiKey)))
      {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
      }
      else
      {
        // validate bearer token
        if (!string.IsNullOrEmpty(bearerToken))
        {
          var token = bearerToken?.Split(' ').Last();

          if (token!= null)
          {
            var isValidToken = await ValidateMsToken(token,_appConfig.JwtTokenValidationInfo.Audience,_appConfig.JwtTokenValidationInfo.Issuer,requestContext);

            if (!isValidToken)
            {
              throw new UnauthorizedAccessException();
            }
          }
          else
          {
            throw new UnauthorizedAccessException();
          }
        }
        if (!string.IsNullOrEmpty(requestContext.UserName))
        {
          var isValidUser = await CheckIfUserExists(requestContext.UserName);
          if (!isValidUser)
          {
            throw new UnauthorizedAccessException();
          }
        }
        await _next(context);
			}
    }
    public async Task <bool> ValidateMsToken(string token, string audience, string issuer, RequestContext requestContext)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var validationParameters = new TokenValidationParameters
      {
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = false,
        SignatureValidator = delegate (string token, TokenValidationParameters parameters)
        {
          var jwt = new JwtSecurityToken(token);

          return jwt;
        },
      };
      try
      {
        SecurityToken validatedToken;
        var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
        var userNameclaims = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "unique_name");
        if (userNameclaims != null)
        {
          var userName = userNameclaims.Value;
          requestContext.UserName = userName;
        }
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
    private async Task<bool> CheckIfUserExists(string userName)
    {
      bool isUserExists = false;
      using (var scope = _serviceProvider.CreateScope())
      {
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        isUserExists = await userService.IsUserActive(userName);
      }

      return isUserExists;
    }
  }
}

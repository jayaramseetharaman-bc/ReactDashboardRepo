using BrickendonDashboard.Domain.Contexts;
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

namespace BrickendonDashboard.Api.Middleware
{
  public class AuthenticatorMiddleware
  {
     private RequestDelegate _next;
     private readonly ApplicationConfigurationInfo _appConfig;

    public AuthenticatorMiddleware(RequestDelegate next, ApplicationConfigurationInfo appConfig)
    {
      _next=next;
      _appConfig=appConfig;
    }

    public async Task Invoke (HttpContext context, RequestContext requestContext)
    {
      var apiKey = context.Request.Headers["X-API-Key"];
      var bearerToken = context.Request.Headers["Authorization"].FirstOrDefault();
      requestContext.apiKey = apiKey;


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
            var IsValidToken = await ValidateMsToken(token,_appConfig.JwtTokenValidationInfo.Audience,_appConfig.JwtTokenValidationInfo.Issuer);

            if (!IsValidToken)
            {
              throw new UnauthorizedAccessException();
            }
          }
          else
          {
            throw new UnauthorizedAccessException();
          }
          await _next(context);
        }
      }

    }
    public async Task <bool> ValidateMsToken(string token,string audience,string issuer)
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

  }
}

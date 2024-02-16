using BrickendonDashboard.Domain;
using BrickendonDashboard.Domain.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Services.Wrapper
{
  public class WrapperApiService : IWrapperApiService
  {
    private readonly IHttpClientFactory _httpClientFactory;

    public WrapperApiService(IHttpClientFactory httpClientFactory)
    {
      _httpClientFactory = httpClientFactory;
    }

    public async Task<T> GetAsync<T>( string? url, string cacheKey, string errorMessage)
    {

      var client = GetHttpClient();

      var response = await client.GetAsync(url);
      var responseString = await response.Content.ReadAsStringAsync();

      if (response.IsSuccessStatusCode)
      {
        var result = JsonConvert.DeserializeObject<T>(responseString);
        return result;
      }
      else if (response.StatusCode == HttpStatusCode.NotFound)
      {
        throw new ResourceNotFoundException();
      }
      else if (response.StatusCode == HttpStatusCode.BadRequest)
      {
        throw new CustomException(responseString);
      }
      else
      {
        throw new CustomException(errorMessage);
      }
    }

    public async Task<T> PostAsync<T>(string? url, object requestData, string errorMessage)
    {
      var client = GetHttpClient();

      HttpContent data = new StringContent(JsonConvert.SerializeObject(requestData, new JsonSerializerSettings
      { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Encoding.UTF8, "application/json");

      var response = await client.PostAsync(url, data);
      var responseString = await response.Content.ReadAsStringAsync();

      if (response.IsSuccessStatusCode)
      {
        var result = JsonConvert.DeserializeObject<T>(responseString);
        return result;
      }
      else if (response.StatusCode == HttpStatusCode.NotFound)
      {
        throw new ResourceNotFoundException();
      }
      else if (response.StatusCode == HttpStatusCode.BadRequest)
      {
        throw new CustomException(responseString);
      }
      else
      {
        throw new CustomException(errorMessage);
      }
    }

    public async Task PutAsync(string? url, object requestData, string errorMessage)
    { 
      var client = GetHttpClient();

      HttpContent data = new StringContent(JsonConvert.SerializeObject(requestData, new JsonSerializerSettings
      { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Encoding.UTF8, "application/json");

      var response = await client.PutAsync(url, data);
      var responseString = await response.Content.ReadAsStringAsync();
      if (response.StatusCode == HttpStatusCode.BadRequest)
      {
        throw new CustomException(responseString);
      }
      else if (response.StatusCode == HttpStatusCode.NotFound)
      {
        throw new ResourceNotFoundException();
      }
      else if (!response.IsSuccessStatusCode)
      {
        throw new CustomException(errorMessage);
      }
    }

    public async Task<T> PutAsync<T>( string? url, object requestData, string errorMessage)
    {
      var client = GetHttpClient();

      HttpContent data = new StringContent(JsonConvert.SerializeObject(requestData, new JsonSerializerSettings
      { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Encoding.UTF8, "application/json");

      var response = await client.PutAsync(url, data);
      var responseString = await response.Content.ReadAsStringAsync();

      if (response.IsSuccessStatusCode)
      {
        var result = JsonConvert.DeserializeObject<T>(responseString);
        return result;
      }
      else if (response.StatusCode == HttpStatusCode.BadRequest)
      {
        throw new CustomException(responseString);
      }
      else if (response.StatusCode == HttpStatusCode.NotFound)
      {
        throw new ResourceNotFoundException();
      }
      else if (!response.IsSuccessStatusCode)
      {
        throw new CustomException(errorMessage);
      }
      else
      {
        throw new CustomException(errorMessage);
      }
    }

    public async Task<T> DeleteAsync<T>( string? url, string errorMessage)
    {
      var client = GetHttpClient();

      var response = await client.DeleteAsync(url);
      var responseString = await response.Content.ReadAsStringAsync();

      if (response.IsSuccessStatusCode)
      {
        var result = JsonConvert.DeserializeObject<T>(responseString);
        return result;
      }
      else if (response.StatusCode == HttpStatusCode.NotFound)
      {
        throw new ResourceNotFoundException();
      }
      else if (response.StatusCode == HttpStatusCode.BadRequest)
      {
        throw new CustomException(responseString);
      }
      else
      {
        throw new CustomException(errorMessage);
      }
    }

    private HttpClient GetHttpClient()
    {
      var clientName = "DashboardUsersApi";
      var client = _httpClientFactory.CreateClient(clientName);

      //if (_requestContext != null && _requestContext.UserId > 0)
      //{
      //  client.DefaultRequestHeaders.Add("USER-ID", _requestContext.UserId.ToString());
      //}

      //if (_requestContext != null && !string.IsNullOrWhiteSpace(_requestContext.UserName))
      //{
      //  client.DefaultRequestHeaders.Add("USER-NAME", _requestContext.UserName);
      //}

      return client;
    }
  }
}

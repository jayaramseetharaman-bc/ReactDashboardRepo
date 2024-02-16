using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Domain
{
  public interface IWrapperApiService
  {
    Task<T> GetAsync<T>(string? url, string cacheKey, string errorMessage);
    Task<T> PostAsync<T>(string? url, object requestData, string errorMessage);
    Task PutAsync(string? url, object requestData, string errorMessage);
    Task<T> DeleteAsync<T>(string? url, string errorMessage);
    Task<T> PutAsync<T>(string? url, object requestData, string errorMessage);
  }
}

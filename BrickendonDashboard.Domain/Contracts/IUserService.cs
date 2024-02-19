using BrickendonDashboard.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Domain.Contracts
{
  public interface IUserService
  {
    Task<UserListResponseInfo> GetUsersWithPaginationAsync(UserListFilterCriteria userListFilterCriteria);
    Task<UserDetails> GetUserAsync(int userId);
		Task DeleteUser(int userId);
    Task<UserResponseInfo> CreateUserAsync(UserRequestInfo userRequestInfo);
    Task<UserResponseInfo> UpdateUserAsync(int userId,UserEditRequestInfo userRequestInfo);

  }
}

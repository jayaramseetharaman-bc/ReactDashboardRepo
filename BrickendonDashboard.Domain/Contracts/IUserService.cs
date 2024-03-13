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
    Task<UserDto> GetUserAsync(string userId);
		Task DeleteUser(string userId);
    Task<UserResponseInfo> CreateUserAsync(UserRequestInfo userRequestInfo);
    Task<UserResponseInfo> UpdateUserAsync(string userId,UserEditRequestInfo userRequestInfo);

		Task<bool> IsUserExist(string userName);
    Task<bool> IsUserActive(string userName);


  }
}

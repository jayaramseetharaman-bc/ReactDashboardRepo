using BrickendonDashboard.DBModel.Entities;
using BrickendonDashboard.Domain.Contracts;
using BrickendonDashboard.Domain.Dtos;
using BrickendonDashboard.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BrickendonDashboard.Services
{
  public class UserService : IUserService
  {
    private readonly IDataContext _dataContext;

    public UserService(IDataContext dataContext)
    {
      _dataContext=dataContext;
    }


    public async Task<UserListResponseInfo> GetUsersWithPaginationAsync(UserListFilterCriteria userListFilterCriteria)
    {
      UserListResponseInfo userListResponseInfo = new UserListResponseInfo();
      Expression<Func<UserDto, object>> sortExpression = null;
      var searchKeyword = userListFilterCriteria.SearchKeyword;
      var sortBy = userListFilterCriteria.SortBy;
      var sortOrder = userListFilterCriteria.SortOrder;

      IQueryable<User> userQuery = null;

      userQuery = _dataContext.User
        .Where(x => x.IsDeleted == false);

      if (userQuery == null)
      {
        throw new ResourceNotFoundException();
      }

      if (searchKeyword != null)
      {
        userQuery = userQuery.Where(u => u.Email.Contains(searchKeyword) || (u.FirstName + " " + u.LastName).ToLower().Contains(searchKeyword));

      }

      var userDtoQuery = userQuery.Select(user => new UserDto()
      {
        UserId = user.Id,
        UserName = user.FirstName + " " + user.LastName,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
        ContactNumber = user.MobileNumber,
        Address = user.Address,
        UserType = user.UserTypeId,
        IsActive = user.IsActive
      });
      switch (sortBy)
      {
        case "userEmail":
          sortExpression = u => u.Email;
          break;
        case "userId":
          sortExpression = u => u.UserId;
          break;
        case "userName":
          sortExpression = u => u.UserName;
          break;
        default:
          sortExpression = u => u.UserName;
          break;
      }
      IOrderedQueryable<UserDto> sortedUserDtoQuery;
      if (sortOrder == "DESC")
      {
        sortedUserDtoQuery = userDtoQuery.OrderByDescending(sortExpression);
      }
      else
      {
        sortedUserDtoQuery = userDtoQuery.OrderBy(sortExpression);
      }

      var currentPage = userListFilterCriteria.PageIndex;
      var pageSize = userListFilterCriteria.PageSize;
      var skipCount = (currentPage - 1) * pageSize;
      var rowCount = await sortedUserDtoQuery.CountAsync();
      var userDtoList = await sortedUserDtoQuery.Skip(skipCount).Take(pageSize).ToListAsync();
      var pageCount = (double)rowCount / pageSize;

      userListResponseInfo.CurrentPage = currentPage;
      userListResponseInfo.PageCount = (int)Math.Ceiling(pageCount);
      userListResponseInfo.RowCount = rowCount;
      userListResponseInfo.UserList = userDtoList ?? new List<UserDto>();

      return userListResponseInfo;

    }

		public async Task<UserDetails> GetUserAsync(int userId)
		{
			var user = await _dataContext.User
					.Include(u => u.UserRole)
					.ThenInclude(ur => ur.Role)
					.Where(x => x.IsDeleted == false && x.Id == userId)
					.FirstOrDefaultAsync();

			if (user == null)
			{
				throw new ResourceNotFoundException();
			}

			var userDto = new UserDto()
			{
				UserId = user.Id,
				UserName = user.FirstName + " " + user.LastName,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				ContactNumber = user.MobileNumber,
				Address = user.Address,
				UserType = user.UserTypeId,
				IsActive = user.IsActive
			};

			var roleIds = user.UserRole.Select(ur => ur.RoleId).ToList();

			return new UserDetails
			{
				UserData = userDto,
				RoleIds = roleIds
			};
		}


		public async Task<UserResponseInfo> CreateUserAsync(UserRequestInfo userRequestInfo)
    {
      var user = await _dataContext.User
        .FirstOrDefaultAsync(u => !u.IsDeleted && u.Email == userRequestInfo.Email);

      if (user != null)
      {
        throw new ResourceAlreadyExistsException();
      }
      user = new User
      {
        FirstName = userRequestInfo.FirstName,
        LastName = userRequestInfo.LastName,
        MobileNumber = userRequestInfo.MobileNumber,
        Email = userRequestInfo.Email,
        Address = userRequestInfo.Address,
        UserTypeId = userRequestInfo.UserTypeId,
        IsActive = true
      };

      _dataContext.User.Add(user);
      await _dataContext.SaveChangesAsync();

      if (userRequestInfo.RoleIds != null && userRequestInfo.RoleIds.Any())
      {
        foreach (var roleId in userRequestInfo.RoleIds)
        {
          var userRole = new UserRole
          {
            UserId = user.Id,
            RoleId = roleId
          };
          _dataContext.UserRole.Add(userRole);
        }
        await _dataContext.SaveChangesAsync();
      }

      return new UserResponseInfo
      {
        UserId = user.Id,
        Email = user.Email
      };
    }

		public async Task<UserResponseInfo> UpdateUserAsync(int userId,UserEditRequestInfo userEditRequestInfo)
    {
      var user = await _dataContext.User
        .FirstOrDefaultAsync(u => !u.IsDeleted && u.Email == userEditRequestInfo.userRequestInfo.Email && u.Id== userId);

      if (user == null)
      {
        throw new ResourceNotFoundException();
      }

      user.FirstName = userEditRequestInfo.userRequestInfo.FirstName;
      user.LastName = userEditRequestInfo.userRequestInfo.LastName;
      user.MobileNumber = userEditRequestInfo.userRequestInfo.MobileNumber;
      user.Email = userEditRequestInfo.userRequestInfo.Email;
      user.Address = userEditRequestInfo.userRequestInfo.Address;
      user.UserTypeId = userEditRequestInfo.userRequestInfo.UserTypeId;
      user.IsActive = userEditRequestInfo.IsActive;

      var existingUserRoles = await _dataContext.UserRole
      .Where(ur => ur.UserId == user.Id)
      .ToListAsync();

      _dataContext.UserRole.RemoveRange(existingUserRoles);

      if (userEditRequestInfo.userRequestInfo.RoleIds != null && userEditRequestInfo.userRequestInfo.RoleIds.Any())
      {
        foreach (var roleId in userEditRequestInfo.userRequestInfo.RoleIds)
        {
          var userRole = new UserRole
          {
            UserId = user.Id,
            RoleId = roleId
          };
          _dataContext.UserRole.Add(userRole);
        }
      }

      await _dataContext.SaveChangesAsync();

      return new UserResponseInfo
      {
        UserId = user.Id,
        Email = user.Email
      };
    }



    public async Task DeleteUser(int userId)
    {
      var user = await _dataContext.User
        .Include(x => x.UserRole)
        .Where(x => x.Id == userId).FirstOrDefaultAsync();
      if (user != null)
      {
        user.IsDeleted = true;
        user.IsActive = false;

        foreach (var userRole in user.UserRole)
        {
          userRole.IsDeleted = true;
        }
        await _dataContext.SaveChangesAsync();

      }

    }
  }
}
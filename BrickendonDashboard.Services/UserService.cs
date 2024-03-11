using BrickendonDashboard.DBModel.Entities;
using BrickendonDashboard.Domain.Contracts;
using BrickendonDashboard.Domain.Dtos;
using BrickendonDashboard.Domain.Exceptions;
using BrickendonDashboard.Shared.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BrickendonDashboard.Services
{
  public class UserService : IUserService
  {
    private readonly IDataContext _dataContext;
		private readonly IHelperService _userHelper;

		public UserService(IDataContext dataContext,IHelperService helperService)
    {
      _dataContext=dataContext;
      _userHelper = helperService;
    }


    public async Task<UserListResponseInfo> GetUsersWithPaginationAsync(UserListFilterCriteria userListFilterCriteria)
    {
      var searchKeyword = userListFilterCriteria.SearchKeyword;
      var sortBy = userListFilterCriteria.SortBy ?? "userName";
			var sortOrder = userListFilterCriteria.SortOrder;

      IQueryable<User> userQuery = null;

      userQuery = _dataContext.User
        .Include(u => u.UserRole)
          .ThenInclude(ur => ur.Role)
          .Where(x => x.IsDeleted == false);

      if (userQuery == null)
      {
        throw new ResourceNotFoundException();
      }

      if (searchKeyword != null)
      {
        userQuery = userQuery.Where(u => u.Email.Contains(searchKeyword) || (u.FirstName + " " + u.LastName).ToLower().Contains(searchKeyword));

      }
      if (userListFilterCriteria.Roles != null && userListFilterCriteria.Roles.Any())
      {
        userQuery = userQuery.Where(u => u.UserRole.Any(ur => userListFilterCriteria.Roles.Contains(ur.RoleId)));
      }
      if (userListFilterCriteria.Status.HasValue)
      {
        userQuery = userQuery.Where(u => u.IsActive == userListFilterCriteria.Status.Value);

      }

      var sortedUserQuery = _dataContext.GetSortedResult(userQuery, userListFilterCriteria.SortBy, userListFilterCriteria.SortOrder);
      var resultSetCriteria = new ResultSetCriteria
      {
        CurrentPage = userListFilterCriteria.PageIndex,
        PageSize = userListFilterCriteria.PageSize
      };

      var pagedResult = await _dataContext.GetPagedResultAsync(sortedUserQuery, resultSetCriteria);

      var userListResponseInfo = new UserListResponseInfo
      {
        CurrentPage = pagedResult.CurrentPage,
        PageCount = pagedResult.PageCount,
        RowCount = pagedResult.RowCount,
        UserList = pagedResult.Results?.Select(u => new UserDto
        {
          UserId = u.Id,
          UserName = u.FirstName + " " + u.LastName,
          FirstName = u.FirstName,
          LastName = u.LastName,
          Email = u.Email,
          ContactNumber = u.MobileNumber,
          Address = u.Address,
          UserType = u.UserTypeId,
          IsActive = u.IsActive,
          RoleIds = u.UserRole.Select(ur => ur.RoleId).ToList()
        }).ToList() ?? new List<UserDto>()
      };
      return userListResponseInfo;

    }

    public async Task<UserDto> GetUserAsync(string userId)
		{
			_userHelper.ValidateUserName(userId);

			var user = await _dataContext.User
					.Include(u => u.UserRole)
					.ThenInclude(ur => ur.Role)
					.Where(x => x.IsDeleted == false && x.Email == userId)
					.FirstOrDefaultAsync();

			if (user == null)
			{
        throw new CustomException(ErrorConstant.ErrorInvalidUserId);
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
				IsActive = user.IsActive,
        RoleIds = user.UserRole.Select(ur => ur.RoleId).ToList(),
		  };
      return userDto;
		}


		public async Task<UserResponseInfo> CreateUserAsync(UserRequestInfo userRequestInfo)
    {
			var userName = userRequestInfo.Email.ToLower();

			_userHelper.ValidateUserName(userName);

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

		public async Task<UserResponseInfo> UpdateUserAsync(string userId, UserEditRequestInfo userEditRequestInfo)
    {
      _userHelper.ValidateUserName(userId);

			if (userId != userEditRequestInfo.userRequestInfo.Email)
			{
				throw new CustomException(ErrorConstant.ErrorInvalidUserId);
			}

			var user = await _dataContext.User
					.FirstOrDefaultAsync(u => !u.IsDeleted && u.Email == userId);

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

		
			foreach (var existingRole in existingUserRoles)
			{
				if (!userEditRequestInfo.userRequestInfo.RoleIds.Contains(existingRole.RoleId))
				{
					_dataContext.UserRole.Remove(existingRole);
				}
			}

			if (userEditRequestInfo.userRequestInfo.RoleIds != null)
			{
				foreach (var roleId in userEditRequestInfo.userRequestInfo.RoleIds)
				{
					if (!existingUserRoles.Any(ur => ur.RoleId == roleId))
					{
						var userRole = new UserRole
						{
							UserId = user.Id,
							RoleId = roleId
						};
						_dataContext.UserRole.Add(userRole);
					}
				}
			}

			await _dataContext.SaveChangesAsync();

			return new UserResponseInfo
			{
				UserId = user.Id,
				Email = user.Email
			};
		}


		public async Task DeleteUser(string userId)
    {
			_userHelper.ValidateUserName(userId);

			var user = await _dataContext.User
        .Include(x => x.UserRole)
        .Where(x => x.Email == userId).FirstOrDefaultAsync();
			
      if (user == null)
			{
				throw new ResourceNotFoundException();
			}
        user.IsDeleted = true;
        user.IsActive = false;

        foreach (var userRole in user.UserRole)
        {
          userRole.IsDeleted = true;
        }

        await _dataContext.SaveChangesAsync();

    }

		public async Task<bool> IsUserExist(string userName)
		{
			var user = await _dataContext.User.FirstOrDefaultAsync(u => !u.IsDeleted && u.Email.ToLower() == userName.ToLower());

			return user != null;
		}
	}
}
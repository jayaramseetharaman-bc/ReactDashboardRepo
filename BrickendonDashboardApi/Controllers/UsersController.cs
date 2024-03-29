using BrickendonDashboard.Domain.Contracts;
using BrickendonDashboard.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BrickendonDashboardApi.Controllers
{
  [EnableCors("AllowOrigin")]
  [ApiController]
  [Route("users")]
  public class UsersController : ControllerBase
  {

    private readonly ILogger<UsersController> _logger;
    private readonly IUserService _userService;

    public UsersController(ILogger<UsersController> logger, IUserService userService)
    {
      _logger = logger;
      _userService = userService;
    }

		/// <summary>
		/// Allows a user to get user details based on pagination and data filters
		/// </summary>
		/// <response  code="200">Ok</response>
		/// <response  code="401">Unauthorised</response>
		/// <response  code="403">Forbidden</response>
		/// <response  code="404">Not found</response>
		/// <response  code="400">Bad request.
		/// </response>
		/// <remarks>
		/// Sample request:
		///
		///     GET /users/data
		///
		/// </remarks>

		[HttpGet("data")]
    [SwaggerOperation(Tags = new[] { "Users" })]
    [ProducesResponseType(typeof(UserListResponseInfo), 200)]
    public async Task<UserListResponseInfo> GetUsersList([FromQuery] UserListFilterCriteria userListFilterCriteria)
    {
      userListFilterCriteria.PageIndex = userListFilterCriteria.PageIndex <= 0 ? 1 : userListFilterCriteria.PageIndex;
      userListFilterCriteria.PageSize = userListFilterCriteria.PageSize <=0 ? 10 : userListFilterCriteria.PageSize;
      return await _userService.GetUsersWithPaginationAsync(userListFilterCriteria);
    }

		/// Allows a user to retrieve details for a given user
		/// </summary>
		/// <response  code="200">Ok</response>
		/// <response  code="401">Unauthorised</response>
		/// <response  code="403">Forbidden</response>
		/// <response  code="404">Not found</response>
		/// <response  code="400">Bad request.
		/// Error Codes: INVALID_USER_ID
		/// </response>
		/// <remarks>
		/// Sample request:
		///
		///    GET /users?user-id=7 
		///     
		///
		/// </remarks>
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Users" })]
		[ProducesResponseType(typeof(UserDto), 200)]
		public async Task<UserDto> GetUser([FromQuery(Name = "user-id")] string userId)
		{
			return await _userService.GetUserAsync(userId);
		}

		/// <summary>
		/// Create a new user 
		/// </summary>
		/// <response  code="200">Ok</response>
		/// <response  code="401">Unauthorised</response>
		/// <response  code="403">Forbidden</response>
		/// <response  code="404">Not found</response>
		/// <response  code="400">Bad request.
		/// </response>
		/// <remarks>
		/// Sample request:
		///
		/// Post/users/
		/// {
		/// "firstName": "string",
		/// "lastName": "string",
		/// "mobileNumber": "string",
		///  "email": "string",
		///  "address": "string",
		///  "userTypeId": 0,
		/// "roleIds": [
		///   0
		///  ]
		/// }
		///
		/// </remarks>


		[HttpPost]
    [SwaggerOperation(Tags = new[] { "Users" })]
    [ProducesResponseType(typeof(UserResponseInfo), 200)]
    public async Task<UserResponseInfo> CreateUser(UserRequestInfo userRequestInfo)
    {
      return await _userService.CreateUserAsync(userRequestInfo);
    }
    /// <summary>
    /// Update an existing user 
    /// </summary>
    /// <response  code="200">Ok</response>
    /// <response  code="401">Unauthorised</response>
    /// <response  code="403">Forbidden</response>
    /// <response  code="404">Not found</response>
    /// <response  code="400">Bad request.
    /// </response>
    /// <remarks>
    /// Sample request:
    ///
    /// Put/users/
    /// {
    ///  "userRequestInfo": {
    /// "firstName": "string",
    /// "lastName": "string",
    /// "mobileNumber": "string",
    ///  "email": "string",
    ///  "address": "string",
    ///  "userTypeId": 0,
    /// "roleIds": [
    ///   0
    ///  ]
    /// },
    ///"isActive": true
    ///}
    ///
    /// </remarks>

    [HttpPut]
    [SwaggerOperation(Tags = new[] { "Users" })]
    [ProducesResponseType(typeof(UserResponseInfo), 200)]
    public async Task<UserResponseInfo> UpdateUser([FromQuery(Name = "user-id")] string userId, UserEditRequestInfo userEditRequestInfo)
    {
      return await _userService.UpdateUserAsync(userId, userEditRequestInfo);
    }

		/// <summary>
		/// Delete an existing user 
		/// </summary>
		/// <response  code="200">Ok</response>
		/// <response  code="401">Unauthorised</response>
		/// <response  code="403">Forbidden</response>
		/// <response  code="404">Not found</response>
		/// <response  code="400">Bad request.
		/// </response>
		/// <remarks>
		/// Sample request:
		///
		///     Delete /users/user-id
		///
		/// </remarks>

		[HttpDelete]
    [SwaggerOperation(Tags = new[] { "Users" })]
    public async Task DeleteUser([FromQuery(Name = "user-id")] string userId)
    {
      await _userService.DeleteUser(userId);

    }


		/// <summary>
		/// Check if user exists 
		/// </summary>
		/// <response  code="200">Ok</response>
		/// <response  code="401">Unauthorised</response>
		/// <response  code="403">Forbidden</response>
		/// <response  code="404">Not found</response>
		/// <response  code="400">Bad request</response>
		/// <remarks>
		/// Sample request:
		///
		///     Get /users/is-exist?user-id=user@mail.com
		/// </remarks>
		[HttpGet("is-exist")]
		[SwaggerOperation(Tags = new[] { "User" })]
		[ProducesResponseType(typeof(bool), 200)]
		public async Task<bool> IsUserExistAsync([FromQuery(Name = "user-id")] string userId)
		{
			return await _userService.IsUserExist(userId);
		}

	}
}
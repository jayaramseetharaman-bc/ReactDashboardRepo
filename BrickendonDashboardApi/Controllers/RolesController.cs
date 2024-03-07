using BrickendonDashboard.Domain.Contracts;
using BrickendonDashboard.Domain.Dtos;
using BrickendonDashboardApi.Controllers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BrickendonDashboard.Api.Controllers
{
  [EnableCors("AllowOrigin")]
  [ApiController]
  [Route("users")]
  public class RolesController
  {
    private readonly ILogger<RolesController> _logger;
    private readonly IRolesService _rolesService;
    public RolesController(ILogger<RolesController> logger,IRolesService rolesService) 
    {
      _logger = logger;
      _rolesService = rolesService;
    }
    /// <summary>
    /// Allow the user to get role details 
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
    ///     GET /users/roles
    ///
    /// </remarks>

    [HttpGet("roles")]
    [SwaggerOperation(Tags = new[] { "Roles" })]
    [ProducesResponseType(typeof(List<RoleDetails>), 200)]
    public async Task <List<RoleDetails>> GetAllRoles()
    {
      var roles = await _rolesService.GetAllRoles();
      return roles;
    }
  }
}

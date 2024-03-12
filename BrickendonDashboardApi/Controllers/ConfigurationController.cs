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
  [Route("users/config")]
  public class ConfigurationController
  {
    private readonly ILogger<ConfigurationController> _logger;
    private readonly IConfigurationService _configurationService;
    public ConfigurationController(ILogger<ConfigurationController> logger,IConfigurationService configurationService) 
    {
      _logger = logger;
      _configurationService = configurationService;
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
    ///     GET /users/config/roles
    ///
    /// </remarks>

    [HttpGet("roles")]
    [SwaggerOperation(Tags = new[] { "Roles" })]
    [ProducesResponseType(typeof(List<RoleDetails>), 200)]
    public async Task <List<RoleDetails>> GetAllRoles()
    {
      var roles = await _configurationService.GetAllRoles();
      return roles;
    }
  }
}

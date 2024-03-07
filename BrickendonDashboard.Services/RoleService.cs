using BrickendonDashboard.Domain.Contracts;
using BrickendonDashboard.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Services
{
  public class RoleService : IRolesService
  {
    private IDataContext _dataContext;
    public RoleService(IDataContext dataContext)
    {
      _dataContext = dataContext;

    }

    public async Task<List<RoleDetails>> GetAllRoles()
    {
      var roles = await _dataContext.Role
        .Where(r => r.IsDeleted == false)
        .Select(r => new RoleDetails
        {
          RoleId = r.Id,
          RoleName = r.Name,
          RoleKey = r.Key,

        }).ToListAsync();
      return roles;
    }
  }
}

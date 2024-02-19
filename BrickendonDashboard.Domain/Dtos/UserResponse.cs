using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Domain.Dtos
{
  public class UserResponseInfo
  {
    public int UserId { get; set; }
    public string? Email { get; set; }
  }

  public class UserRequestInfo
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MobileNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public int UserTypeId { get; set; }
    public List<int> RoleIds { get; set; }
  }

  public class UserEditRequestInfo
  {
    public UserRequestInfo userRequestInfo { get; set; }

    public bool IsActive { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Domain.Dtos
{
  public class UserListResponseInfo :PaginationInfo
  {
    public List<UserDto> UserList { get; set; }
  }

  public class UserDto
  {
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string ContactNumber { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public int UserType { get; set; }
    public bool IsActive { get; set; }
		public List<int> RoleIds { get; set; }
	}

 
}

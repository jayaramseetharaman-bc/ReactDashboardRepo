using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Domain.Dtos
{
  public class UserListFilterCriteria : BaseFilterCriteria
  {
		[FromQuery]
		public List<int>? Roles { get; set; }
    [FromQuery]
    public bool? Status { get; set; }
	}
}

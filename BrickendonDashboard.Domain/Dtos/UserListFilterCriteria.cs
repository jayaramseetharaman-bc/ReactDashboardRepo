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
		public List<int>? SearchByRoles { get; set; }
    [FromQuery]
    public bool? SearchByStatus { get; set; }
	}
}

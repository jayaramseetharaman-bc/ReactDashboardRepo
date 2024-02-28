using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Domain.Dtos
{
  public class UserListFilterCriteria : BaseFilterCriteria
  {
		public List<int>? SearchByRoles { get; set; } 
		public bool? IsActiveFilter { get; set; }
	}
}

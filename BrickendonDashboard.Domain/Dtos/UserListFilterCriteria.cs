using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Domain.Dtos
{
  public class UserListFilterCriteria
  {
    public string? SearchKeyword { get; set; } = null;

    public string? SortBy { get; set; } = "userName";

    public string? SortOrder { get; set; } = "ASC";

    public int PageIndex { get; set; }

    public int PageSize { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Domain.Dtos
{
  public class BaseFilterCriteria
  {
    public string? SearchKeyword { get; set; } = null;

    public string? SortBy { get; set; } = null;

    public string? SortOrder { get; set; } = "ASC";

    public int PageIndex { get; set; }

    public int PageSize { get; set; }
  }
}

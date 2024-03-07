using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Domain.Dtos
{
  public class ResultSetCriteria
  {
    [JsonProperty(PropertyName = "page-size")]
    public int PageSize { get; set; }

    [JsonProperty(PropertyName = "current-page")]
    public int CurrentPage { get; set; }

    [JsonProperty(PropertyName = "is-pagination")]
    public bool IsPagination { get; set; } = true;
  }
}

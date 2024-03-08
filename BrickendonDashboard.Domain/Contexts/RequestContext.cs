using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Domain.Contexts
{
  public class RequestContext
  {
    public int UserId { get; set; } = 1;
    public string ApiKey { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Domain.Contexts
{
  public class RequestContext
  {
    public int UserId { get; set; } = 0;
    public string ApiKey { get; set; } 
    public string UserName { get; set; }
  }
}

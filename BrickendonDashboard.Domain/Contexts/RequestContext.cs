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

    public string UserEmail { get; set; }

    public string UserName { get; set; }
    public string IpAddress { get; set; }

    public string Device { get; set; }

    public List<string> Roles { get; set; }

    public string RequestIntendedOrganisationId { get; set; }

    public string RequestIntendedUserName { get; set; }

    public string apiKey { get; set; }
  }
}

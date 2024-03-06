using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Domain.Dtos
{
  public class ApplicationConfigurationInfo
  {
    public string ApiKey { get; set; }
    public JwtTokenValidationConfigurationInfo JwtTokenValidationInfo { get; set; }
    public class JwtTokenValidationConfigurationInfo
    {
      public string Audience { get; set; }

      public string Issuer { get; set; }

      public string JwksUrl { get; set; }
    }

  }
}

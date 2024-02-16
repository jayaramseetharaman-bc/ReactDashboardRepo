using BrickendonDashboard.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Shared.Services
{
  public class DateTimeService : IDateTimeService
  {
    public DateTime GetUTCNow()
    {
      return DateTime.UtcNow;
    }
  }
}

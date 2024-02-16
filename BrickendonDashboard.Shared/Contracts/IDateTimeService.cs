using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Shared.Contracts
{
    public interface IDateTimeService
    {
      DateTime GetUTCNow();
    }
}


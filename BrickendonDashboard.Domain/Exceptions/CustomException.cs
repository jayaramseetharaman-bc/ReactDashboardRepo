using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Domain.Exceptions
{
  public class CustomException : Exception
  {
    public CustomException()
            : base()
    {
    }

    public CustomException(string errorCode)
        : base(errorCode)
    {
    }
  }
}

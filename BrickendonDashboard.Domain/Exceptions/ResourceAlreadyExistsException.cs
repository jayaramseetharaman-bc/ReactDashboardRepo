using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Domain.Exceptions
{
  public class ResourceAlreadyExistsException : Exception
  {
    public ResourceAlreadyExistsException()
        : base()
    {
    }

    public ResourceAlreadyExistsException(string message)
        : base(message)
    {
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.DBModel.Entities
{
  public class BaseEntity
  {
    public DateTime CreatedOnUtc { get; set; }

    public DateTime LastUpdatedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
  }
}

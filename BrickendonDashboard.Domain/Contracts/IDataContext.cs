using BrickendonDashboard.DBModel.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Domain.Contracts
{
  public interface IDataContext
  {
    public DbSet<User> User { get; set; }

    public DbSet<Roles> Roles { get; set; }

    public DbSet<UserRoles> UserRoles { get; set; }

    public DbSet<UserType> UserType { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

  }
}

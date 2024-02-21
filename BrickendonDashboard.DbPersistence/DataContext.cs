using BrickendonDashboard.DBModel.Entities;
using BrickendonDashboard.Domain.Contexts;
using BrickendonDashboard.Domain.Contracts;
using BrickendonDashboard.Shared.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;

namespace BrickendonDashboard.DbPersistence
{

    public class DataContext : DbContext, IDataContext
    {
       private readonly RequestContext _requestContext;
      private readonly IDateTimeService _dateTimeService;
      public DataContext(DbContextOptions<DataContext> options, IDateTimeService dateTimeService,RequestContext requestContext)
              : base(options)
      {
        _requestContext = requestContext;
        _dateTimeService = dateTimeService;
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
        modelBuilder
            .Entity<User>()
            .HasKey(x => x.Id);
        modelBuilder
            .Entity<User>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
      }

      //public async Task<PagedResultSet<T>> GetPagedResultAsync<T>(IQueryable<T> query, ResultSetCriteria resultSetCriteria)
      //{
      //  var currentPage = resultSetCriteria.CurrentPage;
      //  var pageSize = resultSetCriteria.PageSize;

      //  var result = new PagedResultSet<T>
      //  {
      //    CurrentPage = currentPage,
      //    RowCount = await query.CountAsync()
      //  };
      //  var pageCount = (double)result.RowCount / pageSize;
      //  result.PageCount = (int)Math.Ceiling(pageCount);
      //  var skip = (currentPage - 1) * pageSize;
      //  result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();
      //  return result;
      //}

      public DbSet<User> User { get; set; }

      public DbSet<Role> Role { get; set; }

      public DbSet<UserRole> UserRole { get; set; }

      public DbSet<UserType> UserType { get; set; }


      public async override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
      {
        ValidateEntities();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
      }

      public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
      {
        ValidateEntities();
        return await base.SaveChangesAsync(cancellationToken);
      }

      public override int SaveChanges(bool acceptAllChangesOnSuccess)
      {
        ValidateEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
      }

      public override int SaveChanges()
      {
        ValidateEntities();
        return base.SaveChanges();
      }

      private void ValidateEntities()
      {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>().Where(e => new[] { EntityState.Added, EntityState.Modified }.Contains(e.State)))
        {
          if (entry.State == EntityState.Added)
          {
            entry.Entity.CreatedOnUtc = entry.Entity.LastUpdatedOnUtc = _dateTimeService.GetUTCNow();
             entry.Entity.CreatedBy = entry.Entity.LastUpdatedBy = _requestContext.UserId;
          }
          else
          {
            entry.Entity.LastUpdatedOnUtc = _dateTimeService.GetUTCNow();
            entry.Entity.LastUpdatedBy = _requestContext.UserId;
          }
        }
      }
    }

  }

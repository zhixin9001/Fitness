﻿using Fit.Service.Entities.RBAC;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fit.Service
{
  public class FitDbContext : DbContext
  {
    private ILog log = LogManager.GetLogger(typeof(FitDbContext));
    public FitDbContext() : base("name=connStr")
    {
      //Database.SetInitializer<FitDbContext>(null);

      this.Database.Log = (sql) => { log.DebugFormat("EF-SQL: {0}", sql); };
    }

    //This function is import which maps the entity with it's modelconfig
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<PermissionEntity> Permissions { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<AdminUserEntity> AdminUsers { get; set; }
  }
}

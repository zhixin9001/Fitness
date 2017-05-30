﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit.Service.Entities.RBAC
{
  public class PermissionEntity : BaseEntity
  {
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<RoleEntity> Roles { get; set; }
      = new List<RoleEntity>();
  }
}

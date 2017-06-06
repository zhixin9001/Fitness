﻿using Fit.AdminWeb.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fit.AdminWeb
{
  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      log4net.Config.XmlConfigurator.Configure();
      AreaRegistration.RegisterAllAreas();
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      FilterConfig.RegisterConfig(GlobalFilters.Filters);
      AutofacConfig.Config();
    }
  }
}

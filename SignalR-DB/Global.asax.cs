using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Owin;
using SignalR_DB.Models;

namespace SignalR_DB {
  public class Global : HttpApplication {
    private readonly StudentDbEntities _context = new StudentDbEntities();

    protected void Application_Start(object sender, EventArgs e) {
      // Code that runs on application startup
      AreaRegistration.RegisterAllAreas();
      GlobalConfiguration.Configure(WebApiConfig.Register);
      RouteConfig.RegisterRoutes(RouteTable.Routes);

      //Start SqlDependency with application initialization
      SqlDependency.Start(_context.Database.Connection.ConnectionString);
    }

    protected void Application_End() {
      //Stop SqlDependency
      SqlDependency.Stop(_context.Database.Connection.ConnectionString);
    }
  }
}
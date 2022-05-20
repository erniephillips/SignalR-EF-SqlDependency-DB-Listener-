using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartupAttribute(typeof(SignalR_DB.Startup))]
namespace SignalR_DB {
  public class Startup {
    public void Configuration(IAppBuilder app) {
      app.MapSignalR();
    }
  }
}
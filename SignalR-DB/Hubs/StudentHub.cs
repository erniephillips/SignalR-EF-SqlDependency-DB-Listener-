using EFChangeNotify;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalR_DB.Models;
using SignalR_DB.Repos;
using System;
using System.Linq;
using System.Threading;

//To use EF for SQL Dependency
//https://www.codeproject.com/Articles/496484/SqlDependency-with-Entity-Framework-5-0
//https://github.com/rickbassham/ef-change-notify

namespace SignalR_DB.Hubs {
  public class StudentHub : Hub {
    [HubMethodName("getStudentRecords")]
    public void GetStudentRecords() {
      //run an initial pull of the notifications, the signalr broadcase is then handled in the onchange listener in the entitychangenotifier for all db updates
      //using (var cache = new EntityCache<Student, StudentDbEntities>(p => p.StudentName == "Ernie")) {
      using (var cache = new EntityCache<Student, StudentDbEntities>(x => x.StudentID > 0)) {
        //while (true) {
        IHubContext context = GlobalHost.ConnectionManager.GetHubContext<StudentHub>();
        context.Clients.All.showTheStudentList(cache.Results.ToList());
        //  Thread.Sleep(1000);
        //}
      }

      //this only runs once and does not pull from cached memory like the above memory
      //using (var notifer = new EntityChangeNotifier<Student, StudentDbEntities>(p => p.StudentName == "Ernie")) {
      //  notifer.Error += (sender, e) => {
      //    Console.WriteLine("[{0}, {1}, {2}]:\n{3}", e.Reason.Info, e.Reason.Source, e.Reason.Type, e.Sql);
      //  };
        
      //  notifer.Changed += (sender, e) => {
      //    IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<StudentHub>();
      //    _context.Clients.All.showTheStudentList(e.Results.ToList());
      //  };
      //}
    }
  }
}
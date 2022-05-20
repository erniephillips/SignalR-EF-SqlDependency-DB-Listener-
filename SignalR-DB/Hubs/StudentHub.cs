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
      //IHubContext context = GlobalHost.ConnectionManager.GetHubContext<StudentHub>();
      //context.Clients.All.showTheStudentList(StudentRepository.GetStudentRecords().ToList());

      //initial pull of any info
      using (var cache = new EntityCache<Student, StudentDbEntities>(p => p.StudentName == "Ernie")) {
        while (true) {
          IHubContext context = GlobalHost.ConnectionManager.GetHubContext<StudentHub>();
          context.Clients.All.showTheStudentList(cache.Results.ToList());
          Thread.Sleep(1000);
        }
      }

      //set up a listener for the database which will update the broadcast
      //using (var notifer = new EntityChangeNotifier<Student, StudentDbEntities>(p => p.StudentName == "Ernie")) {
      //  notifer.Error += (sender, e) => {
      //    Console.WriteLine("[{0}, {1}, {2}]:\n{3}", e.Reason.Info, e.Reason.Source, e.Reason.Type, e.Sql);
      //  };
        
      //  notifer.Changed += (sender, e) => {
      //    IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<StudentHub>();
      //    _context.Clients.All.showTheStudentList(e.Results.ToList());
      //  };

      //  IHubContext context = GlobalHost.ConnectionManager.GetHubContext<StudentHub>();
      //  context.Clients.All.showTheStudentList(notifer.Results.ToList());
      //}


      //using (var otherNotifier = new EntityChangeNotifier<Student, StudentDbEntities>(p => p.StudentName == "Ernie")) {
      //  otherNotifier.Changed += (sender, e) => {
      //    Console.WriteLine(e.Results.Count());
      //  };

      //  Console.WriteLine("Press any key to stop listening for changes...");
      //  Console.ReadKey(true);
      //}

    }

  }
}
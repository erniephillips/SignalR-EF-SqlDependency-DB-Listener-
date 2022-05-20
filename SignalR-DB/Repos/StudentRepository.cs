using SignalR_DB.Hubs;
using SignalR_DB.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SignalR_DB.Repos {
  public class StudentRepository {
    static readonly StudentDbEntities _context = new StudentDbEntities();

    public static List<Student> GetStudentRecords() {
      var lstStudentRecords = new List<Student>();
      string dbConnectionSettings = _context.Database.Connection.ConnectionString;

      using (var dbConnection = new SqlConnection(dbConnectionSettings)) {
        dbConnection.Open();

        var sqlCommandText = @"SELECT [StudentID],[StudentName],[DOB],[Weight] FROM [dbo].[Student]";

        using (var sqlCommand = new SqlCommand(sqlCommandText, dbConnection)) {
          AddSQLDependency(sqlCommand);

          if (dbConnection.State == ConnectionState.Closed)
            dbConnection.Open();

          var reader = sqlCommand.ExecuteReader();
          lstStudentRecords = GetStudentRecords(reader);
        }
      }
      return lstStudentRecords;
    }

    /// <summary>
    /// Adds SQLDependency for change notification and passes the information to Student Hub for broadcasting
    /// </summary>
    /// <param name="sqlCommand"></param>
    private static void AddSQLDependency(SqlCommand sqlCommand) {
      sqlCommand.Notification = null;

      var dependency = new SqlDependency(sqlCommand);

      dependency.OnChange += (sender, sqlNotificationEvents) => {
        if (sqlNotificationEvents.Type == SqlNotificationType.Change) {
          StudentHub studentHub = new StudentHub();
          studentHub.GetStudentRecords();
        }
      };
    }

    /// <summary>
    /// Fills the Student Records
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    private static List<Student> GetStudentRecords(SqlDataReader reader) {
      var lstStudentRecords = new List<Student>();
      var dt = new DataTable();
      dt.Load(reader);
      dt
          .AsEnumerable()
          .ToList()
          .ForEach
          (
              i => lstStudentRecords.Add(new Student() {
                StudentID = (int)i["StudentID"]
                      , StudentName = (string)i["StudentName"]
                      , DOB = Convert.ToDateTime(i["DOB"])
                      , Weight = (int)i["Weight"]
              })
          );
      return lstStudentRecords;
    }
  }
}
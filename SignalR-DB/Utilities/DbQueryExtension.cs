﻿using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace EFChangeNotify {
  public static class DbQueryExtension {
    public static ObjectQuery<T> ToObjectQuery<T>(this DbQuery<T> query) {
      //var internalQuery = query.GetType()
      //    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
      //    .Where(field => field.Name == "_internalQuery")
      //    .Select(field => field.GetValue(query))
      //    .First();

      var internalQuery = query.GetType()
          .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
          .Where(field => field.Name == "_internalQuery")
          .Select(field => field.GetValue(query))
          .First();


      var internalQueryType = internalQuery.GetType();
      var fields = internalQueryType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
      var fieldsWhere = fields.Where(field => field.Name == "_objectQuery");
      var fieldsSelect = fieldsWhere.Select(field => field.GetValue(internalQuery));
      var fieldCast = fieldsSelect.Cast<ObjectQuery<T>>();
      var first = fieldCast.First();

      //var objectQuery = internalQuery.GetType()
      //    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
      //    .Where(field => field.Name == "_objectQuery")
      //    .Select(field => field.GetValue(internalQuery))
      //    .Cast<ObjectQuery<T>>()
      //    .First();

      return first;
    }

    public static SqlCommand ToSqlCommand<T>(this DbQuery<T> query) {
      SqlCommand command = new SqlCommand();

      command.CommandText = query.ToString();

      var objectQuery = query.ToObjectQuery();

      foreach (var param in objectQuery.Parameters) {
        command.Parameters.AddWithValue(param.Name, param.Value);
      }

      return command;
    }

    public static string ToTraceString<T>(this DbQuery<T> query) {
      var objectQuery = query.ToObjectQuery();

      return objectQuery.ToTraceStringWithParameters();
    }

    public static string ToTraceStringWithParameters<T>(this ObjectQuery<T> query) {
      string traceString = query.ToTraceString() + "\n";

      foreach (var parameter in query.Parameters) {
        traceString += parameter.Name + " [" + parameter.ParameterType.FullName + "] = " + parameter.Value + "\n";
      }

      return traceString;
    }
  }
}

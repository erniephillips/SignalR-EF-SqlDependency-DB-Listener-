using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace SignalR_DB.Models {
  public partial class StudentDbEntities : DbContext {
    public StudentDbEntities()
        : base("name=StudentDbEntities") {
    }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      modelBuilder.Entity<Student>()
          .Property(e => e.StudentName)
          .IsUnicode(false);
    }
  }
}

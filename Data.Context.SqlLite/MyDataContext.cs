using Data.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Context.SqlLite
{
    public class MyDataContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=TestMyContext.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Student>().HasData(new { Name = "Carol", Id = 1 });
        }
    }
}

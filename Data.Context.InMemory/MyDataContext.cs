using Data.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Context.InMemory
{
    public class MyDataContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        public MyDataContext() : base()
        {
            MyDataContextSeeder.Seed(this);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Filename=:memory:");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Student>().HasData(new { Name = "Carol", Id = 1 });
        }
    }
}

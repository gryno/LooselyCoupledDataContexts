using Data.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Context.SqlServer
{
	public class MyDataContext : DbContext
	{
		public DbSet<Student> Students { get; set; }
		public DbSet<Course> Courses { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=04a-LooseCoupling;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		}
	}
}

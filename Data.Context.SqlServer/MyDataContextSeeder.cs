using Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Context.SqlServer
{
	public static class MyDataContextSeeder
	{
		public static void Seed(MyDataContext context)
		{
			// Console.WriteLine("seeding database - started.");
			context.Database.EnsureDeleted();
			// Console.WriteLine("seeding database - ensure deleted.");
			// Console.ReadLine();
			context.Database.EnsureCreated();
			// Console.WriteLine("seeding database - ensure created.");

			context.Students.Add(new Student()
			{
				Name = "Alice"
			});

			context.Students.Add(new Student()
			{
				Name = "Bob"
			});

			context.Students.Add(new Student()
			{
				Name = "Chuck"
			});

			context.SaveChanges();
			// Console.WriteLine("seeding database - finished.");
		}
	}
}

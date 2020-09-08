using Data.Context.SqlLite;
using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _04_CreateSqlLiteData
{
    class Program
    {
        static void Main(string[] args)
        {
			Console.WriteLine("SqlLite - creates and populates database");

			using (var context = new MyDataContext())
			{
				MyDataContextSeeder.Seed(context);

				List<Student> sList = context.Students.ToList();

				sList.ForEach(s => Console.WriteLine("\tStudent Name: {0}", s.Name));
			}

			Console.Write("Press any key to exit.");
			Console.ReadLine();
		}
	}
}

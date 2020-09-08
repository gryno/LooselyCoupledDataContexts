using Data.Model;
using Data.Context.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _04_CreateInMemoryData
{
    class Program
    {
        static void Main(string[] args)
        {
			Console.WriteLine("InMemory - creates and populates database");

			using (var context = new MyDataContext())
			{
				// Not needed, DbContext Ctor calls it - MyDataContextSeeder.Seed(context);

				List<Student> sList = context.Students.ToList();

				sList.ForEach(s => Console.WriteLine("\tStudent Name: {0}", s.Name));
			}

			Console.Write("Press any key to exit.");
			Console.ReadLine();
		}
	}
}

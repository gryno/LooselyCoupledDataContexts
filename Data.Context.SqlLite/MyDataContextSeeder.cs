﻿using Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Context.SqlLite
{
    public static class MyDataContextSeeder
    {
        public static void Seed(MyDataContext context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Students.Add(new Student()
            {
                Name = "Chad"
            });

            context.Students.Add(new Student()
            {
                Name = "Doug"
            });

            context.SaveChanges();
        }
    }
}

using Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LooselyCoupled
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				// Get config Info, parse to BOs
				IConfiguration cfg = GetConfig();

				List<MetadataModel> libMetadata = GetFromCfg_ArrayOfAvailableDbContexts(cfg);

				string libNameToUse = GetFromCfg_NameOfLibraryToUseForDbContext(cfg);

				MetadataModel currContextMetadata = libMetadata
					.Where(lib => lib.LibraryName.ToLower() == libNameToUse.ToLower()).FirstOrDefault();


				// Find and load DbContext assembly
				Assembly b = Assembly.GetEntryAssembly();
				string[] sNames = b.GetManifestResourceNames();

				Assembly a = Assembly.LoadFrom(currContextMetadata.AssemblyPathAndName);
				Type[] allTypesInAssembly = a.GetTypes();

				Type myDbContextAssemblyInfo = allTypesInAssembly
					.Where(r => r.FullName == currContextMetadata.ClassName)
					.FirstOrDefault();

				if (myDbContextAssemblyInfo == null)
				{
					string msg = string.Format(
						"The host specified class name [{0}] could not be found in the assembly.",
						currContextMetadata.ClassName);
					throw new Exception(msg);
				}

				DbContext dContext = Activator.CreateInstance(myDbContextAssemblyInfo) as DbContext;


				// Look onto the DbContext for specific table(s) whose content is to be echoed.
				DbSet<Student> sList = dContext.Set<Student>();
				List<Student> stList = sList.ToList();

				Console.WriteLine("\nStudent table contains {0} records.", sList.Count());
				stList.ForEach(s => Console.WriteLine("\tStuden Name: {0}", s.Name));
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception encountered:\n\t[{0}]\n\t[{1}]", ex.Message, ex.StackTrace);
			}

			Console.Write("\nPress any key to close app."); Console.ReadLine();
		}

		public static IConfiguration GetConfig()
		{
			ConfigurationBuilder cBldr = new ConfigurationBuilder();
			cBldr
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("hostsettings.json", optional: true, reloadOnChange: true);

			IConfigurationRoot cfg = cBldr.Build();

			return cfg;
		}

		private static void EchoConfigurationMetadata(string[] metadataKey, List<MetadataModel> libMetadata)
		{
			Console.WriteLine("Echo of read information:");
			libMetadata.ForEach(c =>
			{
				Console.WriteLine("\tLibrary Name (key): [{0}]", c.LibraryName);
				Console.WriteLine("\tClass Name: [{0}]", c.ClassName);
				Console.WriteLine("\tAssembly Path and Name: [{0}]", c.AssemblyPathAndName);
			});

			MetadataModel currContextMetadata = libMetadata
				.Where(lib => lib.LibraryName.ToLower() == metadataKey[0].ToLower()).FirstOrDefault();

			if (libMetadata == null || libMetadata.Count == 0)
			{
				string msg = string.Format("LibMetadata not parsed correctly [is null = {0}]",
					libMetadata == null ? "true" : "false");
				throw new ApplicationException(msg);
			}
			if (currContextMetadata == null)
			{
				string msg = string.Format("Not able to find library key [{0}] in array.", metadataKey[0]);
				throw new ApplicationException(msg);
			}

			string assemblyFullName = currContextMetadata.AssemblyPathAndName;
			string namespaceAndClassname = currContextMetadata.ClassName;

			Console.WriteLine("Number of array elements read from json file: {0}", libMetadata.Count);
			Console.WriteLine("Selected Resource (key: [{0}]):\n\tLibrary Name: [{1}]\n\tClass Name: [{2}]",
				 metadataKey[0], currContextMetadata.LibraryName, currContextMetadata.ClassName);
		}

		public static List<MetadataModel> GetFromCfg_ArrayOfAvailableDbContexts(IConfiguration cfg)
		{
			List<MetadataModel> retList = new List<MetadataModel>();

			cfg.GetSection("MetadataModelResources").Bind(retList);

			return retList;
		}

		public static string GetFromCfg_NameOfLibraryToUseForDbContext(IConfiguration cfg)
		{
			string libName = string.Empty;

			libName = cfg.GetSection("LibraryToUse").Value;


			OrganicMaterial om = cfg.ToString().Length > 3 ? 
				new Cat() as OrganicMaterial : 
				new Coal() as OrganicMaterial;


			return libName;
		}
	}

	public abstract class OrganicMaterial
	{
		public int CarbonContent { get; set; }
		public bool IsAlive { get; protected set; }

		public abstract string WhatObjetNameAmI { get; }
	}

	public class Animal : OrganicMaterial
	{
		public bool HasLegs { get => NumberOfLegs > 0; }
		public int NumberOfLegs { get; private set; }
		public Animal(int numOfLegs = 0): base()
		{
			IsAlive = true;
			NumberOfLegs = NumberOfLegs;
		}

		public override string WhatObjetNameAmI { 
			get => "Some Animal"; 
		}

		
	}

	public class Cat : Animal
	{
		public bool IsFerrel { get; set; }
		public Cat() : base (4)
		{

		}
	}

	public class Snake: Animal
	{
		public Snake() : base (0)
		{

		}
	}

	public class Human : Animal
	{
		public Human() : base (2)
		{
				
		}
	}

	public class Vegetible	: OrganicMaterial
	{
		public bool DoesPhotosynthesis { get; set; }

		public override string WhatObjetNameAmI => GetWhatTypeIAm();

		public virtual string GetWhatTypeIAm()
		{
			return "Some Vegetible";
		}

		public Vegetible(bool doesPS) : base()
		{
			IsAlive = true;
			DoesPhotosynthesis = doesPS;
		}
	}

	public class Mushroom : Vegetible
	{
		public Mushroom() : base(false)
		{

		}

		public override string GetWhatTypeIAm()
		{
			return "Mushroom";
		}
	}

	public class Tree : Vegetible
	{
		public Tree() :base (true)
		{

		}
	}

	public class Coal : OrganicMaterial
	{
		public Coal() : base()
		{
			IsAlive = false;
		}

		public override string WhatObjetNameAmI { 
			get => "Coal"; }
	}

}

using Jil;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace SRBG
{
	class Program
	{
		public static string modName = "asd";

		static void Main(string[] args)
		{
			JSON.SetDefaultOptions(Options.ExcludeNulls);

			Console.WriteLine(Strings.INTRO);
			Console.ReadKey();
			Console.Clear();


			try
			{ Directory.GetAccessControl(AppDomain.CurrentDomain.BaseDirectory); }
			catch (UnauthorizedAccessException)
			{
				Console.WriteLine(Strings.NO_ACCESS);
				Console.ReadKey();
				return;
			}

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + Strings.OUTPUT_DIRECTORY_NAME))
				Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + Strings.OUTPUT_DIRECTORY_NAME);

			GenerateFiles(AppDomain.CurrentDomain.BaseDirectory + @"..\");

			if (Unlocks.unlockCount > 0)
			{
				Unlocks.WriteToFile();
				Console.WriteLine(Strings.SUCCESS_UNLOCKS);
			}
			else
				Console.WriteLine(Strings.FAILED_UNLOCKS);

			if (Recipes.totalRecipes > 0)
			{
				Recipes.WriteToFile();
				Groups.WriteToFile();
				Console.WriteLine(Strings.SUCCESS_RECIPE);
			}
			else
				Console.WriteLine(Strings.FAILED_RECIPES);

			stopwatch.Stop();

			Console.WriteLine();
			Console.WriteLine(string.Format(Strings.STATS_ELAPSED_TIME, stopwatch.Elapsed));
			Console.WriteLine(string.Format(Strings.STATS_UNLOCKS_SCANNED, Unlocks.scannedFiles));
			Console.WriteLine(string.Format(Strings.STATS_TOTAL_UNLOCKS, Unlocks.unlockCount));
			Console.WriteLine(string.Format(Strings.STATS_RECIPES_SCANNED, Recipes.scannedFiles));
			Console.WriteLine(string.Format(Strings.STATS_TOTAL_RECIPES, Recipes.totalRecipes));
			Console.WriteLine();

			Console.WriteLine(Strings.EXIT);
			Console.ReadKey();
		}

		static void GenerateFiles(string currentDir)
		{
			string[] files = Directory.GetFiles(currentDir);
			string[] directories = Directory.GetDirectories(currentDir);

			for (int i = 0; i < files.Length; i++)
			{
				string ext = Path.GetExtension(files[i]);
				if (ext.Equals(Strings.RECIPE_FILE_EXTENSION))
				{
					Console.WriteLine(Strings.READING_RECIPE);
					Console.WriteLine(files[i]);

					string str = RemoveComments(File.ReadAllText(files[i]));
					Recipe toAdd = JSON.Deserialize<Recipe>(str);
					toAdd.FixBullshit();
					Recipes.Add(toAdd);
				}
				else if (Strings.ITEM_FILE_EXTENSIONS.Contains(ext))
				{
					Console.WriteLine(Strings.READING_UNLOCKS);
					Console.WriteLine(files[i]);

					string toAdd = RemoveComments(File.ReadAllText(files[i]));
					Unlocks.Add(JSON.Deserialize<Item>(toAdd, Options.PrettyPrintExcludeNulls));
				}
			}

			for (int i = 0; i < directories.Length; i++)
			{ GenerateFiles(directories[i]); }
		}

		// Since JSON does not inherently support comments, I have to remove them myself.
		static string RemoveComments(string str)
		{ return Regex.Replace(str, Strings.UNCOMMENT_REGEX_PATTERN, "", RegexOptions.Singleline); }
	}
}
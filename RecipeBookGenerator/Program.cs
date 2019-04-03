using Jil;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

//				TO DO:
// -	Find out why some unlocks are generating with a null
// -	Search results should utilize log(n), by always cutting the search list by half until a matching result is found
//		Once found, iterate backwards until a non-matching result is found, and then iterate forward from the hit onwards until a non-matching is found
//		Insert first list in a reverse order, and then the second list normally
//		(Or if possible, just insert predecesors to start of list, and followers to end, then iterate normally

namespace SRBG
{
	class Program
	{
		public static string modName = "asd";

		static void Main(string[] args)
		{
			JSON.SetDefaultOptions(Options.ExcludeNulls);
			Console.Title = Strings.TITLE;

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

			Console.WriteLine();

			if (Recipes.totalRecipes > 0)
			{
				Recipes.WriteToFile();
				Groups.WriteToFile();
				Console.WriteLine(Strings.SUCCESS_RECIPES);
			}
			else
				Console.WriteLine(Strings.FAILED_RECIPES);

			if (Unlocks.unlockCount > 0)
			{
				Unlocks.WriteToFile();
				Console.WriteLine(Strings.SUCCESS_UNLOCKS);
			}
			else
				Console.WriteLine(Strings.FAILED_UNLOCKS);
			
			if (ItemNameLibrary.internalNamesIndexed > 0)
			{
				ItemNameLibrary.WriteToFile();
				Console.WriteLine(Strings.SUCCESS_NAMES);
			}
			else
				Console.WriteLine(Strings.FAILED_NAMES);


			Console.WriteLine(Strings.LINE_BREAK);
			Console.WriteLine(string.Format(Strings.STATS_ELAPSED_TIME, stopwatch.Elapsed));
			Console.WriteLine(Strings.LINE_BREAK);
			Console.WriteLine(string.Format(Strings.STATS_UNLOCKS_SCANNED, Unlocks.scannedFiles));
			Console.WriteLine(string.Format(Strings.STATS_TOTAL_UNLOCKS, Unlocks.unlockCount));
			Console.WriteLine(string.Format(Strings.STATS_TOTAL_NAMES_INTERNAL, ItemNameLibrary.internalNamesIndexed));
			Console.WriteLine(string.Format(Strings.STATS_TOTAL_NAMES_DISPLAY, ItemNameLibrary.displayNamesIndexed));
			Console.WriteLine(Strings.LINE_BREAK);
			Console.WriteLine(string.Format(Strings.STATS_RECIPES_SCANNED, Recipes.scannedFiles));
			Console.WriteLine(string.Format(Strings.STATS_TOTAL_RECIPES, Recipes.totalRecipes));
			Console.WriteLine(string.Format(Strings.STATS_TOTAL_GROUPS, Groups.totalGroups));
			Console.WriteLine();

			Console.WriteLine(Strings.EXIT);
			Console.ReadKey();
			Process.Start(AppDomain.CurrentDomain.BaseDirectory + Strings.OUTPUT_DIRECTORY_NAME);
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
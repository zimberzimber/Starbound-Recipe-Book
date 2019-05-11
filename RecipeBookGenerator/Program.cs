using Jil;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

//				TO DO:
// -	Search results should utilize log(n), by always cutting the search list by half until a matching result is found
//		Once found, iterate backwards until a non-matching result is found, and then iterate forward from the hit onwards until a non-matching is found
//		Insert first list in a reverse order, and then the second list normally
//		(Or if possible, just insert predecesors to start of list, and followers to end, then iterate normally

namespace SRBG
{
    partial class Program
    {
        static void Main()
        {
            JSON.SetDefaultOptions(Options.ExcludeNulls);
            Console.Title = Strings.TITLE;

            Console.WriteLine(Strings.INTRO);
            string dir = Console.ReadLine();
            Console.Clear();

            try
            { Directory.GetAccessControl(AppDomain.CurrentDomain.BaseDirectory); }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine(Strings.NO_ACCESS);
                Console.ReadKey();
                return;
            }

            while (!Directory.Exists(dir))
            {
                Console.WriteLine(Strings.INTRO);
                Console.WriteLine();
                Console.WriteLine(string.Format(Strings.DIRECTORY_INVALID, dir));
                dir = Console.ReadLine();
                Console.Clear();
            }

            Strings.modName = Path.GetFileName(dir);

            Console.WriteLine(Strings.SHOULD_USE_THREADING);
            char threadingInput = Console.ReadKey().KeyChar;

            Console.Clear();
            Console.WriteLine(Strings.INITIALIZING);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (!Directory.Exists($"{AppDomain.CurrentDomain.BaseDirectory}{Strings.OUTPUT_DIRECTORY_NAME}"))
                Directory.CreateDirectory($"{AppDomain.CurrentDomain.BaseDirectory}{Strings.OUTPUT_DIRECTORY_NAME}");

            if (!Directory.Exists($"{AppDomain.CurrentDomain.BaseDirectory}{Strings.OUTPUT_DIRECTORY_NAME}\\{Strings.modName}"))
                Directory.CreateDirectory($"{AppDomain.CurrentDomain.BaseDirectory}{Strings.OUTPUT_DIRECTORY_NAME}\\{Strings.modName}");

            if (threadingInput == 'y' || threadingInput == 'Y')
            {
                GenerateFilesMultithread(dir);
                while (threadWorkLeft > 0)
                    Thread.Sleep(100);
            }
            else
                GenerateFiles(dir);

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
            Process.Start(string.Format("{0}{1}\\{2}", AppDomain.CurrentDomain.BaseDirectory, Strings.OUTPUT_DIRECTORY_NAME, Strings.modName));
        }

        static void GenerateFiles(string dir)
        {
            string[] files = Directory.GetFiles(dir);
            string[] directories = Directory.GetDirectories(dir);

            for (int i = 0; i < directories.Length; i++)
            { GenerateFiles(directories[i]); }

            for (int i = 0; i < files.Length; i++)
            {
                string ext = Path.GetExtension(files[i]);
                if (ext.Equals(Strings.RECIPE_FILE_EXTENSION))
                {
                    Console.WriteLine(Strings.READING_RECIPE);
                    Console.WriteLine(files[i]);

                    string str = RemoveComments(File.ReadAllText(files[i]));
                    Recipe toAdd = JSON.Deserialize<Recipe>(str);
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
        }

        // Since JSON does not inherently support comments, I have to remove them myself.
        static string RemoveComments(string str)
        { return Regex.Replace(str, Strings.UNCOMMENT_REGEX_PATTERN, "", RegexOptions.Singleline); }
    }
}
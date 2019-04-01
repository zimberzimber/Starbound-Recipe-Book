using Jil;
using System;
using System.Collections.Generic;
using System.IO;

namespace SRBG
{
	static class Recipes
	{
		static Dictionary<string, List<Recipe>> _recipes = new Dictionary<string, List<Recipe>>(0);
		public static int totalRecipes = 0;
		public static int scannedFiles = 0;

		public static void Add(Recipe rec)
		{
			scannedFiles++;

			if (!_recipes.ContainsKey(rec.output.item))
				_recipes.Add(rec.output.item, new List<Recipe>(0));
			_recipes[rec.output.item].Add(rec);

			totalRecipes++;
			Groups.Add(rec.groups, rec.output.item);
		}

		public static void WriteToFile()
		{
			string str = JSON.Serialize(_recipes);
			using (StreamWriter file = new StreamWriter(string.Format("{0}{1}{2}", AppDomain.CurrentDomain.BaseDirectory, Strings.OUTPUT_DIRECTORY_NAME, Strings.RECIPE_BOOK_FILE_NAME)))
			{
				file.Write(Strings.PATCH_FILE_OPENING_LINE);
				file.Write(str);
				file.Write(Strings.PATCH_FILE_CLOSING_LINE);
			}
		}
	}

	class Recipe
	{
		public InputOutput[] input;
		public InputOutput output;
		public string[] groups;

		// See description for the method with the same name inside the "InputOutput" class
		public void FixBullshit()
		{
			for (int i = 0; i < input.Length; i++) { input[i].FixBullshit(); }
			output.FixBullshit();
		}
	}
	
	class InputOutput
	{
		public string name;
		public string item;
		public int count;

		// For whatever reason, Starbounds recipes accept "name" and "item" in the input/output fields...
		// And despite "item" being used way more often than "name", the game prioritizes the latter
		public void FixBullshit()
		{
			if (item == null)
			{
				item = name;
				name = null;
			}
		}
	}
}

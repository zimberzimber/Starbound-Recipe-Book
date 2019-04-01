namespace SRBG
{
	static class Strings
	{
		public const string RECIPE_FILE_EXTENSION = ".recipe";
		public const string PATCH_FILE_EXTENSION = ".patch";
		public static readonly string[] ITEM_FILE_EXTENSIONS = new string[] {
			".item", ".object", ".activeitem", ".back", ".chest", ".head", ".legs", ".augment", ".currency", ".consumable" };

		public const string PATCH_FILE_OPENING_LINE_NAME_LIBRARY = "[{\"op\":\"add\",\"path\":\"/{0}\",\"value\":";
		public const string PATCH_FILE_OPENING_LINE = "[{\"op\":\"add\",\"path\":\"/generated\",\"value\":";
		public const string PATCH_FILE_CLOSING_LINE = "]";

		public const string RECIPE_BOOK_FILE_NAME = "RecipeBook.patch";
		public const string GROUP_DICTIONARY_FILE_NAME = "GroupDictionary.patch";
		public const string UNLOCKS_DICTIONARY_FILE_NAME = "UnlocksDictionary.patch";
		public const string ITEM_NAME_LIBRARY_FILE_NAME = "ItemNameLibrary.patch";
		public const string OUTPUT_DIRECTORY_NAME = @"\output\";

		// Single line comment regex:		[/]+[/](.*?)[\n]
		// Multi-line comment regex:		[/]+[*](.*?)[*]+[/]
		public const string UNCOMMENT_REGEX_PATTERN = @"([/]+[/](.*?)[\n])|([/]+[*](.*?)[*]+[/])";

		public const string INTRO =
@"Welcome to SRBG (Starbound Recipe Book Generator)

This program will generate three files inside the 'output' folder at its directory:
- One holding all the recipes found in this and deeper directories
- One for tags and the items that have them.
- One for what is unlocked by what

Should you have any questions, feedback, or bugs to report, you can reach me at...
- Frackin' Universe Discord
- Starbound Discord
- Steam / GitHub: zimberzimber

Press any key to continue...";

		public const string NO_ACCESS =
@"Program does not have write access.
Please restart it in admin mode.";

		public const string READING_RECIPE = "Reading file for RECIPE + GROUPS...";
		public const string SUCCESS_RECIPE = "RecipeBook.txt and GroupDictionary.txt successfuly generated inside the 'output' folder";
		public const string FAILED_RECIPES = "No recipe files found. RecipeBook.txt and GroupDictionary.txt not generated.";

		public const string READING_UNLOCKS = "Reading file for UNLOCKS...";
		public const string SUCCESS_UNLOCKS = "UnlocksDictionary.txt successfuly generated inside the 'output' folder";
		public const string FAILED_UNLOCKS = "No items with unlocks on pickup found. File not generated.";

		public const string STATS_ELAPSED_TIME = "Elapsed time:\t\t\t{0}";
		public const string STATS_UNLOCKS_SCANNED = "Files scanned for unlocks:\t{0}";
		public const string STATS_TOTAL_UNLOCKS = "Total unlocks:\t\t\t{0}";
		public const string STATS_RECIPES_SCANNED = "Files scanned for recipes:\t{0}";
		public const string STATS_TOTAL_RECIPES = "Total recipes:\t\t\t{0}";

		public const string EXIT = "Press any key to quit...";
	}
}

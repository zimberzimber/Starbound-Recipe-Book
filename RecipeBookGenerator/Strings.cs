namespace SRBG
{
	static class Strings
	{
        public static string modName;

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



		// Used to remove comments from the files as JSON de/serializers do not inherently support commenting
		// Single line comment regex:		[/]+[/](.*?)[\n]
		// Multi-line comment regex:		[/]+[*](.*?)[*]+[/]
		public const string UNCOMMENT_REGEX_PATTERN = @"([/]+[/](.*?)[\n])|([/]+[*](.*?)[*]+[/])";

		// Used to remove formatting from item names for accurate search results
		public const string UNFORMAT_REGEX_PATTERN = @"[\^](.*?)[;]";



		public const string TITLE = "Starbound Recipe Book Generator";

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

Please insert a directory to generate recipes from...";

        public const string DIRECTORY_INVALID =
@"Invalid directory '{0}'";

		public const string NO_ACCESS =
@"Program does not have write access.
Please restart it in admin mode.";



		public const string READING_RECIPE = "Reading file for RECIPE + GROUPS...";
		public const string SUCCESS_RECIPES = "RecipeBook.patch and GroupDictionary.patch successfuly generated inside the 'output' folder";
		public const string FAILED_RECIPES = "No recipe files found. RecipeBook.txt and GroupDictionary.txt not generated.";

		public const string READING_UNLOCKS = "Reading file for UNLOCKS + ITEM NAMES...";
		public const string SUCCESS_UNLOCKS = "UnlocksDictionary.patch successfuly generated inside the 'output' folder";
		public const string FAILED_UNLOCKS = "No items with unlocks on pickup found. File not generated.";

		public const string SUCCESS_NAMES = "ItemNameLibrary.patch successfuly generated inside the 'output' folder";
		public const string FAILED_NAMES = "No item names found. ItemNameLibrary.patch not generated.";



		public const string STATS_ELAPSED_TIME = "Elapsed time:\t{0}";
		public const string LINE_BREAK = "____________________________________________________________";

		public const string STATS_UNLOCKS_SCANNED = "Files scanned for unlocks and item names:\t{0}";
		public const string STATS_TOTAL_UNLOCKS = "Total unlocks:\t\t{0}";
		public const string STATS_TOTAL_NAMES_INTERNAL = "Total internal names:\t{0}";
		public const string STATS_TOTAL_NAMES_DISPLAY = "Total display names:\t{0}";

		public const string STATS_RECIPES_SCANNED = "Files scanned for recipes and groups:\t\t{0}";
		public const string STATS_TOTAL_RECIPES = "Total recipes:\t\t{0}";
		public const string STATS_TOTAL_GROUPS = "Total groups:\t\t{0}";

		public const string EXIT = "Press any key to quit...";
	}
}

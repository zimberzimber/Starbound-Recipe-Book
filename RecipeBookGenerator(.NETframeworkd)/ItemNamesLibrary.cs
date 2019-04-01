using Jil;
using System;
using System.Collections.Generic;
using System.IO;

namespace SRBG
{
	static class ItemNamesLibrary
	{
		static Dictionary<string, List<string>> _nameLibrary = new Dictionary<string, List<string>>(0);

		public static void AddNameEntry(string displayedName, string internalName)
		{
			if (!_nameLibrary.ContainsKey(displayedName))
				_nameLibrary.Add(displayedName, new List<string>(0));
			_nameLibrary[displayedName].Add(internalName);
		}

		public static void WriteToFile()
		{
			string str = JSON.Serialize(_nameLibrary);
			using (StreamWriter file = new StreamWriter(string.Format("{0}{1}{2}", AppDomain.CurrentDomain.BaseDirectory, Strings.OUTPUT_DIRECTORY_NAME, Strings.ITEM_NAME_LIBRARY_FILE_NAME)))
			{
				file.Write(Strings.PATCH_FILE_OPENING_LINE_NAME_LIBRARY);
				file.Write(str);
				file.Write(Strings.PATCH_FILE_CLOSING_LINE);
			}
		}
	}
}

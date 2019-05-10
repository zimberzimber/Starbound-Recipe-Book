using Jil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SRBG
{
	static class ItemNameLibrary
	{
		static Dictionary<string, List<string>> _nameLibrary = new Dictionary<string, List<string>>(0);
		public static int internalNamesIndexed = 0;
		public static int displayNamesIndexed = 0;

        static object _threadLockAnchor = new object();

		public static void AddNameEntry(string displayedName, string internalName)
		{
            lock (_threadLockAnchor)
            {
                if (displayedName == null) return;

                displayedName = Regex.Replace(displayedName, Strings.UNFORMAT_REGEX_PATTERN, "");
                displayedName = displayedName.ToLower();

                if (!_nameLibrary.ContainsKey(displayedName))
                {
                    _nameLibrary.Add(displayedName, new List<string>(0));
                    displayNamesIndexed++;
                }
                _nameLibrary[displayedName].Add(internalName);
                internalNamesIndexed++;
            }
		}

		public static void WriteToFile()
		{
			//_nameLibrary = _nameLibrary.OrderBy(n => n.Key).ToDictionary(n => n.Key, n => n.Value);
			string str = JSON.Serialize(_nameLibrary);
			using (StreamWriter file = new StreamWriter(string.Format("{0}{1}\\{2}\\{3}", AppDomain.CurrentDomain.BaseDirectory, Strings.OUTPUT_DIRECTORY_NAME, Strings.modName, Strings.ITEM_NAME_LIBRARY_FILE_NAME)))
			{
				file.Write(Strings.PATCH_FILE_OPENING_LINE);
				file.Write(str);
				file.Write(Strings.PATCH_FILE_CLOSING_LINE);
			}
		}
	}
}

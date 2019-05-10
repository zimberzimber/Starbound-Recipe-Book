using Jil;
using System;
using System.Collections.Generic;
using System.IO;

namespace SRBG
{
	static class Groups
	{
		static Dictionary<string, List<string>> _groups = new Dictionary<string, List<string>>(0);
		public static int totalGroups = 0;

        static object _threadLockAnchor = new object();

        public static void Add(string[] tags, string item)
        {
            lock (_threadLockAnchor)
            {
                if (tags == null) return;
                for (int i = 0; i < tags.Length; i++)
                { Add(tags[i], item); }
            }
		}

		static void Add(string tag, string item)
		{
			if (!_groups.ContainsKey(tag))
			{
				totalGroups++;
				_groups.Add(tag, new List<string>(0));
			}

			if (!_groups[tag].Contains(item))
				_groups[tag].Add(item);
		}

		public static void WriteToFile()
		{
			string str = JSON.Serialize(_groups);
            using (StreamWriter file = new StreamWriter(string.Format("{0}{1}\\{2}\\{3}", AppDomain.CurrentDomain.BaseDirectory, Strings.OUTPUT_DIRECTORY_NAME, Strings.modName, Strings.GROUP_DICTIONARY_FILE_NAME)))
            {
                file.Write(Strings.PATCH_FILE_OPENING_LINE);
                file.Write(str);
                file.Write(Strings.PATCH_FILE_CLOSING_LINE);
            }
		}
	}
}

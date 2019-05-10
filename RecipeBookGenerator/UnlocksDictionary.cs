using Jil;
using System;
using System.Collections.Generic;
using System.IO;

namespace SRBG
{
    static class Unlocks
    {
        static Dictionary<string, List<string>> _unlocks = new Dictionary<string, List<string>>(0);
        public static int unlockCount = 0;
        public static int scannedFiles = 0;

        public static void Add(Item item)
        {
            scannedFiles++;
            Add(item.learnBlueprintsOnPickup, item.itemName);
            ItemNameLibrary.AddNameEntry(item.shortdescription, item.itemName);
            item.shortdescription = null; // Obsolete outside of the names library
        }

        static void Add(string[] items, string unlockedBy)
        {
            if (items == null) return;

            for (int i = 0; i < items.Length; i++)
            {
                if (!_unlocks.ContainsKey(items[i]))
                    _unlocks.Add(items[i], new List<string>(0));

                if (!_unlocks[items[i]].Contains(unlockedBy))
                {
                    unlockCount++;
                    _unlocks[items[i]].Add(unlockedBy);
                }
            }
        }

        public static void WriteToFile()
        {
            string str = JSON.Serialize(_unlocks);
            using (StreamWriter file = new StreamWriter(string.Format("{0}{1}\\{2}\\{3}", AppDomain.CurrentDomain.BaseDirectory, Strings.OUTPUT_DIRECTORY_NAME, Strings.modName, Strings.UNLOCKS_DICTIONARY_FILE_NAME)))
            {
                file.Write(Strings.PATCH_FILE_OPENING_LINE);
                file.Write(str);
                file.Write(Strings.PATCH_FILE_CLOSING_LINE);
            }
        }
    }

    public class Item
    {
        public string itemName;
        public string shortdescription;
        public string[] learnBlueprintsOnPickup;
    }
}

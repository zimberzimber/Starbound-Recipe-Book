using Jil;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading;

namespace SRBG
{
    partial class Program
    {

        static Queue threadWorkQueue = Queue.Synchronized(new Queue());
        static int threadWorkLeft;

        static void GenerateFilesMultithread(string dir)
        {
            AddFilesToQueue(dir);

            threadWorkLeft = threadWorkQueue.Count;
            int workItems = threadWorkQueue.Count;
            for (int i = 0; i < workItems; i++)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    string file = threadWorkQueue.Dequeue().ToString();
                    string ext = Path.GetExtension(file);

                    if (ext.Equals(Strings.RECIPE_FILE_EXTENSION))
                    {
                        Console.WriteLine(string.Format("{0}\n{1}", Strings.READING_RECIPE, file));

                        string str = RemoveComments(File.ReadAllText(file));
                        Recipe toAdd = JSON.Deserialize<Recipe>(str);
                        toAdd.FixBullshit();
                        Recipes.Add(toAdd);
                    }
                    else if (Strings.ITEM_FILE_EXTENSIONS.Contains(ext))
                    {
                        Console.WriteLine(string.Format("{0}\n{1}", Strings.READING_UNLOCKS, file));

                        string toAdd = RemoveComments(File.ReadAllText(file));
                        Unlocks.Add(JSON.Deserialize<Item>(toAdd, Options.PrettyPrintExcludeNulls));
                    }
                    Interlocked.Decrement(ref threadWorkLeft);
                });
            }
        }

        static void AddFilesToQueue(string dir)
        {
            string[] files = Directory.GetFiles(dir);
            string[] directories = Directory.GetDirectories(dir);

            for (int i = 0; i < directories.Length; i++)
                AddFilesToQueue(directories[i]);

            for (int i = 0; i < files.Length; i++)
            {
                string ext = Path.GetExtension(files[i]);
                if (ext.Equals(Strings.RECIPE_FILE_EXTENSION) || Strings.ITEM_FILE_EXTENSIONS.Contains(ext))
                    threadWorkQueue.Enqueue(files[i]);
            }
        }
    }
}

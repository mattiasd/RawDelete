using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RawDelete
{
    class Program
    {
        static void Main(string[] args)
        {
            var toDelete = new List<string>();
            FindRaw(args[0], toDelete);

            if (args.Length == 1 || args[1] != "-d")
            {
                Console.WriteLine();
                Console.WriteLine("Nothing deleted. Confim with -d to delete.");
            }
            else
            {
                Console.WriteLine("Deleting...");
                foreach (var file in toDelete)
                {
                    File.Delete(file);
                }
                Console.WriteLine("Done");
            }
        }

        static void FindRaw(string dir, List<string> toDelete)
        {
            foreach (string d in Directory.GetDirectories(dir))
            {
                Console.WriteLine("Searching " + d);

                var fileNames = Directory.GetFiles(d);

                var fileGroups = from f in fileNames
                                 group f by Path.GetFileNameWithoutExtension(f) into g
                                 select new { Name = g.Key, FileNames = g };

                foreach (var g in fileGroups.Where(arg => arg.FileNames.Count() == 1 && arg.FileNames.First().EndsWith("ARW", StringComparison.Ordinal)))
                {
                    foreach (var fname in g.FileNames)
                    {
                        toDelete.Add(fname);
                        Console.WriteLine(fname);
                    }
                }

                FindRaw(d, toDelete);
            }
        }
    }
}

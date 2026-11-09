using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MikeyXPCreamy.BioRand6
{
    // Copies of the randomized files, for viewing. RE6 loads the ones in its own folder.
    internal static class InstalledFiles
    {
        private const string GameFolder = "nativePC";

        public static List<string> Names(PatchStore store)
        {
            return store.ActiveFiles().Select(file => file.RelativePath.Replace('/', '\\')).OrderBy(name => name, StringComparer.OrdinalIgnoreCase).ToList();
        }

        public static void Sync(string root, PatchStore store)
        {
            List<JournalFile> files = store.ActiveFiles();
            var keep = new HashSet<string>(files.Select(file => TargetPath(root, file)), StringComparer.OrdinalIgnoreCase);
            RemoveOthers(root, keep);
            foreach (JournalFile file in files)
            {
                string target = TargetPath(root, file);
                if (!File.Exists(target) || Hashes.FileHash(target) != file.PatchedSha256)
                    DataFiles.CopyVerified(store.GamePath(file.RelativePath), target, file.PatchedSha256);
            }
        }

        private static string TargetPath(string root, JournalFile file)
        {
            return DataFiles.Within(root, Path.Combine(root, file.RelativePath.Replace('/', '\\')));
        }

        private static void RemoveOthers(string root, HashSet<string> keep)
        {
            string folder = Path.Combine(root, GameFolder);
            if (!Directory.Exists(folder))
                return;
            DataFiles.CheckTree(root, folder);
            foreach (string file in Directory.GetFiles(folder, "*", SearchOption.AllDirectories))
            {
                if (!keep.Contains(file))
                    DataFiles.Delete(root, file);
            }
            foreach (string child in Directory.GetDirectories(folder, "*", SearchOption.AllDirectories).OrderByDescending(path => path.Length))
            {
                if (!Directory.EnumerateFileSystemEntries(child).Any())
                    DataFiles.Delete(root, child);
            }
            if (!Directory.EnumerateFileSystemEntries(folder).Any())
                DataFiles.Delete(root, folder);
        }
    }
}

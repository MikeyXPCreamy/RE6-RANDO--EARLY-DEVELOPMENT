using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MikeyXPCreamy.BioRand6
{
    internal static class RunDescription
    {
        private const string LineEnd = "\r\n";
        private const string Indent = "    ";

        public static void Write(string root, IsolatedRun run, List<string> files, string copyProblem)
        {
            WriteText(Path.Combine(root, "manifest.txt"),
                "[MOD]" + LineEnd +
                "Name = BioRand 6: A Resident Evil 6 Randomizer" + LineEnd +
                "Author = " + MainForm.Author + LineEnd +
                "Version = " + MainForm.Version + LineEnd);

            string title = "BioRand 6 v" + MainForm.Version + LineEnd;
            WriteText(Path.Combine(root, "description.txt"), run == null ? title : title + "Seed: " + run.Setup.Seed + LineEnd);
            WriteText(Path.Combine(root, "log.txt"), LogText(title, run, files, copyProblem));
        }

        private static string LogText(string title, IsolatedRun run, List<string> files, string copyProblem)
        {
            var log = new StringBuilder(title);
            if (run != null)
                log.Append(Indent).Append("Seed: ").Append(run.Setup.Seed).Append(LineEnd);

            if (run != null && run.Loadouts != null)
            {
                log.Append("Starting weapons:").Append(LineEnd);
                foreach (FreshCharacter character in run.Loadouts.Characters)
                {
                    log.Append(Indent).Append(character.Character).Append(':').Append(LineEnd);
                    foreach (FreshWeapon weapon in character.Weapons)
                        log.Append(Indent).Append(Indent).Append(weapon.Weapon).Append(" (").Append(weapon.LoadedRounds).Append(" loaded, ").Append(weapon.ReserveRounds).Append(" reserve)").Append(LineEnd);
                }
            }

            if (files.Count > 0)
            {
                log.Append("Randomized files (").Append(files.Count).Append("):").Append(LineEnd);
                foreach (string file in files)
                    log.Append(Indent).Append(file).Append(LineEnd);
            }
            if (copyProblem != null)
                log.Append("Copies of the randomized files could not be saved: ").Append(copyProblem).Append(LineEnd);
            return log.ToString();
        }

        private static void WriteText(string path, string text)
        {
            DataFiles.Write(path, new UTF8Encoding(false).GetBytes(text));
        }
    }
}

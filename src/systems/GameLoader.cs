using System.IO;
using System.Text;

namespace MikeyXPCreamy.BioRand6
{
    public static class GameLoader
    {
        private const int MinimumLength = 1024;
        private const int MaximumLength = 1024 * 1024;

        private static readonly byte[] Marker = Encoding.ASCII.GetBytes("BioRand6-loader-dinput8");

        private static readonly PlacedFile Placement = new PlacedFile(
            folder: "",
            fileName: "dinput8.dll",
            isOwnFile: IsLoader,
            damagedMessage: "BioRand's game loader is damaged.",
            folderMissingMessage: "The RE6 folder is missing.",
            nameTakenMessage: "Another program's dinput8.dll is in the RE6 folder. Move it out to start a BioRand run.",
            copyFailedMessage: "Game loader verification failed.",
            linkedMessage: "Linked game folders are not supported.");

        public static void Place(string gameRoot, string loader)
        {
            Placement.Place(gameRoot, loader);
        }

        public static bool IsPlaced(string gameRoot)
        {
            return Placement.IsPlaced(gameRoot);
        }

        public static void Remove(string gameRoot)
        {
            Placement.Remove(gameRoot);
        }

        private static bool IsLoader(string path)
        {
            byte[] bytes;
            using (var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (file.Length < MinimumLength || file.Length > MaximumLength)
                    return false;
                bytes = new byte[file.Length];
                if (file.Read(bytes, 0, bytes.Length) != bytes.Length)
                    return false;
            }
            return bytes[0] == 'M' && bytes[1] == 'Z' && HasMarker(bytes);
        }

        private static bool HasMarker(byte[] bytes)
        {
            for (int start = 0; start + Marker.Length <= bytes.Length; start++)
            {
                int matched = 0;
                while (matched < Marker.Length && bytes[start + matched] == Marker[matched])
                    matched++;
                if (matched == Marker.Length)
                    return true;
            }
            return false;
        }
    }
}

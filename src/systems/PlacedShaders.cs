using System.IO;
using System.Linq;
using System.Text;

namespace MikeyXPCreamy.BioRand6
{
    public static class PlacedShaders
    {
        private const string Folder = @"nativePC\sa\DX9";

        public static readonly PlacedFile Characters = Package("BioRand6Chars", "character", "Character");

        public static readonly PlacedFile Swap = Package("BioRand6Swap", "swap", "Swap");

        public static readonly PlacedFile Enemies = Package("BioRand6Enemies", "enemy", "Enemy");

        public static bool AnyPlaced(string gameRoot)
        {
            return Characters.IsPlaced(gameRoot) || Swap.IsPlaced(gameRoot) || Enemies.IsPlaced(gameRoot);
        }

        private static PlacedFile Package(string name, string label, string capitalizedLabel)
        {
            string fileName = name + ".arc";
            byte[] header = Encoding.ASCII.GetBytes("ARC\0\x07\0\x01\0sc\\DX9\\" + name + "\0");
            return new PlacedFile(
                folder: Folder,
                fileName: fileName,
                isOwnFile: path => StartsWith(path, header),
                damagedMessage: "The " + label + " shader package is damaged.",
                folderMissingMessage: "The RE6 shader folder is missing.",
                nameTakenMessage: "Another file already uses the name " + fileName + ".",
                copyFailedMessage: capitalizedLabel + " shader package verification failed.",
                linkedMessage: "Linked shader folders are not supported.");
        }

        private static bool StartsWith(string path, byte[] header)
        {
            var head = new byte[header.Length];
            using (var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (file.Length < ShaderPackages.ArcMinimumLength || file.Read(head, 0, head.Length) != head.Length)
                    return false;
            }
            return head.SequenceEqual(header);
        }
    }
}

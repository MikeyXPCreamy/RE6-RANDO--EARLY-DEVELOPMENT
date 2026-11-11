using System;
using System.IO;

namespace MikeyXPCreamy.BioRand6
{
    public sealed class PlacedFile
    {
        private const string PendingSuffix = ".pending";

        private readonly string folder;
        private readonly string fileName;
        private readonly Func<string, bool> isOwnFile;
        private readonly string damagedMessage;
        private readonly string folderMissingMessage;
        private readonly string nameTakenMessage;
        private readonly string copyFailedMessage;
        private readonly string linkedMessage;

        public PlacedFile(string folder, string fileName, Func<string, bool> isOwnFile, string damagedMessage, string folderMissingMessage, string nameTakenMessage, string copyFailedMessage, string linkedMessage)
        {
            this.folder = folder;
            this.fileName = fileName;
            this.isOwnFile = isOwnFile;
            this.damagedMessage = damagedMessage;
            this.folderMissingMessage = folderMissingMessage;
            this.nameTakenMessage = nameTakenMessage;
            this.copyFailedMessage = copyFailedMessage;
            this.linkedMessage = linkedMessage;
        }

        private string GamePath(string gameRoot)
        {
            return Path.Combine(Path.GetFullPath(gameRoot), folder, fileName);
        }

        public void Place(string gameRoot, string source)
        {
            if (!isOwnFile(source))
                throw new InvalidDataException(damagedMessage);
            string target = GamePath(gameRoot);
            string expected = Hashes.FileHash(source);
            if (!Directory.Exists(Path.GetDirectoryName(target)))
                throw new DirectoryNotFoundException(folderMissingMessage);
            RefuseLinks(target);
            if (File.Exists(target) && Hashes.FileHash(target) == expected)
                return;
            if (File.Exists(target) && !isOwnFile(target))
                throw new IOException(nameTakenMessage);

            string pending = target + PendingSuffix;
            if (File.Exists(pending))
                File.Delete(pending);
            File.Copy(source, pending);
            if (Hashes.FileHash(pending) != expected)
            {
                File.Delete(pending);
                throw new IOException(copyFailedMessage);
            }
            if (File.Exists(target))
                File.Replace(pending, target, null);
            else
                File.Move(pending, target);
            if (Hashes.FileHash(target) != expected)
                throw new IOException(copyFailedMessage);
        }

        public bool IsPlaced(string gameRoot)
        {
            try
            {
                string target = GamePath(gameRoot);
                return IsOwnAt(target) || IsOwnAt(target + PendingSuffix);
            }
            catch (IOException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }

        public void Remove(string gameRoot)
        {
            string target = GamePath(gameRoot);
            if (!Directory.Exists(Path.GetDirectoryName(target)))
                return;
            RefuseLinks(target);
            RemoveIfOwn(target);
            RemoveIfOwn(target + PendingSuffix);
        }

        private bool IsOwnAt(string path)
        {
            return File.Exists(path) && isOwnFile(path);
        }

        private void RemoveIfOwn(string path)
        {
            if (IsOwnAt(path))
                File.Delete(path);
        }

        private void RefuseLinks(string target)
        {
            if (IsLink(target) || IsLink(Path.GetDirectoryName(target)))
                throw new IOException(linkedMessage);
        }

        private static bool IsLink(string path)
        {
            return (File.Exists(path) || Directory.Exists(path)) && (File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0;
        }
    }
}

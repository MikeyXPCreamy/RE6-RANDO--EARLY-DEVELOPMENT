using System;
using System.IO;

namespace MikeyXPCreamy.BioRand6
{
    public static class GenerateProgress
    {
        private const long BytesPerKilobyte = 1024;
        private const long BytesPerMegabyte = 1024 * 1024;
        private const int KindColumnWidth = 10;

        [ThreadStatic] public static Action<string> Report;

        [ThreadStatic] public static Action<string, string, string> ReportFile;

        public static void Note(string message)
        {
            Action<string> report = Report;
            if (report != null)
                report(message);
        }

        public static void File(string kind, string path, string shown)
        {
            Action<string> report = Report;
            Action<string, string, string> reportFile = ReportFile;
            if (report == null && reportFile == null)
                return;

            string size = SizeText(new FileInfo(path));
            if (reportFile != null)
                reportFile(kind, shown, size);
            else
                report(kind.PadRight(KindColumnWidth) + shown + (size.Length > 0 ? "   " + size : ""));
        }

        private static string SizeText(FileInfo info)
        {
            if (!info.Exists)
                return "";
            if (info.Length >= BytesPerMegabyte)
                return (info.Length / (double)BytesPerMegabyte).ToString("0.0") + " MB";
            return Math.Max(1, info.Length / BytesPerKilobyte) + " KB";
        }
    }
}

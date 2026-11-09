using System;
using System.IO;
using System.Text;

namespace MikeyXPCreamy.BioRand6
{
    public static class ChapterLoadouts
    {
        private const string Magic = "RE6CHP3\0";
        private const int FormatVersion = 2;
        private const int Chapters = 5;
        private const int Characters = 8;
        private const int RecordSize = 164;
        private const int PayloadSize = Chapters * Characters * RecordSize;
        private const uint FnvOffset = 2166136261;
        private const uint FnvPrime = 16777619;

        public static ChapterLoadoutReport Write(string path, byte[] personalSave, SeedSetup seed)
        {
            byte[] payload = new byte[PayloadSize];
            var report = new ChapterLoadoutReport();
            for (int chapter = 1; chapter <= Chapters; chapter++)
            {
                FreshSaveCandidate candidate = FreshSaveCodec.GenerateChapter(personalSave, seed, chapter);
                for (int character = 0; character < Characters; character++)
                {
                    int offset = ((chapter - 1) * Characters + character) * RecordSize;
                    Array.Copy(FreshSaveCodec.Record(candidate.Bytes, character), 0, payload, offset, RecordSize);
                }
                report.Chapters.Add(candidate.Report);
            }

            uint checksum = FnvOffset;
            foreach (byte value in payload)
                checksum = (checksum ^ value) * FnvPrime;

            using (var file = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            using (var writer = new BinaryWriter(file))
            {
                writer.Write(Encoding.ASCII.GetBytes(Magic));
                writer.Write(FormatVersion);
                writer.Write(PayloadSize);
                writer.Write(checksum);
                writer.Write(payload);
                writer.Flush();
                file.Flush(true);
            }
            return report;
        }
    }
}

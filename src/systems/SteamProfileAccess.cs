using System;
using System.IO;

namespace MikeyXPCreamy.BioRand6
{
    public sealed class SteamProfileAccess
    {
        private readonly string gameRoot;
        private readonly EmbeddedRuntime runtime;

        public SteamProfileAccess(string gameRoot, EmbeddedRuntime runtime)
        {
            this.gameRoot = gameRoot;
            this.runtime = runtime;
        }

        public byte[] ReadPersonal(string directory)
        {
            if (SteamGame.Running())
                throw new InvalidOperationException("Close RE6 first.");

            string copyPath = Path.Combine(directory, "personal-baseline.bin");
            EmbeddedRuntime.Run(runtime.Extract("SteamBridge.exe"), gameRoot, copyPath);
            byte[] save = File.ReadAllBytes(copyPath);
            FreshSaveCodec.Validate(save);
            return save;
        }
    }
}

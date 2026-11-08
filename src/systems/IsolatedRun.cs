using System.Runtime.Serialization;

namespace MikeyXPCreamy.BioRand6
{
    [DataContract]
    public sealed class IsolatedRun
    {
        [DataMember] public int Schema = 2;
        [DataMember] public string SeedId;
        [DataMember] public string Directory;
        [DataMember] public string SavePath;
        [DataMember] public string PersonalSha256;
        [DataMember] public string State;
        [DataMember] public string StorageId;
        [DataMember] public string GameRoot;
        [DataMember] public string BackendVersion;
        [DataMember] public string ChapterSha256;
        [DataMember] public string WeaponSha256;
        [DataMember] public SeedSetup Setup;
        [DataMember] public bool AllChaptersSelectable = true;
        [DataMember] public FreshSaveReport Loadouts;
        [DataMember] public bool AutomaticChapterLoadouts;
    }
}

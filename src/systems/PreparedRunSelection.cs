using System.Runtime.Serialization;

namespace MikeyXPCreamy.BioRand6
{
    [DataContract]
    public sealed class PreparedRunSelection
    {
        [DataMember] public int Schema = 1;
        [DataMember] public string RunDirectory;
        [DataMember] public string SeedId;
        [DataMember] public string StorageId;
    }
}

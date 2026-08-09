using System.Text.Json.Serialization;
using Server.Regions;

namespace Server.Engines.MurderPenalty;

public class MurdererJailRegion : JailRegion
{
    [JsonConstructor]
    public MurdererJailRegion(string name, Map map, int priority, params Rectangle3D[] area)
        : base(name, map, priority, area)
    {
    }

    public MurdererJailRegion(string name, Map map, Region parent, params Rectangle3D[] area)
        : base(name, map, parent, area)
    {
    }

    public MurdererJailRegion(string name, Map map, Region parent, int priority, params Rectangle3D[] area)
        : base(name, map, parent, priority, area)
    {
    }

    public static void Configure()
    {
        RegionJsonSerializer.Register<MurdererJailRegion>();
    }
}

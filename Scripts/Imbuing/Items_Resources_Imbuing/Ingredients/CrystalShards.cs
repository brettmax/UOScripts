using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Crystal Shards - Used for damage increase properties
/// </summary>
[SerializationGenerator(0, false)]
public partial class CrystalShards : Item
{
    [Constructible]
    public CrystalShards(int amount = 1) : base(0x5738)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0x47E;
    }

    public override int LabelNumber => 1113347; // Crystal Shards
    public override double DefaultWeight => 0.1;
}

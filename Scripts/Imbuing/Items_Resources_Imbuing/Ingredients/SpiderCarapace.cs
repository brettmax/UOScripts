using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Spider Carapace - Used for Arachnid Doom slayer
/// </summary>
[SerializationGenerator(0, false)]
public partial class SpiderCarapace : Item
{
    [Constructible]
    public SpiderCarapace(int amount = 1) : base(0x5720)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113368; // Spider Carapace
    public override double DefaultWeight => 0.1;
}

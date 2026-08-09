using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Relic Fragment - High imbuing ingredient from unraveling high-weight items
/// </summary>
[SerializationGenerator(0, false)]
public partial class RelicFragment : Item
{
    [Constructible]
    public RelicFragment(int amount = 1) : base(0x2DB3)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1031699; // Relic Fragment
    public override double DefaultWeight => 0.1;
}

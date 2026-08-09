using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Magical Residue - Basic imbuing ingredient from unraveling low-weight items
/// </summary>
[SerializationGenerator(0, false)]
public partial class MagicalResidue : Item
{
    [Constructible]
    public MagicalResidue(int amount = 1) : base(0x2DB1)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1031697; // Magical Residue
    public override double DefaultWeight => 0.1;
}

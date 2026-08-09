using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Undying Flesh - Used for Silver/Undead slayer
/// </summary>
[SerializationGenerator(0, false)]
public partial class UndyingFlesh : Item
{
    [Constructible]
    public UndyingFlesh(int amount = 1) : base(0x5728)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113369; // Undying Flesh
    public override double DefaultWeight => 0.1;
}

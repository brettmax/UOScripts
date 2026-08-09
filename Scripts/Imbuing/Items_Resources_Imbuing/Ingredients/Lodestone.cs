using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Lodestone - Used for skill belts crafting and imbuing skill bonuses
/// Drops from Cavern of the Discarded
/// </summary>
[SerializationGenerator(0, false)]
public partial class Lodestone : Item
{
    [Constructible]
    public Lodestone(int amount = 1) : base(0x5739)
    {
        Stackable = true;
        Amount = amount;
    }

    public override int LabelNumber => 1113348; // Lodestone
    public override double DefaultWeight => 0.1;
}

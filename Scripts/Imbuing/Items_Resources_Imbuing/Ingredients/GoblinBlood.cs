using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Goblin Blood - Used for Repond slayer
/// </summary>
[SerializationGenerator(0, false)]
public partial class GoblinBlood : Item
{
    [Constructible]
    public GoblinBlood(int amount = 1) : base(0x572C)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113363; // Goblin Blood
    public override double DefaultWeight => 0.1;
}

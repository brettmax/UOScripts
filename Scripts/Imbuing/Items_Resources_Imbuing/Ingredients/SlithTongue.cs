using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Slith Tongue - Used for Hit Dispel
/// </summary>
[SerializationGenerator(0, false)]
public partial class SlithTongue : Item
{
    [Constructible]
    public SlithTongue(int amount = 1) : base(0x5748)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113367; // Slith Tongue
    public override double DefaultWeight => 0.1;
}

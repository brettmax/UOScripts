using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Crystalline Blackrock - Used for skill bonuses on jewelry
/// </summary>
[SerializationGenerator(0, false)]
public partial class CrystallineBlackrock : Item
{
    [Constructible]
    public CrystallineBlackrock(int amount = 1) : base(0x5732)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0x455;
    }

    public override int LabelNumber => 1077568; // Crystalline Blackrock
    public override double DefaultWeight => 0.1;
}

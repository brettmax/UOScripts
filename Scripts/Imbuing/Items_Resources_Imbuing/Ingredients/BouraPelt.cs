using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Boura Pelt - Used for resistance properties
/// </summary>
[SerializationGenerator(0, false)]
public partial class BouraPelt : Item
{
    [Constructible]
    public BouraPelt(int amount = 1) : base(0x5742)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113355; // Boura Pelt
    public override double DefaultWeight => 0.1;
}

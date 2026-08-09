using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Void Orb - Used for leech properties
/// </summary>
[SerializationGenerator(0, false)]
public partial class VoidOrb : Item
{
    [Constructible]
    public VoidOrb(int amount = 1) : base(0x573A)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0x455;
    }

    public override int LabelNumber => 1113354; // Void Orb
    public override double DefaultWeight => 0.1;
}

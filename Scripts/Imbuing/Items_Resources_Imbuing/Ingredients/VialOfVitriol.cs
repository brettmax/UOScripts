using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Vial of Vitriol - Used for Elemental Ban slayer
/// </summary>
[SerializationGenerator(0, false)]
public partial class VialOfVitriol : Item
{
    [Constructible]
    public VialOfVitriol(int amount = 1) : base(0x5722)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113364; // Vial of Vitriol
    public override double DefaultWeight => 0.1;
}

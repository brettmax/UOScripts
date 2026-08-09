using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Lava Serpent Crust - Used for Reptilian Death slayer
/// </summary>
[SerializationGenerator(0, false)]
public partial class LavaSerpentCrust : Item
{
    [Constructible]
    public LavaSerpentCrust(int amount = 1) : base(0x572D)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113365; // Lava Serpent Crust
    public override double DefaultWeight => 0.1;
}

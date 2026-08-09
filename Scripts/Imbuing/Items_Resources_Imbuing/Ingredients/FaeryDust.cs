using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Faery Dust - Used for lower reagent cost
/// </summary>
[SerializationGenerator(0, false)]
public partial class FaeryDust : Item
{
    [Constructible]
    public FaeryDust(int amount = 1) : base(0x5745)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0x9;
    }

    public override int LabelNumber => 1113358; // Faery Dust
    public override double DefaultWeight => 0.1;
}

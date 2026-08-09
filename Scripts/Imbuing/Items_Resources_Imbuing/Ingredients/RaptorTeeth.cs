using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Raptor Teeth - Used for hit area effects
/// </summary>
[SerializationGenerator(0, false)]
public partial class RaptorTeeth : Item
{
    [Constructible]
    public RaptorTeeth(int amount = 1) : base(0x5746)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113366; // Raptor Teeth
    public override double DefaultWeight => 0.1;
}

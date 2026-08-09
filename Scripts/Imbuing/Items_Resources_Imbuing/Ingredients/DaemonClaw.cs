using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Daemon Claw - Used for Exorcism slayer
/// </summary>
[SerializationGenerator(0, false)]
public partial class DaemonClaw : Item
{
    [Constructible]
    public DaemonClaw(int amount = 1) : base(0x5721)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113362; // Daemon Claw
    public override double DefaultWeight => 0.1;
}

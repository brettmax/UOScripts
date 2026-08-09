using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Seed of Renewal - Used for regeneration properties
/// </summary>
[SerializationGenerator(0, false)]
public partial class SeedOfRenewal : Item
{
    [Constructible]
    public SeedOfRenewal(int amount = 1) : base(0x5736)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0x48;
    }

    public override int LabelNumber => 1113346; // Seed of Renewal
    public override double DefaultWeight => 0.1;
}

using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Enchanted Essence - Medium imbuing ingredient from unraveling medium-weight items
/// </summary>
[SerializationGenerator(0, false)]
public partial class EnchantedEssence : Item
{
    [Constructible]
    public EnchantedEssence(int amount = 1) : base(0x2DB2)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1031698; // Enchanted Essence
    public override double DefaultWeight => 0.1;
}

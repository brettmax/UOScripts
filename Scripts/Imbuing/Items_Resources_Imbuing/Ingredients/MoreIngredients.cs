using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Reflective Wolf Eye - Used for reflect physical
/// </summary>
[SerializationGenerator(0, false)]
public partial class ReflectiveWolfEye : Item
{
    [Constructible]
    public ReflectiveWolfEye(int amount = 1) : base(0x5749)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113360; // Reflective Wolf Eye
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Silver Snake Skin - Used for spell channeling
/// </summary>
[SerializationGenerator(0, false)]
public partial class SilverSnakeSkin : Item
{
    [Constructible]
    public SilverSnakeSkin(int amount = 1) : base(0x5744)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113359; // Silver Snake Skin
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Arcanic Rune Stone - Used for mage weapon
/// </summary>
[SerializationGenerator(0, false)]
public partial class ArcanicRuneStone : Item
{
    [Constructible]
    public ArcanicRuneStone(int amount = 1) : base(0x573C)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113356; // Arcanic Rune Stone
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Bottle of Ichor - Used for night sight
/// </summary>
[SerializationGenerator(0, false)]
public partial class BottleIchor : Item
{
    [Constructible]
    public BottleIchor(int amount = 1) : base(0x573D)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113361; // Bottle of Ichor
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Chaga Mushroom - Used for luck
/// </summary>
[SerializationGenerator(0, false)]
public partial class ChagaMushroom : Item
{
    [Constructible]
    public ChagaMushroom(int amount = 1) : base(0x573E)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113357; // Chaga Mushroom
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Crushed Glass - Used for enhance potions
/// </summary>
[SerializationGenerator(0, false)]
public partial class CrushedGlass : Item
{
    [Constructible]
    public CrushedGlass(int amount = 1) : base(0x573F)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113351; // Crushed Glass
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Powdered Iron - Used for durability
/// </summary>
[SerializationGenerator(0, false)]
public partial class PowderedIron : Item
{
    [Constructible]
    public PowderedIron(int amount = 1) : base(0x5740)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113353; // Powdered Iron
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Elven Fletching - Used for lower stat req
/// </summary>
[SerializationGenerator(0, false)]
public partial class ElvenFletching : Item
{
    [Constructible]
    public ElvenFletching(int amount = 1) : base(0x5741)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113349; // Elven Fletching
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Delicate Scales - Used for use best skill
/// </summary>
[SerializationGenerator(0, false)]
public partial class DelicateScales : Item
{
    [Constructible]
    public DelicateScales(int amount = 1) : base(0x5743)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113350; // Delicate Scales
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Abyssal Cloth - Used for mage armor
/// </summary>
[SerializationGenerator(0, false)]
public partial class AbyssalCloth : Item
{
    [Constructible]
    public AbyssalCloth(int amount = 1) : base(0x5733)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113352; // Abyssal Cloth
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Fey Wings - Used for Fey slayer
/// </summary>
[SerializationGenerator(0, false)]
public partial class FeyWings : Item
{
    [Constructible]
    public FeyWings(int amount = 1) : base(0x5734)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0;
    }

    public override int LabelNumber => 1113355; // Fey Wings
    public override double DefaultWeight => 0.1;
}

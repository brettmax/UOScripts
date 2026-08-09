using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Essence of Singularity - Used for Defend Chance Increase
/// </summary>
[SerializationGenerator(0, false)]
public partial class EssenceSingularity : Item
{
    [Constructible]
    public EssenceSingularity(int amount = 1) : base(0x571C)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0x1;
    }

    public override int LabelNumber => 1113327; // Essence of Singularity
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Essence of Precision - Used for Hit Chance Increase
/// </summary>
[SerializationGenerator(0, false)]
public partial class EssencePrecision : Item
{
    [Constructible]
    public EssencePrecision(int amount = 1) : base(0x571C)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0x34;
    }

    public override int LabelNumber => 1113326; // Essence of Precision
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Essence of Control - Used for Swing Speed Increase
/// </summary>
[SerializationGenerator(0, false)]
public partial class EssenceControl : Item
{
    [Constructible]
    public EssenceControl(int amount = 1) : base(0x571C)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0x3B;
    }

    public override int LabelNumber => 1113322; // Essence of Control
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Essence of Diligence - Used for Faster Cast Recovery
/// </summary>
[SerializationGenerator(0, false)]
public partial class EssenceDiligence : Item
{
    [Constructible]
    public EssenceDiligence(int amount = 1) : base(0x571C)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0x42;
    }

    public override int LabelNumber => 1113323; // Essence of Diligence
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Essence of Achievement - Used for Faster Casting
/// </summary>
[SerializationGenerator(0, false)]
public partial class EssenceAchievement : Item
{
    [Constructible]
    public EssenceAchievement(int amount = 1) : base(0x571C)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0x56;
    }

    public override int LabelNumber => 1113321; // Essence of Achievement
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Essence of Feeling - Used for Hit Magic Arrow
/// </summary>
[SerializationGenerator(0, false)]
public partial class EssenceFeeling : Item
{
    [Constructible]
    public EssenceFeeling(int amount = 1) : base(0x571C)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0x49;
    }

    public override int LabelNumber => 1113324; // Essence of Feeling
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Essence of Order - Used for Lower Mana Cost
/// </summary>
[SerializationGenerator(0, false)]
public partial class EssenceOrder : Item
{
    [Constructible]
    public EssenceOrder(int amount = 1) : base(0x571C)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0x5C;
    }

    public override int LabelNumber => 1113325; // Essence of Order
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Essence of Passion - Used for Hit Lightning
/// </summary>
[SerializationGenerator(0, false)]
public partial class EssencePassion : Item
{
    [Constructible]
    public EssencePassion(int amount = 1) : base(0x571C)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0x63;
    }

    public override int LabelNumber => 1113328; // Essence of Passion
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Essence of Direction - Used for Velocity
/// </summary>
[SerializationGenerator(0, false)]
public partial class EssenceDirection : Item
{
    [Constructible]
    public EssenceDirection(int amount = 1) : base(0x571C)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0x6A;
    }

    public override int LabelNumber => 1113329; // Essence of Direction
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Essence of Balance - Used for Balanced Weapon
/// </summary>
[SerializationGenerator(0, false)]
public partial class EssenceBalance : Item
{
    [Constructible]
    public EssenceBalance(int amount = 1) : base(0x571C)
    {
        Stackable = true;
        Amount = amount;
        Hue = 0x71;
    }

    public override int LabelNumber => 1113330; // Essence of Balance
    public override double DefaultWeight => 0.1;
}

/// <summary>
/// Essence of Persistence - Dropped at Skeletal Dragon mini-champ
/// </summary>
[SerializationGenerator(0, false)]
public partial class EssencePersistence : Item
{
    [Constructible]
    public EssencePersistence(int amount = 1) : base(0x571C)
    {
        Stackable = true;
        Amount = amount;
        Hue = 37;
    }

    public override int LabelNumber => 1113343; // Essence of Persistence
    public override double DefaultWeight => 0.1;
}

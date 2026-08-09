using ModernUO.Serialization;

namespace Server.Items;

/// <summary>
/// Large Soulforge addon for Imbuing skill (4x4 grid)
/// </summary>
[SerializationGenerator(0, false)]
public partial class SoulForge : BaseAddon
{
    public override BaseAddonDeed Deed => new SoulForgeDeed();

    [Constructible]
    public SoulForge()
    {
        // Row 0
        AddComponent(new AddonComponent(0x4263), 0, 0, 0);
        AddComponent(new AddonComponent(0x4264), 1, 0, 0);
        AddComponent(new AddonComponent(0x4265), 2, 0, 0);
        AddComponent(new AddonComponent(0x4266), 3, 0, 0);

        // Row 1
        AddComponent(new AddonComponent(0x4267), 0, 1, 0);
        AddComponent(new AddonComponent(0x4268), 1, 1, 0);
        AddComponent(new AddonComponent(0x4269), 2, 1, 0);
        AddComponent(new AddonComponent(0x426A), 3, 1, 0);

        // Row 2
        AddComponent(new AddonComponent(0x426B), 0, 2, 0);
        AddComponent(new AddonComponent(0x426C), 1, 2, 0);
        AddComponent(new AddonComponent(0x426D), 2, 2, 0);
        AddComponent(new AddonComponent(0x426E), 3, 2, 0);

        // Row 3
        AddComponent(new AddonComponent(0x426F), 0, 3, 0);
        AddComponent(new AddonComponent(0x4270), 1, 3, 0);
        AddComponent(new AddonComponent(0x4271), 2, 3, 0);
        AddComponent(new AddonComponent(0x4272), 3, 3, 0);
    }
}

/// <summary>
/// Deed for placing a large Soulforge
/// </summary>
[SerializationGenerator(0, false)]
public partial class SoulForgeDeed : BaseAddonDeed
{
    public override BaseAddon Addon => new SoulForge();
    public override int LabelNumber => 1031696; // Soulforge

    [Constructible]
    public SoulForgeDeed()
    {
    }
}

/// <summary>
/// Small Soulforge addon for Imbuing skill (single tile)
/// Same as MiniSoulForge in ServUO
/// </summary>
[SerializationGenerator(0, false)]
public partial class SmallSoulForge : BaseAddon
{
    public override BaseAddonDeed Deed => new SmallSoulForgeDeed();
    public override bool RetainDeedHue => true;

    [Constructible]
    public SmallSoulForge()
    {
        AddComponent(new AddonComponent(0x44C7), 0, 0, 0); // 17607 decimal
    }
}

/// <summary>
/// Deed for placing a small Soulforge
/// </summary>
[SerializationGenerator(0, false)]
public partial class SmallSoulForgeDeed : BaseAddonDeed
{
    public override BaseAddon Addon => new SmallSoulForge();
    public override int LabelNumber => 1149695; // Small Soulforge

    [Constructible]
    public SmallSoulForgeDeed()
    {
    }
}

/// <summary>
/// Royal Soulforge addon (4x4 grid, alternate design)
/// Found in Royal City, Ter Mur
/// </summary>
[SerializationGenerator(0, false)]
public partial class RoyalSoulForge : BaseAddon
{
    public override BaseAddonDeed Deed => new RoyalSoulForgeDeed();

    [Constructible]
    public RoyalSoulForge()
    {
        // Row 0
        AddComponent(new AddonComponent(0x4277), 0, 0, 0);
        AddComponent(new AddonComponent(0x4278), 1, 0, 0);
        AddComponent(new AddonComponent(0x4279), 2, 0, 0);
        AddComponent(new AddonComponent(0x427A), 3, 0, 0);

        // Row 1
        AddComponent(new AddonComponent(0x427B), 0, 1, 0);
        AddComponent(new AddonComponent(0x427C), 1, 1, 0);
        AddComponent(new AddonComponent(0x427D), 2, 1, 0);
        AddComponent(new AddonComponent(0x427E), 3, 1, 0);

        // Row 2
        AddComponent(new AddonComponent(0x427F), 0, 2, 0);
        AddComponent(new AddonComponent(0x4280), 1, 2, 0);
        AddComponent(new AddonComponent(0x4281), 2, 2, 0);
        AddComponent(new AddonComponent(0x4282), 3, 2, 0);

        // Row 3
        AddComponent(new AddonComponent(0x4283), 0, 3, 0);
        AddComponent(new AddonComponent(0x4284), 1, 3, 0);
        AddComponent(new AddonComponent(0x4285), 2, 3, 0);
        AddComponent(new AddonComponent(0x4286), 3, 3, 0);
    }
}

/// <summary>
/// Deed for placing a Royal Soulforge
/// </summary>
[SerializationGenerator(0, false)]
public partial class RoyalSoulForgeDeed : BaseAddonDeed
{
    public override BaseAddon Addon => new RoyalSoulForge();
    public override int LabelNumber => 1031696; // Soulforge

    [Constructible]
    public RoyalSoulForgeDeed()
    {
    }
}

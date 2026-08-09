using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Engines.Imbuing;

/// <summary>
/// Types of items that can be imbued
/// </summary>
public enum ImbuingItemType
{
    Invalid = 0,
    Melee = 1,
    Ranged = 2,
    Armor = 3,
    Shield = 4,
    Hat = 5,
    Jewel = 6
}

/// <summary>
/// Property information for a specific item type category
/// </summary>
public class PropCategoryInfo
{
    public ImbuingItemType ItemType { get; }
    public int Scale { get; }
    public int StandardMax { get; }
    public int LootMax { get; }
    public int[]? PowerfulLootRange { get; }

    public PropCategoryInfo(int itemRef, int standardMax, int lootMax)
        : this((ImbuingItemType)itemRef, -1, standardMax, lootMax, null)
    {
    }

    public PropCategoryInfo(int itemRef, int scale, int standardMax, int lootMax)
        : this((ImbuingItemType)itemRef, scale, standardMax, lootMax, null)
    {
    }

    public PropCategoryInfo(int itemRef, int standardMax, int lootMax, int[] powerfulRange)
        : this((ImbuingItemType)itemRef, -1, standardMax, lootMax, powerfulRange)
    {
    }

    public PropCategoryInfo(ImbuingItemType type, int scale, int standardMax, int lootMax, int[]? powerfulRange)
    {
        ItemType = type;
        Scale = scale;
        StandardMax = standardMax;
        LootMax = lootMax;
        PowerfulLootRange = powerfulRange;
    }
}

/// <summary>
/// Defines an imbuable property with all its attributes
/// </summary>
public class ItemPropertyInfo
{
    public int ID { get; set; }
    public bool Imbuable { get; set; }

    public object Attribute { get; set; }
    public int AttributeName { get; set; }
    public int Weight { get; set; }

    public Type? PrimaryRes { get; set; }
    public Type? GemRes { get; set; }
    public Type? SpecialRes { get; set; }

    public int PrimaryName { get; set; }
    public int GemName { get; set; }
    public int SpecialName { get; set; }

    public int Scale { get; set; }
    public int Start { get; set; }
    public int MaxIntensity { get; set; }
    public int Description { get; set; }

    public PropCategoryInfo?[] PropCategories { get; set; } = new PropCategoryInfo[7];

    // Non-imbuable constructor
    public ItemPropertyInfo(object attribute, int attributeName, int weight, int scale, int start, int maxInt)
        : this(attribute, attributeName, weight, null, null, null, scale, start, maxInt, -1)
    {
    }

    public ItemPropertyInfo(object attribute, int attributeName, int weight, int scale, int start, int maxInt, int description)
        : this(attribute, attributeName, weight, null, null, null, scale, start, maxInt, description)
    {
    }

    public ItemPropertyInfo(object attribute, int attributeName, int weight, int scale, int start, int maxInt, int description, params PropCategoryInfo[] categories)
        : this(attribute, attributeName, weight, null, null, null, scale, start, maxInt, description, categories)
    {
    }

    // Full constructor for imbuable properties
    public ItemPropertyInfo(
        object attribute,
        int attributeName,
        int weight,
        Type? pRes,
        Type? gRes,
        Type? spRes,
        int scale,
        int start,
        int maxInt,
        int desc,
        params PropCategoryInfo[] categories)
    {
        Attribute = attribute;
        AttributeName = attributeName;
        Weight = weight;
        PrimaryRes = pRes;
        GemRes = gRes;
        SpecialRes = spRes;
        Scale = scale;
        Start = start;
        MaxIntensity = maxInt;
        Description = desc;

        if (categories != null)
        {
            foreach (var cat in categories)
            {
                var index = (int)cat.ItemType;
                if (index >= 0 && index < PropCategories.Length)
                {
                    PropCategories[index] = cat;
                }
            }
        }

        // Property is imbuable if it has all three resource types
        Imbuable = pRes != null && gRes != null && spRes != null;

        PrimaryName = GetResourceLocalization(pRes);
        GemName = GetResourceLocalization(gRes);
        SpecialName = GetResourceLocalization(spRes);
    }

    /// <summary>
    /// Gets the localization number for a resource type
    /// </summary>
    public static int GetResourceLocalization(Type? type)
    {
        if (type == null)
            return 0;

        // Standard gems
        if (type == typeof(Tourmaline)) return 1023864;
        if (type == typeof(Ruby)) return 1023859;
        if (type == typeof(Diamond)) return 1023878;
        if (type == typeof(Sapphire)) return 1023857;
        if (type == typeof(Citrine)) return 1023861;
        if (type == typeof(Emerald)) return 1023856;
        if (type == typeof(StarSapphire)) return 1023855;
        if (type == typeof(Amethyst)) return 1023862;
        if (type == typeof(Amber)) return 1023877;

        // Imbuing residues (will be defined)
        if (type.Name == "RelicFragment") return 1031699;
        if (type.Name == "EnchantedEssence") return 1031698;
        if (type.Name == "MagicalResidue") return 1031697;

        // Rare gems
        if (type.Name == "DarkSapphire") return 1032690;
        if (type.Name == "Turquoise") return 1032691;
        if (type.Name == "PerfectEmerald") return 1032692;
        if (type.Name == "EcruCitrine") return 1032693;
        if (type.Name == "WhitePearl") return 1032694;
        if (type.Name == "FireRuby") return 1032695;
        if (type.Name == "BlueDiamond") return 1032696;
        if (type.Name == "BrilliantAmber") return 1032697;

        // Special ingredients
        if (type.Name == "ParasiticPlant") return 1032688;
        if (type.Name == "LuminescentFungi") return 1032689;
        if (type.Name == "CrystallineBlackrock") return 1077568;
        if (type.Name == "BouraPelt") return 1113355;

        // Essences
        if (type.Name == "EssenceSingularity") return 1113341;
        if (type.Name == "EssencePrecision") return 1113327;
        if (type.Name == "EssenceControl") return 1113340;
        if (type.Name == "EssenceDiligence") return 1113338;
        if (type.Name == "EssenceAchievement") return 1113325;
        if (type.Name == "EssenceFeeling") return 1113339;
        if (type.Name == "EssenceOrder") return 1113342;
        if (type.Name == "EssencePassion") return 1113326;
        if (type.Name == "EssenceDirection") return 1113328;
        if (type.Name == "EssenceBalance") return 1113324;
        if (type.Name == "EssencePersistence") return 1113343;

        // Other ingredients
        if (type.Name == "Lodestone") return 1113348;
        if (type.Name == "SeedOfRenewal") return 1113346;
        if (type.Name == "CrystalShards") return 1113347;
        if (type.Name == "BottleIchor") return 1113361;
        if (type.Name == "ReflectiveWolfEye") return 1113360;
        if (type.Name == "FaeryDust") return 1113358;
        if (type.Name == "SilverSnakeSkin") return 1113359;
        if (type.Name == "ArcanicRuneStone") return 1113356;
        if (type.Name == "SlithTongue") return 1113359;
        if (type.Name == "VoidOrb") return 1113354;
        if (type.Name == "RaptorTeeth") return 1113366;
        if (type.Name == "SpiderCarapace") return 1113329;
        if (type.Name == "DaemonClaw") return 1113362;
        if (type.Name == "VialOfVitriol") return 1113331;
        if (type.Name == "GoblinBlood") return 1113335;
        if (type.Name == "LavaSerpentCrust") return 1113336;
        if (type.Name == "UndyingFlesh") return 1113337;
        if (type.Name == "CrushedGlass") return 1113351;
        if (type.Name == "PowderedIron") return 1113353;
        if (type.Name == "ElvenFletching") return 1113349;
        if (type.Name == "DelicateScales") return 1113350;
        if (type.Name == "ChagaMushroom") return 1113357;
        if (type.Name == "FeyWings") return 1113332;
        if (type.Name == "AbyssalCloth") return 1113352;

        return 0;
    }

    #region Static Table

    public static Dictionary<int, ItemPropertyInfo> Table { get; } = new();
    public static Dictionary<ImbuingItemType, List<int>> LootTable { get; } = new();

    private static bool _initialized;

    public static void Initialize()
    {
        if (_initialized)
            return;

        _initialized = true;

        RegisterBaseProperties();
        ItemPropertyTable.RegisterProperties();
        ItemPropertyTable2.RegisterProperties();
    }

    public static void Register(int id, ItemPropertyInfo info)
    {
        info.ID = id;
        Table[id] = info;

        // Add to loot table by item type
        foreach (var cat in info.PropCategories)
        {
            if (cat != null && cat.ItemType != ImbuingItemType.Invalid)
            {
                if (!LootTable.ContainsKey(cat.ItemType))
                    LootTable[cat.ItemType] = new List<int>();

                if (!LootTable[cat.ItemType].Contains(id))
                    LootTable[cat.ItemType].Add(id);
            }
        }
    }

    public static ItemPropertyInfo? GetInfo(int id)
    {
        return Table.TryGetValue(id, out var info) ? info : null;
    }

    public static int GetWeight(int id)
    {
        return GetInfo(id)?.Weight ?? 0;
    }

    public static int GetMaxIntensity(Item item, int id, bool imbuing = false)
    {
        var info = GetInfo(id);
        if (info == null)
            return 0;

        var itemType = GetItemType(item);
        var cat = info.PropCategories[(int)itemType];

        if (cat != null)
        {
            return imbuing ? info.MaxIntensity : cat.StandardMax;
        }

        return info.MaxIntensity;
    }

    public static int GetScale(Item item, int id)
    {
        var info = GetInfo(id);
        if (info == null)
            return 1;

        var itemType = GetItemType(item);
        var cat = info.PropCategories[(int)itemType];

        if (cat?.Scale > 0)
            return cat.Scale;

        return info.Scale > 0 ? info.Scale : 1;
    }

    public static ImbuingItemType GetItemType(Item item)
    {
        return item switch
        {
            BaseRanged => ImbuingItemType.Ranged,
            BaseShield => ImbuingItemType.Shield,
            BaseWeapon => ImbuingItemType.Melee,
            BaseArmor armor when armor.Layer == Layer.Helm => ImbuingItemType.Hat,
            BaseArmor => ImbuingItemType.Armor,
            BaseJewel => ImbuingItemType.Jewel,
            BaseHat => ImbuingItemType.Hat,
            _ => ImbuingItemType.Invalid
        };
    }

    public static int GetMinIntensity(Item item, int id)
    {
        var info = GetInfo(id);
        if (info == null)
            return 1;

        return info.Start > 0 ? info.Start : 1;
    }

    public static TextDefinition? GetAttributeName(int id)
    {
        var info = GetInfo(id);
        return info?.AttributeName ?? 0;
    }

    public static object? GetAttribute(int id)
    {
        var info = GetInfo(id);
        return info?.Attribute;
    }

    #endregion

    #region Base Properties (IDs 1-24)

    private static void RegisterBaseProperties()
    {
        // ID 1: Defend Chance Increase
        Register(1, new ItemPropertyInfo(
            AosAttribute.DefendChance, 1075620, 110,
            typeof(RelicFragment), typeof(Tourmaline), typeof(EssenceSingularity),
            1, 1, 15, 1111947,
            new PropCategoryInfo(1, 15, 15, new[] { 20 }),
            new PropCategoryInfo(2, 25, 25, new[] { 30, 35 }),
            new PropCategoryInfo(3, 0, 5),
            new PropCategoryInfo(4, 15, 15, new[] { 20 }),
            new PropCategoryInfo(5, 0, 5),
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        // ID 2: Hit Chance Increase
        Register(2, new ItemPropertyInfo(
            AosAttribute.AttackChance, 1075616, 130,
            typeof(EnchantedEssence), typeof(Tourmaline), typeof(EssencePrecision),
            1, 1, 15, 1111958,
            new PropCategoryInfo(1, 15, 15, new[] { 20 }),
            new PropCategoryInfo(2, 25, 25, new[] { 30, 35 }),
            new PropCategoryInfo(3, 0, 5),
            new PropCategoryInfo(4, 15, 15, new[] { 20 }),
            new PropCategoryInfo(5, 0, 5),
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        // ID 3: Hit Point Regeneration
        Register(3, new ItemPropertyInfo(
            AosAttribute.RegenHits, 1075627, 100,
            typeof(EnchantedEssence), typeof(Tourmaline), typeof(SeedOfRenewal),
            1, 1, 2, 1111994,
            new PropCategoryInfo(1, 3, 0, 9),
            new PropCategoryInfo(2, 3, 0, 9),
            new PropCategoryInfo(3, 2, 2, new[] { 4 }),
            new PropCategoryInfo(4, 0, 2, new[] { 4 }),
            new PropCategoryInfo(5, 2, 2, new[] { 4 })));

        // ID 4: Stamina Regeneration
        Register(4, new ItemPropertyInfo(
            AosAttribute.RegenStam, 1079411, 100,
            typeof(EnchantedEssence), typeof(Diamond), typeof(SeedOfRenewal),
            1, 1, 3, 1112043,
            new PropCategoryInfo(1, 3, 0, 9),
            new PropCategoryInfo(2, 3, 0, 9),
            new PropCategoryInfo(3, 3, 3, new[] { 4 }),
            new PropCategoryInfo(4, 0, 3, new[] { 4 }),
            new PropCategoryInfo(5, 3, 3, new[] { 4 })));

        // ID 5: Mana Regeneration
        Register(5, new ItemPropertyInfo(
            AosAttribute.RegenMana, 1079410, 100,
            typeof(EnchantedEssence), typeof(Sapphire), typeof(SeedOfRenewal),
            1, 1, 2, 1112003,
            new PropCategoryInfo(1, 3, 0, 9),
            new PropCategoryInfo(2, 3, 0, 9),
            new PropCategoryInfo(3, 2, 2, new[] { 4 }),
            new PropCategoryInfo(4, 0, 2, new[] { 4 }),
            new PropCategoryInfo(5, 2, 2, new[] { 4 }),
            new PropCategoryInfo(6, 0, 2, 4)));

        // ID 6: Strength Bonus
        Register(6, new ItemPropertyInfo(
            AosAttribute.BonusStr, 1079767, 110,
            typeof(EnchantedEssence), typeof(Diamond), typeof(FireRuby),
            1, 1, 8, 1112044,
            new PropCategoryInfo(1, 0, 5),
            new PropCategoryInfo(2, 0, 5),
            new PropCategoryInfo(3, 0, 5),
            new PropCategoryInfo(4, 0, 5),
            new PropCategoryInfo(5, 0, 5),
            new PropCategoryInfo(6, 8, 8, new[] { 9, 10 })));

        // ID 7: Dexterity Bonus
        Register(7, new ItemPropertyInfo(
            AosAttribute.BonusDex, 1079732, 110,
            typeof(EnchantedEssence), typeof(Ruby), typeof(BlueDiamond),
            1, 1, 8, 1111948,
            new PropCategoryInfo(1, 0, 5),
            new PropCategoryInfo(2, 0, 5),
            new PropCategoryInfo(3, 0, 5),
            new PropCategoryInfo(4, 0, 5),
            new PropCategoryInfo(5, 0, 5),
            new PropCategoryInfo(6, 8, 8, new[] { 9, 10 })));

        // ID 8: Intelligence Bonus
        Register(8, new ItemPropertyInfo(
            AosAttribute.BonusInt, 1079756, 110,
            typeof(EnchantedEssence), typeof(Tourmaline), typeof(Turquoise),
            1, 1, 8, 1111995,
            new PropCategoryInfo(1, 0, 5),
            new PropCategoryInfo(2, 0, 5),
            new PropCategoryInfo(3, 0, 5),
            new PropCategoryInfo(4, 0, 5),
            new PropCategoryInfo(5, 0, 5),
            new PropCategoryInfo(6, 8, 8, new[] { 9, 10 })));

        // ID 9: Hit Points Increase
        Register(9, new ItemPropertyInfo(
            AosAttribute.BonusHits, 1075630, 110,
            typeof(EnchantedEssence), typeof(Ruby), typeof(LuminescentFungi),
            1, 1, 5, 1111993,
            new PropCategoryInfo(1, 0, 5, new[] { 6, 7 }),
            new PropCategoryInfo(2, 0, 5, new[] { 6, 7 }),
            new PropCategoryInfo(3, 5, 5, new[] { 6, 7 }),
            new PropCategoryInfo(4, 0, 5, new[] { 6, 7 }),
            new PropCategoryInfo(5, 5, 5, new[] { 6, 7 })));

        // ID 10: Stamina Increase
        Register(10, new ItemPropertyInfo(
            AosAttribute.BonusStam, 1075632, 110,
            typeof(EnchantedEssence), typeof(Diamond), typeof(LuminescentFungi),
            1, 1, 8, 1112042,
            new PropCategoryInfo(1, 0, 5),
            new PropCategoryInfo(2, 0, 5),
            new PropCategoryInfo(3, 8, 8, new[] { 9, 10 }),
            new PropCategoryInfo(4, 0, 5),
            new PropCategoryInfo(5, 8, 8, new[] { 9, 10 }),
            new PropCategoryInfo(6, 0, 5)));

        // ID 11: Mana Increase
        Register(11, new ItemPropertyInfo(
            AosAttribute.BonusMana, 1075631, 110,
            typeof(EnchantedEssence), typeof(Sapphire), typeof(LuminescentFungi),
            1, 1, 8, 1112002,
            new PropCategoryInfo(1, 0, 5),
            new PropCategoryInfo(2, 0, 5),
            new PropCategoryInfo(3, 8, 8, new[] { 9, 10 }),
            new PropCategoryInfo(4, 0, 5),
            new PropCategoryInfo(5, 8, 8, new[] { 9, 10 }),
            new PropCategoryInfo(6, 0, 5)));

        // ID 12: Damage Increase
        Register(12, new ItemPropertyInfo(
            AosAttribute.WeaponDamage, 1079399, 100,
            typeof(EnchantedEssence), typeof(Citrine), typeof(CrystalShards),
            5, 1, 50, 1112005,
            new PropCategoryInfo(1, 50, 50, new[] { 55, 60, 65, 70 }),
            new PropCategoryInfo(2, 50, 50, new[] { 55, 60, 65, 70 }),
            new PropCategoryInfo(4, 0, 35),
            new PropCategoryInfo(6, 25, 25, new[] { 30, 35 })));

        // ID 13: Swing Speed Increase
        Register(13, new ItemPropertyInfo(
            AosAttribute.WeaponSpeed, 1075629, 110,
            typeof(RelicFragment), typeof(Tourmaline), typeof(EssenceControl),
            5, 5, 30, 1112045,
            new PropCategoryInfo(1, 30, 30, new[] { 35, 40 }),
            new PropCategoryInfo(4, 0, 5, new[] { 10 }),
            new PropCategoryInfo(6, 0, 5, new[] { 10 })));

        // ID 14: Spell Damage Increase
        Register(14, new ItemPropertyInfo(
            AosAttribute.SpellDamage, 1075628, 100,
            typeof(EnchantedEssence), typeof(Emerald), typeof(CrystalShards),
            1, 1, 12, 1112041,
            new PropCategoryInfo(6, 12, 12, new[] { 14, 16, 18 })));

        // ID 15: Faster Cast Recovery
        Register(15, new ItemPropertyInfo(
            AosAttribute.CastRecovery, 1075618, 120,
            typeof(RelicFragment), typeof(Amethyst), typeof(EssenceDiligence),
            1, 1, 3, 1111952,
            new PropCategoryInfo(6, 3, 3, new[] { 4 })));

        // ID 16: Faster Casting
        Register(16, new ItemPropertyInfo(
            AosAttribute.CastSpeed, 1075617, 140,
            typeof(RelicFragment), typeof(Ruby), typeof(EssenceAchievement),
            0, 1, 1, 1111951,
            new PropCategoryInfo(1, 1, 1),
            new PropCategoryInfo(2, 1, 1),
            new PropCategoryInfo(4, 1, 1),
            new PropCategoryInfo(6, 1, 1)));

        // ID 17: Lower Mana Cost
        Register(17, new ItemPropertyInfo(
            AosAttribute.LowerManaCost, 1075621, 110,
            typeof(RelicFragment), typeof(Tourmaline), typeof(EssenceOrder),
            1, 1, 8, 1111996,
            new PropCategoryInfo(1, 0, 5),
            new PropCategoryInfo(2, 0, 5),
            new PropCategoryInfo(3, 8, 8, new[] { 10 }),
            new PropCategoryInfo(4, 0, 5),
            new PropCategoryInfo(5, 8, 8, new[] { 10 }),
            new PropCategoryInfo(6, 8, 8, new[] { 10 })));

        // ID 18: Lower Reagent Cost
        Register(18, new ItemPropertyInfo(
            AosAttribute.LowerRegCost, 1075625, 100,
            typeof(MagicalResidue), typeof(Amber), typeof(FaeryDust),
            1, 1, 20, 1111997,
            new PropCategoryInfo(3, 20, 20, new[] { 25 }),
            new PropCategoryInfo(5, 20, 20, new[] { 25 }),
            new PropCategoryInfo(6, 20, 20, new[] { 25 })));

        // ID 19: Reflect Physical Damage
        Register(19, new ItemPropertyInfo(
            AosAttribute.ReflectPhysical, 1075626, 100,
            typeof(MagicalResidue), typeof(Citrine), typeof(ReflectiveWolfEye),
            1, 1, 15, 1112006,
            new PropCategoryInfo(1, 0, 15),
            new PropCategoryInfo(2, 0, 15),
            new PropCategoryInfo(3, 15, 15, new[] { 20 }),
            new PropCategoryInfo(4, 15, 15, new[] { 20 }),
            new PropCategoryInfo(5, 15, 15, new[] { 20 })));

        // ID 20: Enhance Potions
        Register(20, new ItemPropertyInfo(
            AosAttribute.EnhancePotions, 1075624, 100,
            typeof(EnchantedEssence), typeof(Citrine), typeof(CrushedGlass),
            5, 5, 25, 1111950,
            new PropCategoryInfo(1, 0, 15),
            new PropCategoryInfo(2, 0, 15),
            new PropCategoryInfo(3, 0, 5),
            new PropCategoryInfo(5, 0, 5),
            new PropCategoryInfo(6, 25, 25, new[] { 30, 35 })));

        // ID 21: Luck
        Register(21, new ItemPropertyInfo(
            AosAttribute.Luck, 1061153, 100,
            typeof(MagicalResidue), typeof(Citrine), typeof(ChagaMushroom),
            10, 10, 100, 1111999,
            new PropCategoryInfo(1, 100, 100, new[] { 110, 120, 130, 140, 150 }),
            new PropCategoryInfo(2, 120, 120, new[] { 130, 140, 150, 160, 170 }),
            new PropCategoryInfo(3, 100, 100, new[] { 110, 120, 130, 140, 150 }),
            new PropCategoryInfo(4, 100, 100, new[] { 110, 120, 130, 140, 150 }),
            new PropCategoryInfo(5, 100, 100, new[] { 110, 120, 130, 140, 150 }),
            new PropCategoryInfo(6, 100, 100, new[] { 110, 120, 130, 140, 150 })));

        // ID 22: Spell Channeling
        Register(22, new ItemPropertyInfo(
            AosAttribute.SpellChanneling, 1079766, 100,
            typeof(MagicalResidue), typeof(Diamond), typeof(SilverSnakeSkin),
            0, 1, 1, 1112040,
            new PropCategoryInfo(1, 1, 1),
            new PropCategoryInfo(2, 1, 1),
            new PropCategoryInfo(4, 1, 1)));

        // ID 23: Night Sight
        Register(23, new ItemPropertyInfo(
            AosAttribute.NightSight, 1015168, 50,
            typeof(MagicalResidue), typeof(Tourmaline), typeof(BottleIchor),
            0, 1, 1, 1112004,
            new PropCategoryInfo(3, 1, 1),
            new PropCategoryInfo(5, 1, 1),
            new PropCategoryInfo(6, 1, 1)));
    }

    #endregion
}

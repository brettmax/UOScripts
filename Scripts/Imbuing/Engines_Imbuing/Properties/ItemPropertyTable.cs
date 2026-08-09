using Server.Items;

namespace Server.Engines.Imbuing;

/// <summary>
/// Property table part 2: IDs 25-61 (Hit effects, Resistances, Special Weapon)
/// </summary>
public static class ItemPropertyTable
{
    public static void RegisterProperties()
    {
        RegisterHitLeechEffects();
        RegisterHitAreaEffects();
        RegisterHitSpellEffects();
        RegisterWeaponSpecialProperties();
        RegisterDurabilityProperties();
        RegisterResistanceProperties();
        RegisterSpecialWeaponProperties();
    }

    private static void RegisterHitLeechEffects()
    {
        // ID 25: Hit Life Leech
        ItemPropertyInfo.Register(25, new ItemPropertyInfo(
            AosWeaponAttribute.HitLeechHits, 1079698, 110,
            typeof(MagicalResidue), typeof(Ruby), typeof(VoidOrb),
            1, 2, 50, 1111964,
            new PropCategoryInfo(1, 10, 50, 50),
            new PropCategoryInfo(2, 10, 50, 50)));

        // ID 26: Hit Stamina Leech
        ItemPropertyInfo.Register(26, new ItemPropertyInfo(
            AosWeaponAttribute.HitLeechStam, 1079707, 100,
            typeof(MagicalResidue), typeof(Diamond), typeof(VoidOrb),
            1, 2, 50, 1111992,
            new PropCategoryInfo(1, 10, 50, 50),
            new PropCategoryInfo(2, 10, 50, 50)));

        // ID 27: Hit Mana Leech
        ItemPropertyInfo.Register(27, new ItemPropertyInfo(
            AosWeaponAttribute.HitLeechMana, 1079701, 110,
            typeof(MagicalResidue), typeof(Sapphire), typeof(VoidOrb),
            1, 2, 50, 1111967,
            new PropCategoryInfo(1, 10, 50, 50),
            new PropCategoryInfo(2, 10, 50, 50)));

        // ID 28: Hit Lower Attack
        ItemPropertyInfo.Register(28, new ItemPropertyInfo(
            AosWeaponAttribute.HitLowerAttack, 1079699, 110,
            typeof(EnchantedEssence), typeof(Emerald), typeof(ParasiticPlant),
            1, 2, 50, 1111965,
            new PropCategoryInfo(1, 50, 50, new[] { 55, 60, 65, 70 }),
            new PropCategoryInfo(2, 50, 50, new[] { 55, 60, 65, 70 })));

        // ID 29: Hit Lower Defense
        ItemPropertyInfo.Register(29, new ItemPropertyInfo(
            AosWeaponAttribute.HitLowerDefend, 1079700, 130,
            typeof(EnchantedEssence), typeof(Tourmaline), typeof(ParasiticPlant),
            1, 2, 50, 1111966,
            new PropCategoryInfo(1, 50, 50, new[] { 55, 60, 65, 70 }),
            new PropCategoryInfo(2, 50, 50, new[] { 55, 60, 65, 70 })));
    }

    private static void RegisterHitAreaEffects()
    {
        // ID 30: Hit Physical Area
        ItemPropertyInfo.Register(30, new ItemPropertyInfo(
            AosWeaponAttribute.HitPhysicalArea, 1079696, 100,
            typeof(MagicalResidue), typeof(Diamond), typeof(RaptorTeeth),
            1, 2, 50, 1111956,
            new PropCategoryInfo(1, 50, 50, new[] { 55, 60, 65, 70 }),
            new PropCategoryInfo(2, 50, 50, new[] { 55, 60, 65, 70 })));

        // ID 31: Hit Fire Area
        ItemPropertyInfo.Register(31, new ItemPropertyInfo(
            AosWeaponAttribute.HitFireArea, 1079695, 100,
            typeof(MagicalResidue), typeof(Ruby), typeof(RaptorTeeth),
            1, 2, 50, 1111955,
            new PropCategoryInfo(1, 50, 50, new[] { 55, 60, 65, 70 }),
            new PropCategoryInfo(2, 50, 50, new[] { 55, 60, 65, 70 })));

        // ID 32: Hit Cold Area
        ItemPropertyInfo.Register(32, new ItemPropertyInfo(
            AosWeaponAttribute.HitColdArea, 1079693, 100,
            typeof(MagicalResidue), typeof(Sapphire), typeof(RaptorTeeth),
            1, 2, 50, 1111953,
            new PropCategoryInfo(1, 50, 50, new[] { 55, 60, 65, 70 }),
            new PropCategoryInfo(2, 50, 50, new[] { 55, 60, 65, 70 })));

        // ID 33: Hit Poison Area
        ItemPropertyInfo.Register(33, new ItemPropertyInfo(
            AosWeaponAttribute.HitPoisonArea, 1079697, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(RaptorTeeth),
            1, 2, 50, 1111957,
            new PropCategoryInfo(1, 50, 50, new[] { 55, 60, 65, 70 }),
            new PropCategoryInfo(2, 50, 50, new[] { 55, 60, 65, 70 })));

        // ID 34: Hit Energy Area
        ItemPropertyInfo.Register(34, new ItemPropertyInfo(
            AosWeaponAttribute.HitEnergyArea, 1079694, 100,
            typeof(MagicalResidue), typeof(Amethyst), typeof(RaptorTeeth),
            1, 2, 50, 1111954,
            new PropCategoryInfo(1, 50, 50, new[] { 55, 60, 65, 70 }),
            new PropCategoryInfo(2, 50, 50, new[] { 55, 60, 65, 70 })));
    }

    private static void RegisterHitSpellEffects()
    {
        // ID 35: Hit Magic Arrow
        ItemPropertyInfo.Register(35, new ItemPropertyInfo(
            AosWeaponAttribute.HitMagicArrow, 1079706, 120,
            typeof(RelicFragment), typeof(Amber), typeof(EssenceFeeling),
            1, 2, 50, 1111963,
            new PropCategoryInfo(1, 50, 50, new[] { 55, 60, 65, 70 }),
            new PropCategoryInfo(2, 50, 50, new[] { 55, 60, 65, 70 })));

        // ID 36: Hit Harm
        ItemPropertyInfo.Register(36, new ItemPropertyInfo(
            AosWeaponAttribute.HitHarm, 1079704, 110,
            typeof(EnchantedEssence), typeof(Emerald), typeof(ParasiticPlant),
            1, 2, 50, 1111961,
            new PropCategoryInfo(1, 50, 50, new[] { 55, 60, 65, 70 }),
            new PropCategoryInfo(2, 50, 50, new[] { 55, 60, 65, 70 })));

        // ID 37: Hit Fireball
        ItemPropertyInfo.Register(37, new ItemPropertyInfo(
            AosWeaponAttribute.HitFireball, 1079703, 140,
            typeof(EnchantedEssence), typeof(Ruby), typeof(FireRuby),
            1, 2, 50, 1111960,
            new PropCategoryInfo(1, 50, 50, new[] { 55, 60, 65, 70 }),
            new PropCategoryInfo(2, 50, 50, new[] { 55, 60, 65, 70 })));

        // ID 38: Hit Lightning
        ItemPropertyInfo.Register(38, new ItemPropertyInfo(
            AosWeaponAttribute.HitLightning, 1079705, 140,
            typeof(RelicFragment), typeof(Amethyst), typeof(EssencePassion),
            1, 2, 50, 1111962,
            new PropCategoryInfo(1, 50, 50, new[] { 55, 60, 65, 70 }),
            new PropCategoryInfo(2, 50, 50, new[] { 55, 60, 65, 70 })));

        // ID 39: Hit Dispel
        ItemPropertyInfo.Register(39, new ItemPropertyInfo(
            AosWeaponAttribute.HitDispel, 1079702, 100,
            typeof(MagicalResidue), typeof(Amber), typeof(SlithTongue),
            1, 2, 50, 1111959,
            new PropCategoryInfo(1, 50, 50, new[] { 55, 60, 65, 70 }),
            new PropCategoryInfo(2, 50, 50, new[] { 55, 60, 65, 70 })));
    }

    private static void RegisterWeaponSpecialProperties()
    {
        // ID 40: Use Best Weapon Skill
        ItemPropertyInfo.Register(40, new ItemPropertyInfo(
            AosWeaponAttribute.UseBestSkill, 1079592, 150,
            typeof(EnchantedEssence), typeof(Amber), typeof(DelicateScales),
            0, 1, 1, 1111946,
            new PropCategoryInfo(1, 1, 1)));

        // ID 41: Mage Weapon
        ItemPropertyInfo.Register(41, new ItemPropertyInfo(
            AosWeaponAttribute.MageWeapon, 1079759, 100,
            typeof(EnchantedEssence), typeof(Emerald), typeof(ArcanicRuneStone),
            1, 1, 10, 1112001,
            new PropCategoryInfo(1, 10, 10, new[] { 11, 12, 13, 14, 15 }),
            new PropCategoryInfo(2, 10, 10, new[] { 11, 12, 13, 14, 15 })));
    }

    private static void RegisterDurabilityProperties()
    {
        // ID 42: Weapon Durability
        ItemPropertyInfo.Register(42, new ItemPropertyInfo(
            AosWeaponAttribute.DurabilityBonus, 1017323, 100,
            typeof(EnchantedEssence), typeof(Diamond), typeof(PowderedIron),
            10, 10, 100, 1111949,
            new PropCategoryInfo(1, 0, 100, new[] { 110, 120, 130, 140, 150 }),
            new PropCategoryInfo(2, 0, 100, new[] { 110, 120, 130, 140, 150 })));

        // ID 43: Armor Durability
        ItemPropertyInfo.Register(43, new ItemPropertyInfo(
            AosArmorAttribute.DurabilityBonus, 1017323, 100,
            typeof(EnchantedEssence), typeof(Diamond), typeof(PowderedIron),
            10, 10, 100, 1111949,
            new PropCategoryInfo(3, 0, 100, new[] { 110, 120, 130, 140, 150 }),
            new PropCategoryInfo(4, 100, 100, new[] { 110, 120, 130, 140, 150 }),
            new PropCategoryInfo(5, 0, 100, new[] { 110, 120, 130, 140, 150 })));

        // ID 44: Weapon Lower Stat Requirement
        ItemPropertyInfo.Register(44, new ItemPropertyInfo(
            AosWeaponAttribute.LowerStatReq, 1079757, 100,
            typeof(EnchantedEssence), typeof(Amethyst), typeof(ElvenFletching),
            10, 10, 100, 1111998,
            new PropCategoryInfo(1, 0, 100),
            new PropCategoryInfo(2, 0, 100)));

        // ID 45: Armor Lower Stat Requirement
        ItemPropertyInfo.Register(45, new ItemPropertyInfo(
            AosArmorAttribute.LowerStatReq, 1079757, 100,
            typeof(EnchantedEssence), typeof(Amethyst), typeof(ElvenFletching),
            10, 10, 100, 1111998,
            new PropCategoryInfo(3, 0, 100),
            new PropCategoryInfo(4, 0, 100),
            new PropCategoryInfo(5, 0, 100)));

        // ID 49: Mage Armor
        ItemPropertyInfo.Register(49, new ItemPropertyInfo(
            AosArmorAttribute.MageArmor, 1079758, 0,
            typeof(EnchantedEssence), typeof(Diamond), typeof(AbyssalCloth),
            0, 1, 1, 1112000,
            new PropCategoryInfo(3, 1, 1)));
    }

    private static void RegisterResistanceProperties()
    {
        // ID 51: Physical Resist
        ItemPropertyInfo.Register(51, new ItemPropertyInfo(
            AosElementAttribute.Physical, 1061158, 100,
            typeof(MagicalResidue), typeof(Diamond), typeof(BouraPelt),
            1, 1, 15, 1112010,
            new PropCategoryInfo(1, 10, 100, 100),
            new PropCategoryInfo(2, 10, 100, 100),
            new PropCategoryInfo(3, 15, 15, new[] { 20, 25, 30 }),
            new PropCategoryInfo(4, 15, 15),
            new PropCategoryInfo(5, 15, 15, new[] { 20, 25, 30 }),
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        // ID 52: Fire Resist
        ItemPropertyInfo.Register(52, new ItemPropertyInfo(
            AosElementAttribute.Fire, 1061159, 100,
            typeof(MagicalResidue), typeof(Ruby), typeof(BouraPelt),
            1, 1, 15, 1112009,
            new PropCategoryInfo(1, 10, 100, 100),
            new PropCategoryInfo(2, 10, 100, 100),
            new PropCategoryInfo(3, 15, 15, new[] { 20, 25, 30 }),
            new PropCategoryInfo(4, 15, 15),
            new PropCategoryInfo(5, 15, 15, new[] { 20, 25, 30 }),
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        // ID 53: Cold Resist
        ItemPropertyInfo.Register(53, new ItemPropertyInfo(
            AosElementAttribute.Cold, 1061160, 100,
            typeof(MagicalResidue), typeof(Sapphire), typeof(BouraPelt),
            1, 1, 15, 1112007,
            new PropCategoryInfo(1, 10, 100, 100),
            new PropCategoryInfo(2, 10, 100, 100),
            new PropCategoryInfo(3, 15, 15, new[] { 20, 25, 30 }),
            new PropCategoryInfo(4, 15, 15),
            new PropCategoryInfo(5, 15, 15, new[] { 20, 25, 30 }),
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        // ID 54: Poison Resist
        ItemPropertyInfo.Register(54, new ItemPropertyInfo(
            AosElementAttribute.Poison, 1061161, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(BouraPelt),
            1, 1, 15, 1112011,
            new PropCategoryInfo(1, 10, 100, 100),
            new PropCategoryInfo(2, 10, 100, 100),
            new PropCategoryInfo(3, 15, 15, new[] { 20, 25, 30 }),
            new PropCategoryInfo(4, 15, 15),
            new PropCategoryInfo(5, 15, 15, new[] { 20, 25, 30 }),
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        // ID 55: Energy Resist
        ItemPropertyInfo.Register(55, new ItemPropertyInfo(
            AosElementAttribute.Energy, 1061162, 100,
            typeof(MagicalResidue), typeof(Amethyst), typeof(BouraPelt),
            1, 1, 15, 1112008,
            new PropCategoryInfo(1, 10, 100, 100),
            new PropCategoryInfo(2, 10, 100, 100),
            new PropCategoryInfo(3, 15, 15, new[] { 20, 25, 30 }),
            new PropCategoryInfo(4, 15, 15),
            new PropCategoryInfo(5, 15, 15, new[] { 20, 25, 30 }),
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));
    }

    private static void RegisterSpecialWeaponProperties()
    {
        // ID 60: Velocity (Ranged)
        ItemPropertyInfo.Register(60, new ItemPropertyInfo(
            "WeaponVelocity", 1080416, 130,
            typeof(RelicFragment), typeof(Tourmaline), typeof(EssenceDirection),
            1, 2, 50, 1112048,
            new PropCategoryInfo(1, 50, 50),
            new PropCategoryInfo(2, 50, 50)));

        // ID 61: Balanced Weapon
        // Allows using potions while wielding two-handed weapons (melee and ranged)
        ItemPropertyInfo.Register(61, new ItemPropertyInfo(
            AosAttribute.BalancedWeapon, 1072792, 150,
            typeof(RelicFragment), typeof(Amber), typeof(EssenceBalance),
            0, 1, 1, 1072792,
            new PropCategoryInfo(1, 1, 1),   // Melee (two-handed)
            new PropCategoryInfo(2, 1, 1))); // Ranged (two-handed)
    }
}

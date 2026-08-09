using Server.Items;

namespace Server.Engines.Imbuing;

/// <summary>
/// Property table part 3: IDs 100+ (Slayers, Skill Bonuses, Special Properties)
/// </summary>
public static class ItemPropertyTable2
{
    public static void RegisterProperties()
    {
        RegisterSlayers();
        RegisterSuperSlayers();
        RegisterSkillBonuses();
        RegisterWeaponResistBonuses();
    }

    private static void RegisterSlayers()
    {
        // Regular Slayers (Weight: 100)
        ItemPropertyInfo.Register(101, new ItemPropertyInfo(
            SlayerName.OrcSlaying, 1079741, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111977,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(102, new ItemPropertyInfo(
            SlayerName.TrollSlaughter, 1079754, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111990,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(103, new ItemPropertyInfo(
            SlayerName.OgreTrashing, 1079739, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111975,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(104, new ItemPropertyInfo(
            SlayerName.DragonSlaying, 1061284, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111970,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(105, new ItemPropertyInfo(
            SlayerName.Terathan, 1079753, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111989,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(106, new ItemPropertyInfo(
            SlayerName.SnakesBane, 1079744, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111980,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(107, new ItemPropertyInfo(
            SlayerName.LizardmanSlaughter, 1079738, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111974,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(108, new ItemPropertyInfo(
            SlayerName.GargoylesFoe, 1079737, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111973,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(111, new ItemPropertyInfo(
            SlayerName.Ophidian, 1079740, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111976,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(112, new ItemPropertyInfo(
            SlayerName.SpidersDeath, 1079746, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111982,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(113, new ItemPropertyInfo(
            SlayerName.ScorpionsBane, 1079743, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111979,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(114, new ItemPropertyInfo(
            SlayerName.FlameDousing, 1079736, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111972,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(115, new ItemPropertyInfo(
            SlayerName.WaterDissipation, 1079755, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111991,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(116, new ItemPropertyInfo(
            SlayerName.Vacuum, 1079733, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111968,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(117, new ItemPropertyInfo(
            SlayerName.ElementalHealth, 1079742, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111978,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(118, new ItemPropertyInfo(
            SlayerName.EarthShatter, 1079735, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111971,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(119, new ItemPropertyInfo(
            SlayerName.BloodDrinking, 1079734, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111969,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(120, new ItemPropertyInfo(
            SlayerName.SummerWind, 1079745, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(WhitePearl),
            0, 1, 1, 1111981,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));
    }

    private static void RegisterSuperSlayers()
    {
        // Super Slayers (Weight: 130)
        ItemPropertyInfo.Register(121, new ItemPropertyInfo(
            SlayerName.Silver, 1079752, 130,
            typeof(RelicFragment), typeof(Ruby), typeof(UndyingFlesh),
            0, 1, 1, 1111988,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(122, new ItemPropertyInfo(
            SlayerName.Repond, 1079750, 130,
            typeof(RelicFragment), typeof(Ruby), typeof(GoblinBlood),
            0, 1, 1, 1111986,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(123, new ItemPropertyInfo(
            SlayerName.ReptilianDeath, 1079751, 130,
            typeof(RelicFragment), typeof(Ruby), typeof(LavaSerpentCrust),
            0, 1, 1, 1111987,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(124, new ItemPropertyInfo(
            SlayerName.Exorcism, 1079748, 130,
            typeof(RelicFragment), typeof(Ruby), typeof(DaemonClaw),
            0, 1, 1, 1111984,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(125, new ItemPropertyInfo(
            SlayerName.ArachnidDoom, 1079747, 130,
            typeof(RelicFragment), typeof(Ruby), typeof(SpiderCarapace),
            0, 1, 1, 1111983,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(126, new ItemPropertyInfo(
            SlayerName.ElementalBan, 1079749, 130,
            typeof(RelicFragment), typeof(Ruby), typeof(VialOfVitriol),
            0, 1, 1, 1111985,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));

        ItemPropertyInfo.Register(127, new ItemPropertyInfo(
            SlayerName.Fey, 1154652, 130,
            typeof(RelicFragment), typeof(Ruby), typeof(FeyWings),
            0, 1, 1, 1154652,
            new PropCategoryInfo(1, 1, 1), new PropCategoryInfo(2, 1, 1)));
    }

    private static void RegisterSkillBonuses()
    {
        // Skill Bonuses (Jewels only, Weight: 140)
        ItemPropertyInfo.Register(151, new ItemPropertyInfo(
            SkillName.Fencing, 1044102, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112012,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(152, new ItemPropertyInfo(
            SkillName.Macing, 1044101, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112013,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(153, new ItemPropertyInfo(
            SkillName.Swords, 1044100, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112016,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(154, new ItemPropertyInfo(
            SkillName.Musicianship, 1044089, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112015,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(155, new ItemPropertyInfo(
            SkillName.Magery, 1044085, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112014,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(156, new ItemPropertyInfo(
            SkillName.Wrestling, 1044103, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112021,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(157, new ItemPropertyInfo(
            SkillName.AnimalTaming, 1044095, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112017,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(158, new ItemPropertyInfo(
            SkillName.SpiritSpeak, 1044092, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112019,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(159, new ItemPropertyInfo(
            SkillName.Tactics, 1044087, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112020,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(160, new ItemPropertyInfo(
            SkillName.Provocation, 1044082, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112018,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(161, new ItemPropertyInfo(
            SkillName.Focus, 1044110, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112024,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(162, new ItemPropertyInfo(
            SkillName.Parry, 1044065, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112026,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(163, new ItemPropertyInfo(
            SkillName.Stealth, 1044107, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112027,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(164, new ItemPropertyInfo(
            SkillName.Meditation, 1044106, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112025,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(165, new ItemPropertyInfo(
            SkillName.AnimalLore, 1044062, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112022,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(166, new ItemPropertyInfo(
            SkillName.Discordance, 1044075, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112023,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(167, new ItemPropertyInfo(
            SkillName.Mysticism, 1044115, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1115213,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(168, new ItemPropertyInfo(
            SkillName.Bushido, 1044112, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112029,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(169, new ItemPropertyInfo(
            SkillName.Necromancy, 1044109, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112031,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(170, new ItemPropertyInfo(
            SkillName.Veterinary, 1044099, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112033,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(171, new ItemPropertyInfo(
            SkillName.Stealing, 1044093, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112032,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(172, new ItemPropertyInfo(
            SkillName.EvalInt, 1044076, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112030,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(173, new ItemPropertyInfo(
            SkillName.Anatomy, 1044061, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112028,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(174, new ItemPropertyInfo(
            SkillName.Peacemaking, 1044069, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112038,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(175, new ItemPropertyInfo(
            SkillName.Ninjitsu, 1044113, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112037,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(176, new ItemPropertyInfo(
            SkillName.Chivalry, 1044111, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112035,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(177, new ItemPropertyInfo(
            SkillName.Archery, 1044091, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112034,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(178, new ItemPropertyInfo(
            SkillName.MagicResist, 1044086, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112039,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(179, new ItemPropertyInfo(
            SkillName.Healing, 1044077, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1112036,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(180, new ItemPropertyInfo(
            SkillName.Throwing, 1044117, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1115212,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(181, new ItemPropertyInfo(
            SkillName.Lumberjacking, 1002100, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1002101,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(182, new ItemPropertyInfo(
            SkillName.Snooping, 1002138, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1002139,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(183, new ItemPropertyInfo(
            SkillName.Mining, 1002111, 140,
            typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock),
            5, 1, 15, 1002112,
            new PropCategoryInfo(6, 15, 15, new[] { 20 })));
    }

    private static void RegisterWeaponResistBonuses()
    {
        // Weapon Resist Bonuses
        ItemPropertyInfo.Register(233, new ItemPropertyInfo(
            AosWeaponAttribute.ResistPhysicalBonus, 1061158, 100,
            typeof(MagicalResidue), typeof(Diamond), typeof(BouraPelt),
            1, 1, 15, 1112010,
            new PropCategoryInfo(1, 15, 15, new[] { 20 }),
            new PropCategoryInfo(2, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(234, new ItemPropertyInfo(
            AosWeaponAttribute.ResistFireBonus, 1061159, 100,
            typeof(MagicalResidue), typeof(Ruby), typeof(BouraPelt),
            1, 1, 15, 1112009,
            new PropCategoryInfo(1, 15, 15, new[] { 20 }),
            new PropCategoryInfo(2, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(235, new ItemPropertyInfo(
            AosWeaponAttribute.ResistColdBonus, 1061160, 100,
            typeof(MagicalResidue), typeof(Sapphire), typeof(BouraPelt),
            1, 1, 15, 1112007,
            new PropCategoryInfo(1, 15, 15, new[] { 20 }),
            new PropCategoryInfo(2, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(236, new ItemPropertyInfo(
            AosWeaponAttribute.ResistPoisonBonus, 1061161, 100,
            typeof(MagicalResidue), typeof(Emerald), typeof(BouraPelt),
            1, 1, 15, 1112011,
            new PropCategoryInfo(1, 15, 15, new[] { 20 }),
            new PropCategoryInfo(2, 15, 15, new[] { 20 })));

        ItemPropertyInfo.Register(237, new ItemPropertyInfo(
            AosWeaponAttribute.ResistEnergyBonus, 1061162, 100,
            typeof(MagicalResidue), typeof(Amethyst), typeof(BouraPelt),
            1, 1, 15, 1112008,
            new PropCategoryInfo(1, 15, 15, new[] { 20 }),
            new PropCategoryInfo(2, 15, 15, new[] { 20 })));
    }
}

using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Imbuing;

/// <summary>
/// Handles dropping imbuing ingredients from creatures.
/// Call from BaseCreature.OnDeath to add ingredient drops.
/// </summary>
public static class ImbuingResourceDrops
{
    /// <summary>
    /// Add imbuing ingredient drops to a creature's corpse based on its type and fame.
    /// </summary>
    public static void AddDrops(BaseCreature creature, Container corpse)
    {
        if (creature == null || corpse == null)
            return;

        // Base chance increases with fame
        var fame = creature.Fame;

        // Special ingredient drops based on creature type
        AddSpecialIngredient(creature, corpse);

        // Essence drops from high-level creatures
        if (fame >= 10000)
        {
            AddEssenceDrop(creature, corpse);
        }
    }

    private static void AddSpecialIngredient(BaseCreature creature, Container corpse)
    {
        var type = creature.GetType();
        var typeName = type.Name.ToLower();

        Item? drop = null;
        var chance = 0.15; // 15% base chance

        // Check creature type for specific drops
        if (typeName.Contains("daemon") || typeName.Contains("demon"))
        {
            if (Utility.RandomDouble() < chance)
            {
                drop = new DaemonClaw();
            }
        }
        else if (typeName.Contains("spider") || typeName.Contains("dread"))
        {
            if (Utility.RandomDouble() < chance)
            {
                drop = new SpiderCarapace();
            }
        }
        else if (typeName.Contains("goblin"))
        {
            if (Utility.RandomDouble() < chance)
            {
                drop = new GoblinBlood();
            }
        }
        else if (typeName.Contains("raptor"))
        {
            if (Utility.RandomDouble() < chance)
            {
                drop = new RaptorTeeth();
            }
        }
        else if (typeName.Contains("slith"))
        {
            if (Utility.RandomDouble() < chance)
            {
                drop = new SlithTongue();
            }
        }
        else if (typeName.Contains("boura"))
        {
            if (Utility.RandomDouble() < chance)
            {
                drop = new BouraPelt();
            }
        }
        else if (typeName.Contains("undead") || typeName.Contains("lich") || typeName.Contains("zombie"))
        {
            if (Utility.RandomDouble() < chance)
            {
                drop = new UndyingFlesh();
            }
        }
        else if (typeName.Contains("lava") || typeName.Contains("serpent"))
        {
            if (Utility.RandomDouble() < chance)
            {
                drop = new LavaSerpentCrust();
            }
        }
        else if (typeName.Contains("void") || typeName.Contains("daemon"))
        {
            if (Utility.RandomDouble() < 0.10) // Rarer
            {
                drop = new VoidOrb();
            }
        }
        else if (typeName.Contains("wolf"))
        {
            if (Utility.RandomDouble() < chance)
            {
                drop = new ReflectiveWolfEye();
            }
        }
        else if (typeName.Contains("snake") || typeName.Contains("silver"))
        {
            if (Utility.RandomDouble() < chance)
            {
                drop = new SilverSnakeSkin();
            }
        }
        else if (typeName.Contains("fairy") || typeName.Contains("pixie") || typeName.Contains("wisp"))
        {
            if (Utility.RandomDouble() < 0.10)
            {
                drop = new FaeryDust();
            }
        }
        else if (typeName.Contains("scorpion"))
        {
            if (Utility.RandomDouble() < chance)
            {
                drop = new VialOfVitriol();
            }
        }
        else if (typeName.Contains("crystal") || typeName.Contains("elemental"))
        {
            if (Utility.RandomDouble() < chance)
            {
                drop = new CrystalShards();
            }
        }

        if (drop != null)
        {
            corpse.DropItem(drop);
        }
    }

    private static void AddEssenceDrop(BaseCreature creature, Container corpse)
    {
        var fame = creature.Fame;
        var typeName = creature.GetType().Name;

        // Specific creature -> specific essence (guaranteed drop)
        Item? specificEssence = GetSpecificEssence(typeName);
        if (specificEssence != null)
        {
            if (Utility.RandomDouble() < 0.25) // 25% chance for specific essence
            {
                corpse.DropItem(specificEssence);
            }
            return; // Don't add random essence if creature has specific drop
        }

        // Generic high-fame creatures get random essence
        var chance = 0.05; // 5% base chance for essence

        // Higher fame = higher chance
        if (fame >= 20000)
        {
            chance = 0.15;
        }
        else if (fame >= 15000)
        {
            chance = 0.10;
        }

        if (Utility.RandomDouble() > chance)
            return;

        // Random essence type (11 total)
        Item essence = Utility.Random(11) switch
        {
            0 => new EssenceSingularity(),
            1 => new EssencePrecision(),
            2 => new EssenceControl(),
            3 => new EssenceDiligence(),
            4 => new EssenceAchievement(),
            5 => new EssenceFeeling(),
            6 => new EssenceOrder(),
            7 => new EssencePassion(),
            8 => new EssenceDirection(),
            9 => new EssenceBalance(),
            _ => new EssencePersistence()
        };

        corpse.DropItem(essence);
    }

    /// <summary>
    /// Gets specific essence drop for certain creature types.
    /// Based on ServUO mini-champ regions, adapted for direct creature drops.
    /// </summary>
    private static Item? GetSpecificEssence(string typeName)
    {
        return typeName switch
        {
            // Skeletal Dragon -> Essence of Persistence
            "SkeletalDragon" => new EssencePersistence(),

            // Ancient Lich -> Essence of Direction
            "AncientLich" or "AncientLichRenowned" => new EssenceDirection(),

            // Fire creatures -> Essence of Passion
            "FireDaemon" or "LavaElemental" or "FireElemental" or "Efreet" => new EssencePassion(),

            // Fairy/Nature creatures -> Essence of Feeling
            "FairyDragon" or "Pixie" or "Wisp" or "DarkWisp" => new EssenceFeeling(),

            // Goblins -> Essence of Control
            "GrayGoblin" or "GreenGoblin" or "GrayGoblinMage" or "GreenGoblinAlchemist" => new EssenceControl(),

            // Dragons -> Essence of Singularity
            "GreaterDragon" or "ShadowWyrm" or "AncientWyrm" => new EssenceSingularity(),

            // Precision creatures (ranged/fast) -> Essence of Precision
            "Raptor" or "SilverSerpent" => new EssencePrecision(),

            // Demons/Balrons -> Essence of Achievement
            "Balron" or "Daemon" or "ArchDaemon" => new EssenceAchievement(),

            // Mages/Casters -> Essence of Diligence
            "Lich" or "LichLord" or "EvilMageLord" => new EssenceDiligence(),

            // Order creatures -> Essence of Order
            "OphidianMatriarch" or "OphidianArchmage" => new EssenceOrder(),

            // Boura/Beast -> Essence of Balance
            "HighPlainsBoura" or "ChiefParoxysmus" => new EssenceBalance(),

            _ => null
        };
    }

    /// <summary>
    /// Add rare gem drops to a creature's corpse.
    /// Called from gem mining or certain high-level creatures.
    /// </summary>
    public static void AddRareGemDrop(Container corpse, double chance = 0.05)
    {
        if (Utility.RandomDouble() > chance)
            return;

        Item gem = Utility.Random(7) switch
        {
            0 => new FireRuby(),
            1 => new BlueDiamond(),
            2 => new Turquoise(),
            3 => new DarkSapphire(),
            4 => new PerfectEmerald(),
            5 => new EcruCitrine(),
            _ => new BrilliantAmber()
        };

        corpse.DropItem(gem);
    }

    /// <summary>
    /// Add plant-based imbuing ingredients from gardening/harvesting.
    /// </summary>
    public static Item? GetHarvestDrop()
    {
        if (Utility.RandomDouble() > 0.10)
            return null;

        return Utility.Random(4) switch
        {
            0 => new ParasiticPlant(),
            1 => new LuminescentFungi(),
            2 => new SeedOfRenewal(),
            _ => new ChagaMushroom()
        };
    }
}

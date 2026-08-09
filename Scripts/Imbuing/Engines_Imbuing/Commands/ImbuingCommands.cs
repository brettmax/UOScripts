using Server.Commands;
using Server.Items;
using Server.Targeting;

namespace Server.Engines.Imbuing;

public static class ImbuingCommands
{
    public static void Initialize()
    {
        CommandSystem.Register("ImbuingKit", AccessLevel.GameMaster, ImbuingKit_OnCommand);
    }

    [Usage("ImbuingKit")]
    [Description("Adds 999 of each imbuing ingredient to your backpack.")]
    private static void ImbuingKit_OnCommand(CommandEventArgs e)
    {
        var from = e.Mobile;
        var pack = from.Backpack;

        if (pack == null)
        {
            from.SendMessage("You have no backpack!");
            return;
        }

        var count = 0;

        // Basic unravel resources
        pack.DropItem(new MagicalResidue(999)); count++;
        pack.DropItem(new EnchantedEssence(999)); count++;
        pack.DropItem(new RelicFragment(999)); count++;

        // Special ingredients
        pack.DropItem(new SeedOfRenewal(999)); count++;
        pack.DropItem(new CrystalShards(999)); count++;
        pack.DropItem(new BottleIchor(999)); count++;
        pack.DropItem(new ReflectiveWolfEye(999)); count++;
        pack.DropItem(new FaeryDust(999)); count++;
        pack.DropItem(new BouraPelt(999)); count++;
        pack.DropItem(new SilverSnakeSkin(999)); count++;
        pack.DropItem(new SlithTongue(999)); count++;
        pack.DropItem(new VoidOrb(999)); count++;
        pack.DropItem(new RaptorTeeth(999)); count++;
        pack.DropItem(new SpiderCarapace(999)); count++;
        pack.DropItem(new DaemonClaw(999)); count++;
        pack.DropItem(new VialOfVitriol(999)); count++;
        pack.DropItem(new GoblinBlood(999)); count++;
        pack.DropItem(new LavaSerpentCrust(999)); count++;
        pack.DropItem(new UndyingFlesh(999)); count++;
        pack.DropItem(new CrushedGlass(999)); count++;
        pack.DropItem(new CrystallineBlackrock(999)); count++;
        pack.DropItem(new PowderedIron(999)); count++;
        pack.DropItem(new ElvenFletching(999)); count++;
        pack.DropItem(new DelicateScales(999)); count++;
        pack.DropItem(new ParasiticPlant(999)); count++;
        pack.DropItem(new LuminescentFungi(999)); count++;
        pack.DropItem(new ChagaMushroom(999)); count++;
        pack.DropItem(new Lodestone(999)); count++;

        // Essences
        pack.DropItem(new EssenceSingularity(999)); count++;
        pack.DropItem(new EssenceBalance(999)); count++;
        pack.DropItem(new EssencePassion(999)); count++;
        pack.DropItem(new EssenceDirection(999)); count++;
        pack.DropItem(new EssencePrecision(999)); count++;
        pack.DropItem(new EssenceControl(999)); count++;
        pack.DropItem(new EssenceDiligence(999)); count++;
        pack.DropItem(new EssenceAchievement(999)); count++;
        pack.DropItem(new EssenceFeeling(999)); count++;
        pack.DropItem(new EssenceOrder(999)); count++;
        pack.DropItem(new EssencePersistence(999)); count++;

        // Rare gems
        pack.DropItem(new FireRuby(999)); count++;
        pack.DropItem(new BlueDiamond(999)); count++;
        pack.DropItem(new Turquoise(999)); count++;
        pack.DropItem(new DarkSapphire(999)); count++;
        pack.DropItem(new PerfectEmerald(999)); count++;
        pack.DropItem(new EcruCitrine(999)); count++;
        pack.DropItem(new BrilliantAmber(999)); count++;
        pack.DropItem(new WhitePearl(999)); count++;

        // Standard gems (used in some recipes)
        pack.DropItem(new Amber(999)); count++;
        pack.DropItem(new Citrine(999)); count++;
        pack.DropItem(new Ruby(999)); count++;
        pack.DropItem(new Tourmaline(999)); count++;
        pack.DropItem(new Amethyst(999)); count++;
        pack.DropItem(new Emerald(999)); count++;
        pack.DropItem(new Sapphire(999)); count++;
        pack.DropItem(new StarSapphire(999)); count++;
        pack.DropItem(new Diamond(999)); count++;

        from.SendMessage(0x35, $"Added {count} stacks of imbuing ingredients (999 each) to your backpack!");
    }
}

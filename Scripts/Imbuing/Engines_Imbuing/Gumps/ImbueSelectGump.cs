using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Imbuing;

/// <summary>
/// Property selection gump for Imbuing - shows categories and available properties.
/// </summary>
public class ImbueSelectGump : DynamicGump
{
    private const int LabelColor = 0x7FFF;

    private readonly PlayerMobile _player;
    private readonly Item _item;

    public override bool Singleton => true;

    public ImbueSelectGump(PlayerMobile pm, Item item) : base(50, 50)
    {
        _player = pm;
        _item = item;

        pm.CloseGump<ImbuingGump>();
        pm.CloseGump<ImbueGump>();
    }

    protected override void BuildLayout(ref DynamicGumpBuilder builder)
    {
        // SoulForge Check
        if (!Imbuing.CheckSoulForge(_player, 2))
        {
            return;
        }

        var context = Imbuing.GetContext(_player);
        context.LastImbued = _item;

        var itemType = ItemPropertyInfo.GetItemType(_item);
        var twoHanded = _item.Layer == Layer.TwoHanded;
        var itemRef = (int)itemType;

        builder.AddPage();
        builder.AddBackground(0, 0, 520, 520, 5054);
        builder.AddImageTiled(10, 10, 500, 500, 2624);
        builder.AddImageTiled(10, 30, 500, 10, 5058);
        builder.AddImageTiled(230, 40, 10, 440, 5058);
        builder.AddImageTiled(10, 480, 500, 10, 5058);

        builder.AddAlphaRegion(10, 10, 500, 500);

        // IMBUING MENU
        builder.AddHtmlLocalized(10, 12, 520, 20, 1079588, LabelColor);

        var yOffset = 0;

        // === Attribute Categories ===
        builder.AddHtmlLocalized(10, 60, 220, 20, 1044010, LabelColor);  // <CENTER>CATEGORIES</CENTER>
        builder.AddHtmlLocalized(240, 60, 270, 20, 1044011, LabelColor); // <CENTER>SELECTIONS</CENTER>

        // Casting - always available
        builder.AddButton(15, 90 + yOffset * 25, 4005, 4007, 10001);
        builder.AddHtmlLocalized(50, 90 + yOffset * 25, 150, 18, 1114248, LabelColor); // Casting
        yOffset++;

        // Combat - weapons, shields, jewelry
        if (itemRef is 1 or 2 or 4 or 6)
        {
            builder.AddButton(15, 90 + yOffset * 25, 4005, 4007, 10002);
            builder.AddHtmlLocalized(50, 90 + yOffset * 25, 150, 18, 1114249, LabelColor); // Combat
            yOffset++;
        }

        // Hit Area Effects - weapons only
        if (itemRef is 1 or 2)
        {
            builder.AddButton(15, 90 + yOffset * 25, 4005, 4007, 10006);
            builder.AddHtmlLocalized(50, 90 + yOffset * 25, 150, 18, 1114250, LabelColor); // Hit Area Effects
            yOffset++;

            builder.AddButton(15, 90 + yOffset * 25, 4005, 4007, 10007);
            builder.AddHtmlLocalized(50, 90 + yOffset * 25, 150, 18, 1114251, LabelColor); // Hit Effects
            yOffset++;
        }

        // Misc - always available
        builder.AddButton(15, 90 + yOffset * 25, 4005, 4007, 10003);
        builder.AddHtmlLocalized(50, 90 + yOffset * 25, 150, 18, 1114252, LabelColor); // Misc.
        yOffset++;

        // Ranged - ranged weapons only
        if (itemRef == 2)
        {
            builder.AddButton(15, 90 + yOffset * 25, 4005, 4007, 10015);
            builder.AddHtmlLocalized(50, 90 + yOffset * 25, 150, 18, 1114253, LabelColor); // Ranged
            yOffset++;
        }

        // Resists - weapons, armor, jewelry
        if (itemRef is 1 or 2 or 3 or 5 or 6)
        {
            builder.AddButton(15, 90 + yOffset * 25, 4005, 4007, 10004);
            builder.AddHtmlLocalized(50, 90 + yOffset * 25, 150, 18, 1114254, LabelColor); // Resists
            yOffset++;
        }

        // Slayers - weapons only
        if (itemRef is 1 or 2)
        {
            builder.AddButton(15, 90 + yOffset * 25, 4005, 4007, 10008);
            builder.AddHtmlLocalized(50, 90 + yOffset * 25, 150, 18, 1114263, LabelColor); // Slayers
            yOffset++;

            builder.AddButton(15, 90 + yOffset * 25, 4005, 4007, 10009);
            builder.AddHtmlLocalized(50, 90 + yOffset * 25, 150, 18, 1114264, LabelColor); // Super Slayers
            yOffset++;
        }

        // Skill Groups - jewelry only
        if (itemRef == 6)
        {
            for (var i = 0; i < 5; i++)
            {
                builder.AddButton(15, 90 + yOffset * 25, 4005, 4007, 10010 + i);
                builder.AddHtmlLocalized(50, 90 + yOffset * 25, 150, 18, 1114255 + i, LabelColor); // Skill Group 1-5
                yOffset++;
            }
        }

        // Stats - armor, hats, jewelry
        if (itemRef is 3 or 5 or 6)
        {
            builder.AddButton(15, 90 + yOffset * 25, 4005, 4007, 10005);
            builder.AddHtmlLocalized(50, 90 + yOffset * 25, 150, 18, 1114262, LabelColor); // Stats
            yOffset++;
        }

        // === Property Selections based on category ===
        yOffset = 0;
        var menuCat = context.ImbMenu_Cat;

        BuildCategorySelections(ref builder, menuCat, itemRef, twoHanded, ref yOffset);

        // Cancel button
        builder.AddButton(15, 490, 4005, 4007, 1);
        builder.AddHtmlLocalized(50, 490, 150, 20, 1011012, LabelColor); // Cancel
    }

    private void BuildCategorySelections(
        ref DynamicGumpBuilder builder, int menuCat, int itemRef, bool twoHanded, ref int yOffset)
    {
        switch (menuCat)
        {
            case 1: // CASTING
                BuildCastingSelections(ref builder, itemRef, ref yOffset);
                break;
            case 2: // COMBAT
                BuildCombatSelections(ref builder, itemRef, twoHanded, ref yOffset);
                break;
            case 3: // MISC
                BuildMiscSelections(ref builder, itemRef, ref yOffset);
                break;
            case 4: // RESISTS
                BuildResistSelections(ref builder, ref yOffset);
                break;
            case 5: // STATS
                BuildStatSelections(ref builder, itemRef, ref yOffset);
                break;
            case 6: // HIT AREA EFFECTS
                BuildHitAreaSelections(ref builder, ref yOffset);
                break;
            case 7: // ON HIT EFFECTS
                BuildHitEffectSelections(ref builder, ref yOffset);
                break;
            case 8: // SLAYERS
                BuildSlayerSelections(ref builder, ref yOffset);
                break;
            case 9: // SUPER SLAYERS
                BuildSuperSlayerSelections(ref builder, ref yOffset);
                break;
            case >= 10 and <= 14: // SKILL GROUPS
                BuildSkillSelections(ref builder, menuCat, ref yOffset);
                break;
            case 15: // RANGED
                BuildRangedSelections(ref builder, twoHanded, ref yOffset);
                break;
        }
    }

    private void BuildCastingSelections(ref DynamicGumpBuilder builder, int itemRef, ref int yOffset)
    {
        if (itemRef is 1 or 2) // Weapons
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10122);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079766, LabelColor); // Spell Channeling
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10141);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079759, LabelColor); // Mage Weapon
            yOffset++;

            if (_item is BaseWeapon weapon &&
                (weapon.Attributes.SpellChanneling == 0 || weapon.Attributes.CastSpeed < 0))
            {
                builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10116);
                builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075617, LabelColor); // Faster Casting
                yOffset++;
            }
        }
        else if (itemRef is 3 or 5) // Armor, Hats
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10118);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075625, LabelColor); // Lower Reg Cost
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10117);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075621, LabelColor); // Lower Mana Cost
            yOffset++;
        }
        else if (itemRef == 4) // Shields
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10122);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079766, LabelColor); // Spell Channeling
            yOffset++;

            if (_item is BaseShield shield &&
                (shield.Attributes.SpellChanneling == 0 || shield.Attributes.CastSpeed < 0))
            {
                builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10116);
                builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075617, LabelColor); // Faster Casting
                yOffset++;
            }
        }
        else if (itemRef == 6) // Jewelry
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10114);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075628, LabelColor); // Spell Damage Increase
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10118);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075625, LabelColor); // Lower Reg Cost
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10117);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075621, LabelColor); // Lower Mana Cost
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10116);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075617, LabelColor); // Faster Casting
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10115);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075618, LabelColor); // Faster Cast Recovery
            yOffset++;
        }
    }

    private void BuildCombatSelections(
        ref DynamicGumpBuilder builder, int itemRef, bool twoHanded, ref int yOffset)
    {
        if (itemRef == 1) // Melee Weapons
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10112);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079399, LabelColor); // Damage Increase
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10113);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075629, LabelColor); // Swing Speed Increase
            yOffset++;

            if (twoHanded)
            {
                builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10161);
                builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1072792, LabelColor); // Balanced
                yOffset++;
            }

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10102);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075616, LabelColor); // Hit Chance Increase
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10101);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075620, LabelColor); // Defense Chance Increase
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10140);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079592, LabelColor); // Use Best Weapon Skill
            yOffset++;
        }
        else if (itemRef == 2) // Ranged Weapons
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10112);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079399, LabelColor); // Damage Increase
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10113);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075629, LabelColor); // Swing Speed Increase
            yOffset++;
        }
        else if (itemRef == 4) // Shields
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10101);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075620, LabelColor); // Defense Chance Increase
            yOffset++;
        }
        else if (itemRef == 6) // Jewelry
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10112);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079399, LabelColor); // Damage Increase
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10102);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075616, LabelColor); // Hit Chance Increase
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10101);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075620, LabelColor); // Defense Chance Increase
            yOffset++;
        }
    }

    private void BuildMiscSelections(ref DynamicGumpBuilder builder, int itemRef, ref int yOffset)
    {
        if (itemRef is 1 or 2) // Weapons
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10121);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1061153, LabelColor); // Luck
            yOffset++;
        }
        else if (itemRef is 3 or 5) // Armor, Hats
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10119);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075626, LabelColor); // Reflect Physical Damage
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10123);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1015168, LabelColor); // Night Sight
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10121);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1061153, LabelColor); // Luck
            yOffset++;
        }
        else if (itemRef == 4) // Shields
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10119);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075626, LabelColor); // Reflect Physical Damage
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10124);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079757, LabelColor); // Lower Requirements
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10142);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1017323, LabelColor); // Durability
            yOffset++;
        }
        else if (itemRef == 6) // Jewelry
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10123);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1015168, LabelColor); // Night Sight
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10121);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1061153, LabelColor); // Luck
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10120);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075624, LabelColor); // Enhance Potions
            yOffset++;
        }
    }

    private void BuildResistSelections(ref DynamicGumpBuilder builder, ref int yOffset)
    {
        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10154);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1061161, LabelColor); // Poison Resist
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10151);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1061158, LabelColor); // Physical Resist
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10152);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1061159, LabelColor); // Fire Resist
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10155);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1061162, LabelColor); // Energy Resist
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10153);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1061160, LabelColor); // Cold Resist
        yOffset++;
    }

    private void BuildStatSelections(ref DynamicGumpBuilder builder, int itemRef, ref int yOffset)
    {
        if (itemRef is 3 or 5) // Armor, Hats
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10110);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075632, LabelColor); // Stamina Increase
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10103);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075627, LabelColor); // Hit Points Regeneration
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10104);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079411, LabelColor); // Stamina Regeneration
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10105);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079410, LabelColor); // Mana Regeneration
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10111);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075631, LabelColor); // Mana Increase
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10109);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075630, LabelColor); // Hit Point Increase
            yOffset++;
        }
        else if (itemRef == 6) // Jewelry
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10106);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079767, LabelColor); // Strength Bonus
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10108);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079756, LabelColor); // Intelligence Bonus
            yOffset++;

            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10107);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079732, LabelColor); // Dexterity Bonus
            yOffset++;
        }
    }

    private void BuildHitAreaSelections(ref DynamicGumpBuilder builder, ref int yOffset)
    {
        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10133);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079697, LabelColor); // Hit Poison Area
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10130);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079696, LabelColor); // Hit Physical Area
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10131);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079695, LabelColor); // Hit Fire Area
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10134);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079694, LabelColor); // Hit Energy Area
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10132);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079693, LabelColor); // Hit Cold Area
        yOffset++;
    }

    private void BuildHitEffectSelections(ref DynamicGumpBuilder builder, ref int yOffset)
    {
        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10126);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079707, LabelColor); // Hit Stam Leech
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10127);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079701, LabelColor); // Hit Mana Leech
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10135);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079706, LabelColor); // Hit Magic Arrow
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10129);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079700, LabelColor); // Hit Lower Defense
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10128);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079699, LabelColor); // Hit Lower Attack
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10138);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079705, LabelColor); // Hit Lightning
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10125);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079698, LabelColor); // Hit Life Leech
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10136);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079704, LabelColor); // Hit Harm
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10137);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079703, LabelColor); // Hit Fireball
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10139);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1079702, LabelColor); // Hit Dispel
        yOffset++;
    }

    private void BuildSlayerSelections(ref DynamicGumpBuilder builder, ref int yOffset)
    {
        // Regular slayers
        var slayers = new (int buttonId, int locId)[]
        {
            (10215, 1079755), // Water Elemental Slayer
            (10202, 1079754), // Troll Slayer
            (10205, 1079753), // Terathan Slayer
            (10212, 1079746), // Spider Slayer
            (10220, 1079745), // Snow Elemental Slayer
            (10206, 1079744), // Snake Slayer
            (10213, 1079743), // Scorpion Slayer
            (10217, 1079742), // Poison Elemental Slayer
            (10201, 1079741), // Orc Slayer
            (10211, 1079740), // Ophidian Slayer
            (10203, 1079739), // Ogre Slayer
            (10207, 1079738), // Lizardman Slayer
            (10208, 1079737), // Gargoyle Slayer
            (10214, 1079736), // Fire Elemental Slayer
            (10218, 1079735), // Earth Elemental Slayer
            (10204, 1061284), // Dragon Slayer
            (10219, 1079734), // Blood Elemental Slayer
            (10216, 1079733), // Air Elemental Slayer
        };

        foreach (var (buttonId, locId) in slayers)
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, buttonId);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, locId, LabelColor);
            yOffset++;
        }
    }

    private void BuildSuperSlayerSelections(ref DynamicGumpBuilder builder, ref int yOffset)
    {
        var superSlayers = new (int buttonId, int locId)[]
        {
            (10221, 1079752), // Undead Slayer
            (10223, 1079751), // Reptile Slayer
            (10222, 1079750), // Repond Slayer
            (10227, 1154652), // Fey Slayer
            (10226, 1079749), // Elemental Slayer
            (10224, 1079748), // Demon Slayer
            (10225, 1079747), // Arachnid Slayer
        };

        foreach (var (buttonId, locId) in superSlayers)
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, buttonId);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, locId, LabelColor);
            yOffset++;
        }
    }

    private void BuildSkillSelections(ref DynamicGumpBuilder builder, int menuCat, ref int yOffset)
    {
        var skillGroups = new (int buttonId, int locId)[][]
        {
            // Group 1 (menuCat == 10)
            new[]
            {
                (10251, 1044102), // Fencing
                (10252, 1044101), // Mace Fighting
                (10253, 1044100), // Swordsmanship
                (10254, 1044089), // Musicianship
                (10255, 1044085), // Magery
            },
            // Group 2 (menuCat == 11)
            new[]
            {
                (10256, 1044103), // Wrestling
                (10257, 1044095), // Animal Taming
                (10258, 1044092), // Spirit Speak
                (10259, 1044087), // Tactics
                (10260, 1044082), // Provocation
            },
            // Group 3 (menuCat == 12)
            new[]
            {
                (10261, 1044110), // Focus
                (10262, 1044065), // Parrying
                (10263, 1044107), // Stealth
                (10264, 1044106), // Meditation
                (10265, 1044062), // Animal Lore
                (10266, 1044075), // Discordance
            },
            // Group 4 (menuCat == 13)
            new[]
            {
                (10267, 1044115), // Mysticism
                (10268, 1044112), // Bushido
                (10269, 1044109), // Necromancy
                (10270, 1044099), // Veterinary
                (10271, 1044093), // Stealing
                (10272, 1044076), // Eval Intelligence
                (10273, 1044061), // Anatomy
            },
            // Group 5 (menuCat == 14)
            new[]
            {
                (10274, 1044069), // Peacemaking
                (10280, 1044117), // Throwing
                (10275, 1044113), // Ninjitsu
                (10276, 1044111), // Chivalry
                (10277, 1044091), // Archery
                (10278, 1044086), // Resist Spells
                (10279, 1044077), // Healing
            },
        };

        var groupIndex = menuCat - 10;
        if (groupIndex >= 0 && groupIndex < skillGroups.Length)
        {
            foreach (var (buttonId, locId) in skillGroups[groupIndex])
            {
                builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, buttonId);
                builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, locId, LabelColor);
                yOffset++;
            }
        }
    }

    private void BuildRangedSelections(ref DynamicGumpBuilder builder, bool twoHanded, ref int yOffset)
    {
        if (twoHanded)
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10160);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1080416, LabelColor); // Velocity
            yOffset++;
        }

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10102);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075616, LabelColor); // Hit Chance Increase
        yOffset++;

        builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10101);
        builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1075620, LabelColor); // Defense Chance Increase
        yOffset++;

        if (twoHanded)
        {
            builder.AddButton(250, 90 + yOffset * 20, 4005, 4007, 10161);
            builder.AddHtmlLocalized(295, 90 + yOffset * 20, 150, 18, 1072792, LabelColor); // Balanced
            yOffset++;
        }
    }

    public override void OnResponse(NetState sender, in RelayInfo info)
    {
        var context = Imbuing.GetContext(_player);

        switch (info.ButtonID)
        {
            case 0:
            case 1:
                _player.EndAction<ImbuingActionLock>();
                break;

            case >= 10001 and <= 10015:
                context.ImbMenu_Cat = info.ButtonID - 10000;
                _player.SendGump(new ImbueSelectGump(_player, _item));
                break;

            default:
                // Property selected - convert button ID to property ID
                var id = info.ButtonID - 10100;

                // Convert AosElementalAttribute to WeaponAttributes for weapons
                if (_item is BaseWeapon && id >= 51 && id <= 55)
                {
                    id += 182;
                }

                context.Imbue_Mod = id;

                if (Imbuing.OnBeforeImbue(_player, context.LastImbued, id, -1))
                {
                    _player.SendGump(new ImbueGump(_player, context.LastImbued, id, -1));
                }
                break;
        }
    }
}

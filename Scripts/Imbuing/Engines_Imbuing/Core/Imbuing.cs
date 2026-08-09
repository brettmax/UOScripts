using System;
using System.Collections.Generic;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Engines.Imbuing;

/// <summary>
/// Marker class for action locks (used with BeginAction/EndAction)
/// </summary>
public class ImbuingActionLock;

/// <summary>
/// Core Imbuing skill handler
/// </summary>
public static class Imbuing
{
    private static readonly Dictionary<Mobile, ImbuingContext> _contextTable = new();

    public static void Initialize()
    {
        SkillInfo.Table[(int)SkillName.Imbuing].Callback = OnUse;
        ItemPropertyInfo.Initialize();
    }

    /// <summary>
    /// Called when the player uses the Imbuing skill
    /// </summary>
    public static TimeSpan OnUse(Mobile from)
    {
        if (!from.Alive)
        {
            from.SendLocalizedMessage(500949); // You can't do that when you're dead.
            return TimeSpan.Zero;
        }

        if (from is PlayerMobile pm)
        {
            pm.CloseGump<ImbuingGump>();
            pm.SendGump(new ImbuingGump(pm));
        }

        return TimeSpan.FromSeconds(1.0);
    }

    #region Context Management

    /// <summary>
    /// Gets the imbuing context for a mobile, creating one if needed
    /// </summary>
    public static ImbuingContext GetContext(Mobile m)
    {
        if (!_contextTable.TryGetValue(m, out var context))
        {
            context = new ImbuingContext(m);
            _contextTable[m] = context;
        }

        return context;
    }

    /// <summary>
    /// Adds or updates the context for a mobile
    /// </summary>
    public static void AddContext(Mobile from, ImbuingContext context)
    {
        _contextTable[from] = context;
    }

    #endregion

    #region Validation

    /// <summary>
    /// Checks if a Soulforge is within range
    /// </summary>
    public static bool CheckSoulForge(Mobile from, int range, bool message = true)
    {
        return CheckSoulForge(from, range, message, false, out _);
    }

    /// <summary>
    /// Checks if a Soulforge is within range and returns bonus
    /// </summary>
    public static bool CheckSoulForge(Mobile from, int range, bool message, bool checkQueen, out double bonus)
    {
        bonus = 0;
        var map = from.Map;

        if (map == null)
            return false;

        var found = false;

        foreach (var item in map.GetItemsInRange(from.Location, range))
        {
            // Soulforge ItemIDs
            if ((item.ItemID >= 0x4277 && item.ItemID <= 0x4286) ||
                (item.ItemID >= 0x4263 && item.ItemID <= 0x4272) ||
                item.ItemID == 0x44C7)
            {
                found = true;
                break;
            }
        }

        if (!found && message)
        {
            from.SendLocalizedMessage(1079787); // You must be near a soulforge to imbue an item.
        }

        // TODO: Add region bonuses (Queen's Palace, Royal City)

        return found;
    }

    /// <summary>
    /// Validates if an item can be imbued
    /// </summary>
    public static bool CanImbueItem(Mobile from, Item item)
    {
        if (!CheckSoulForge(from, 2))
            return false;

        if (item == null || !item.IsChildOf(from.Backpack))
        {
            from.SendLocalizedMessage(1079575); // The item must be in your backpack to imbue it.
            return false;
        }

        if (item.LootType == LootType.Blessed || item.LootType == LootType.Newbied)
        {
            from.SendLocalizedMessage(1080438); // You cannot imbue a blessed item.
            return false;
        }

        if (IsSpecialItem(item))
        {
            from.SendLocalizedMessage(1079576); // You cannot imbue this item.
            return false;
        }

        if (item is BaseJewel && item is not BaseRing && item is not BaseBracelet)
        {
            from.SendLocalizedMessage(1079576); // You cannot imbue this item.
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if an item is a special/artifact item that cannot be imbued
    /// </summary>
    public static bool IsSpecialItem(Item item)
    {
        if (item == null)
            return true;

        // Artifacts cannot be imbued (Low quality = artifacts)
        if (item is BaseWeapon weapon && weapon.Quality == WeaponQuality.Low)
            return true;

        if (item is BaseArmor armor && armor.Quality == ArmorQuality.Low)
            return true;

        // TODO: Add more artifact checks

        return false;
    }

    /// <summary>
    /// Validates if an item can be unraveled
    /// </summary>
    public static bool CanUnravelItem(Mobile from, Item item, bool message = true)
    {
        if (!CheckSoulForge(from, 2, false))
        {
            if (message)
                from.SendLocalizedMessage(1080433); // You must be near a soulforge to magically unravel an item.
            return false;
        }

        if (!item.IsChildOf(from.Backpack))
        {
            if (message)
                from.SendLocalizedMessage(1080424); // The item must be in your backpack to magically unravel it.
            return false;
        }

        if (item.LootType == LootType.Blessed || item.LootType == LootType.Newbied)
        {
            if (message)
                from.SendLocalizedMessage(1080421); // You cannot unravel the magic of a blessed item.
            return false;
        }

        if (item is not BaseWeapon && item is not BaseArmor && item is not BaseJewel && item is not BaseHat)
        {
            if (message)
                from.SendLocalizedMessage(1080425); // You cannot magically unravel this item.
            return false;
        }

        return true;
    }

    #endregion

    #region Weight Calculations

    /// <summary>
    /// Gets the maximum weight allowed for an item
    /// </summary>
    public static int GetMaxWeight(Item item)
    {
        var maxWeight = 450; // Base

        // Check for exceptional quality
        if (item is BaseWeapon weapon)
        {
            if (weapon.Quality == WeaponQuality.Exceptional)
                maxWeight += 50;

            if (weapon is BaseRanged)
                maxWeight += 50;
            else if (weapon.Layer == Layer.TwoHanded)
                maxWeight += 100;
        }
        else if (item is BaseArmor armor)
        {
            if (armor.Quality == ArmorQuality.Exceptional)
                maxWeight += 50;
        }
        else if (item is BaseJewel)
        {
            maxWeight = 500; // Jewels have fixed 500
        }

        return maxWeight;
    }

    /// <summary>
    /// Gets the maximum number of properties allowed
    /// </summary>
    public static int GetMaxProps(Item item)
    {
        return 5; // Fixed at 5
    }

    /// <summary>
    /// Calculates the total number of imbued properties on an item
    /// </summary>
    public static int GetTotalMods(Item item, int excludeId = -1)
    {
        // TODO: Implement proper mod counting
        return 0;
    }

    /// <summary>
    /// Calculates the total weight of all properties on an item
    /// </summary>
    public static int GetTotalWeight(Item item, int excludeId = -1, bool trueWeight = false, bool imbuing = false)
    {
        // TODO: Implement proper weight calculation
        return 0;
    }

    #endregion

    #region Success Calculation

    /// <summary>
    /// Calculates the success chance for imbuing
    /// </summary>
    public static double GetSuccessChance(Mobile from, Item item, int totalItemIntensity, int propIntensity, double bonus)
    {
        var skill = from.Skills[SkillName.Imbuing].Value;
        var resultWeight = totalItemIntensity + propIntensity;

        // Formula coefficients (different for Gargoyle race)
        double a, b, c, w;

        if (from.Race == Race.Gargoyle)
        {
            a = 1362.281555;
            b = 66.32801518;
            c = 235.2223147;
            w = -1481.037561;
        }
        else
        {
            a = 1554.96118;
            b = 53.81743328;
            c = 230.0038452;
            w = -1664.857794;
        }

        // Jewelry bonus
        var jewelryMod = item is BaseJewel ? 0.9162 : 1.0;

        // Complex formula from OSI
        var success = Math.Max(0, Math.Round(Math.Floor(
            20 * skill + 10 * a * Math.Pow(Math.E, b / (resultWeight + c)) +
            10 * w - 2400) / 1000 * jewelryMod + bonus, 3) * 100);

        return success;
    }

    #endregion

    #region Resource Calculations

    /// <summary>
    /// Gets the amount of gems needed for imbuing
    /// </summary>
    public static int GetGemAmount(Item item, int id, int value)
    {
        var max = ItemPropertyInfo.GetMaxIntensity(item, id, true);
        if (max <= 0) return 1;

        var v = Math.Floor(value / ((double)max / 10));
        return Math.Clamp((int)v, 1, 10);
    }

    /// <summary>
    /// Gets the amount of primary resource needed
    /// </summary>
    public static int GetPrimaryAmount(Item item, int id, int value)
    {
        var max = ItemPropertyInfo.GetMaxIntensity(item, id, true);
        if (max <= 0) return 1;

        var v = Math.Floor(value / ((double)max / 5.0));
        return Math.Clamp((int)v, 1, 5);
    }

    /// <summary>
    /// Gets the amount of special resource needed (0 if below 90% intensity)
    /// </summary>
    public static int GetSpecialAmount(Item item, int id, int value)
    {
        var max = ItemPropertyInfo.GetMaxIntensity(item, id, true);
        if (max <= 0) return 0;

        var intensity = (int)((double)value / max * 100);

        if (intensity >= 100) return 10;
        if (intensity > 90) return intensity - 90;
        return 0;
    }

    #endregion

    #region Imbuing Actions

    /// <summary>
    /// Attempts to imbue an item with a property
    /// </summary>
    public static void TryImbueItem(Mobile from, Item item, int id, int value)
    {
        if (!CheckSoulForge(from, 2, true, false, out var bonus))
            return;

        var info = ItemPropertyInfo.GetInfo(id);
        if (info == null)
            return;

        // Validate resources
        var gemAmount = GetGemAmount(item, id, value);
        var primAmount = GetPrimaryAmount(item, id, value);
        var specAmount = GetSpecialAmount(item, id, value);

        // Check if player has resources
        if (info.GemRes != null && from.Backpack?.GetAmount(info.GemRes) < gemAmount)
        {
            from.SendLocalizedMessage(1079773); // You do not have enough resources to imbue this item.
            return;
        }

        if (info.PrimaryRes != null && from.Backpack?.GetAmount(info.PrimaryRes) < primAmount)
        {
            from.SendLocalizedMessage(1079773);
            return;
        }

        if (specAmount > 0 && info.SpecialRes != null && from.Backpack?.GetAmount(info.SpecialRes) < specAmount)
        {
            from.SendLocalizedMessage(1079773);
            return;
        }

        // Calculate success chance
        var totalWeight = GetTotalWeight(item, id, false, true);
        var propWeight = info.Weight;
        var success = GetSuccessChance(from, item, totalWeight, propWeight, bonus);

        // Try skill check
        var skill = from.Skills[SkillName.Imbuing].Value;
        if (skill < 120)
        {
            var mins = 120 - success * 1.2;
            var maxs = Math.Max(120 / (success / 100), skill);
            from.CheckSkill(SkillName.Imbuing, mins, maxs);
        }

        // Visual effects
        from.PlaySound(0x243);
        from.FixedParticles(0x375A, 1, 17, 9773, 32, 0, EffectLayer.Waist);

        // Test success
        if (success / 100 >= Utility.RandomDouble())
        {
            // Success - consume all resources
            from.Backpack?.ConsumeTotal(info.GemRes, gemAmount);
            from.Backpack?.ConsumeTotal(info.PrimaryRes, primAmount);
            if (specAmount > 0)
                from.Backpack?.ConsumeTotal(info.SpecialRes, specAmount);

            ImbueItem(from, item, id, value);

            from.SendLocalizedMessage(1079775); // You successfully imbue the item!
            from.PlaySound(0x1F5);
        }
        else
        {
            // Failure - consume only primary resource
            from.Backpack?.ConsumeTotal(info.PrimaryRes, primAmount);
            from.SendLocalizedMessage(1079774); // You attempt to imbue the item, but fail.
            from.PlaySound(0x1E4);
        }
    }

    /// <summary>
    /// Actually applies the imbued property to the item
    /// </summary>
    public static void ImbueItem(Mobile from, Item item, int id, int value)
    {
        var info = ItemPropertyInfo.GetInfo(id);
        if (info == null)
            return;

        // Apply property based on attribute type
        if (info.Attribute is AosAttribute attr)
        {
            ApplyAosAttribute(item, attr, value);
        }
        else if (info.Attribute is AosWeaponAttribute weaponAttr)
        {
            ApplyWeaponAttribute(item, weaponAttr, value);
        }
        else if (info.Attribute is AosArmorAttribute armorAttr)
        {
            ApplyArmorAttribute(item, armorAttr, value);
        }
        else if (info.Attribute is AosElementAttribute elemAttr)
        {
            ApplyElementalResist(item, elemAttr, value);
        }
        else if (info.Attribute is SlayerName slayer)
        {
            ApplySlayer(item, slayer);
        }
        else if (info.Attribute is SkillName skill)
        {
            ApplySkillBonus(item, skill, value);
        }

        // Refresh item properties tooltip
        item.InvalidateProperties();

        // TODO: Track TimesImbued on item
    }

    private static void ApplyAosAttribute(Item item, AosAttribute attr, int value)
    {
        // BalancedWeapon works on ALL two-handed weapons via Attributes
        // No special handling needed - falls through to normal weapon.Attributes[attr] = value

        switch (item)
        {
            case BaseWeapon weapon:
                weapon.Attributes[attr] = value;
                break;
            case BaseArmor armor:
                armor.Attributes[attr] = value;
                break;
            case BaseJewel jewel:
                jewel.Attributes[attr] = value;
                break;
            case BaseClothing clothing:
                clothing.Attributes[attr] = value;
                break;
        }
    }

    private static void ApplyWeaponAttribute(Item item, AosWeaponAttribute attr, int value)
    {
        if (item is BaseWeapon weapon)
        {
            weapon.WeaponAttributes[attr] = value;
        }
    }

    private static void ApplyArmorAttribute(Item item, AosArmorAttribute attr, int value)
    {
        if (item is BaseArmor armor)
        {
            armor.ArmorAttributes[attr] = value;
        }
        else if (item is BaseClothing clothing)
        {
            clothing.ClothingAttributes[attr] = value;
        }
    }

    private static void ApplyElementalResist(Item item, AosElementAttribute attr, int value)
    {
        if (item is BaseArmor armor)
        {
            switch (attr)
            {
                case AosElementAttribute.Physical:
                    armor.PhysicalBonus = value;
                    break;
                case AosElementAttribute.Fire:
                    armor.FireBonus = value;
                    break;
                case AosElementAttribute.Cold:
                    armor.ColdBonus = value;
                    break;
                case AosElementAttribute.Poison:
                    armor.PoisonBonus = value;
                    break;
                case AosElementAttribute.Energy:
                    armor.EnergyBonus = value;
                    break;
            }
        }
        else if (item is BaseJewel jewel)
        {
            jewel.Resistances[attr] = value;
        }
    }

    private static void ApplySlayer(Item item, SlayerName slayer)
    {
        if (item is BaseWeapon weapon)
        {
            weapon.Slayer = slayer;
        }
    }

    private static void ApplySkillBonus(Item item, SkillName skill, int value)
    {
        if (item is BaseJewel jewel)
        {
            // Find empty skill bonus slot
            var bonuses = jewel.SkillBonuses;
            for (var i = 0; i < 5; i++)
            {
                if (bonuses.GetSkill(i) == SkillName.Alchemy && bonuses.GetBonus(i) == 0)
                {
                    bonuses.SetValues(i, skill, value);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Check if imbuing can proceed (validation before actually imbuing)
    /// </summary>
    public static bool OnBeforeImbue(Mobile from, Item item, int id, int value)
    {
        return OnBeforeImbue(from, item, id, value, -1, -1, -1, -1);
    }

    /// <summary>
    /// Check if imbuing can proceed with full validation
    /// </summary>
    public static bool OnBeforeImbue(Mobile from, Item item, int id, int value, int totalProps, int maxProps, int totalWeight, int maxWeight)
    {
        if (!CanImbueItem(from, item))
            return false;

        if (!CanImbueProperty(from, item, id))
            return false;

        if (totalProps >= 0 && maxProps >= 0 && totalProps >= maxProps)
        {
            from.SendLocalizedMessage(1079772); // You cannot imbue this item with any more properties.
            return false;
        }

        if (totalWeight >= 0 && maxWeight >= 0 && totalWeight > maxWeight)
        {
            from.SendLocalizedMessage(1079771); // The item weight is too high for imbuing.
            return false;
        }

        return true;
    }

    /// <summary>
    /// Validates if a specific property can be imbued on an item
    /// </summary>
    public static bool CanImbueProperty(Mobile from, Item item, int id)
    {
        if (!ItemPropertyInfo.Table.TryGetValue(id, out var info))
        {
            from.SendLocalizedMessage(1079576); // You cannot imbue this item.
            return false;
        }

        // Check if item type supports this property
        var itemType = ItemPropertyInfo.GetItemType(item);
        if (itemType == ImbuingItemType.Invalid)
        {
            from.SendLocalizedMessage(1079576);
            return false;
        }

        // BalancedWeapon only works on two-handed weapons
        if (info.Attribute is AosAttribute attr && attr == AosAttribute.BalancedWeapon)
        {
            if (item is not BaseWeapon weapon || weapon.Layer != Layer.TwoHanded)
            {
                from.SendLocalizedMessage(1079576); // You cannot imbue this item.
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Gets how many times an item has been imbued
    /// </summary>
    public static int TimesImbued(Item item)
    {
        // TODO: Track TimesImbued on items (requires IImbuableEquipment interface)
        return 0;
    }

    /// <summary>
    /// Gets base resistances for an item
    /// </summary>
    public static int[] GetBaseResists(Item item)
    {
        var resists = new int[5]; // Physical, Fire, Cold, Poison, Energy

        if (item is BaseArmor armor)
        {
            resists[0] = armor.BasePhysicalResistance;
            resists[1] = armor.BaseFireResistance;
            resists[2] = armor.BaseColdResistance;
            resists[3] = armor.BasePoisonResistance;
            resists[4] = armor.BaseEnergyResistance;
        }
        else if (item is BaseClothing clothing)
        {
            // Clothing has no base resists by default
        }

        return resists;
    }

    /// <summary>
    /// Gets the current value for a property ID on an item
    /// </summary>
    public static int GetValueForID(Item item, int id)
    {
        if (!ItemPropertyInfo.Table.TryGetValue(id, out var info))
            return 0;

        var attr = info.Attribute;

        switch (item)
        {
            case BaseWeapon weapon:
                if (attr is AosAttribute aosAttr)
                {
                    // Special handling for BalancedWeapon on ranged weapons
                    if (aosAttr == AosAttribute.BalancedWeapon && weapon is BaseRanged ranged)
                        return ranged.Balanced ? 1 : 0;

                    return weapon.Attributes[aosAttr];
                }
                if (attr is AosWeaponAttribute weaponAttr)
                    return weapon.WeaponAttributes[weaponAttr];
                if (attr is SlayerName slayer)
                    return weapon.Slayer == slayer || weapon.Slayer2 == slayer ? 1 : 0;
                break;

            case BaseArmor armor:
                if (attr is AosAttribute armorAos)
                    return armor.Attributes[armorAos];
                if (attr is AosArmorAttribute armorAttr)
                    return armor.ArmorAttributes[armorAttr];
                if (attr is AosElementAttribute elemAttr)
                    return elemAttr switch
                    {
                        AosElementAttribute.Physical => armor.PhysicalBonus,
                        AosElementAttribute.Fire     => armor.FireBonus,
                        AosElementAttribute.Cold     => armor.ColdBonus,
                        AosElementAttribute.Poison   => armor.PoisonBonus,
                        AosElementAttribute.Energy   => armor.EnergyBonus,
                        _                            => 0
                    };
                break;

            case BaseJewel jewel:
                if (attr is AosAttribute jewelAos)
                    return jewel.Attributes[jewelAos];
                if (attr is AosElementAttribute jewelElem)
                    return jewel.Resistances[jewelElem];
                if (attr is SkillName skill)
                {
                    for (var i = 0; i < 5; i++)
                    {
                        if (jewel.SkillBonuses.GetSkill(i) == skill)
                            return (int)jewel.SkillBonuses.GetBonus(i);
                    }
                }
                break;
        }

        return 0;
    }

    /// <summary>
    /// Gets skill group for skill bonus restrictions
    /// </summary>
    public static SkillName[] GetSkillGroup(SkillName skill)
    {
        // Skills are grouped to prevent stacking similar bonuses
        return skill switch
        {
            // Group 1 - Combat melee/magic
            SkillName.Fencing or SkillName.Macing or SkillName.Swords or SkillName.Musicianship or SkillName.Magery =>
                new[] { SkillName.Fencing, SkillName.Macing, SkillName.Swords, SkillName.Musicianship, SkillName.Magery },

            // Group 2 - Taming/Spirit
            SkillName.Wrestling or SkillName.AnimalTaming or SkillName.SpiritSpeak or SkillName.Tactics or SkillName.Provocation =>
                new[] { SkillName.Wrestling, SkillName.AnimalTaming, SkillName.SpiritSpeak, SkillName.Tactics, SkillName.Provocation },

            // Group 3 - Defensive/Support
            SkillName.Focus or SkillName.Parry or SkillName.Stealth or SkillName.Meditation or SkillName.AnimalLore or SkillName.Discordance =>
                new[] { SkillName.Focus, SkillName.Parry, SkillName.Stealth, SkillName.Meditation, SkillName.AnimalLore, SkillName.Discordance },

            // Group 4 - Advanced magic
            SkillName.Mysticism or SkillName.Bushido or SkillName.Necromancy or SkillName.Veterinary or SkillName.Stealing or SkillName.EvalInt or SkillName.Anatomy =>
                new[] { SkillName.Mysticism, SkillName.Bushido, SkillName.Necromancy, SkillName.Veterinary, SkillName.Stealing, SkillName.EvalInt, SkillName.Anatomy },

            // Group 5 - Misc
            SkillName.Peacemaking or SkillName.Throwing or SkillName.Ninjitsu or SkillName.Chivalry or SkillName.Archery or SkillName.MagicResist or SkillName.Healing =>
                new[] { SkillName.Peacemaking, SkillName.Throwing, SkillName.Ninjitsu, SkillName.Chivalry, SkillName.Archery, SkillName.MagicResist, SkillName.Healing },

            _ => new[] { skill }
        };
    }

    #endregion

    #region Unraveling

    /// <summary>
    /// Unravels an item to extract magical ingredients
    /// </summary>
    public static bool UnravelItem(Mobile from, Item item, bool message = true)
    {
        if (!CanUnravelItem(from, item, message))
            return false;

        var weight = GetTotalWeight(item, -1, false, true);
        var skill = from.Skills[SkillName.Imbuing].Value;

        Item? result = null;
        var amount = 1;

        if (weight >= 480)
        {
            if (skill < 95.0)
            {
                if (message)
                    from.SendLocalizedMessage(1080422); // Your imbuing skill is not high enough to unravel this item.
                return false;
            }

            if (from.CheckSkill(SkillName.Imbuing, 90.1, 120.0))
            {
                result = new RelicFragment();
            }
            else
            {
                result = new EnchantedEssence();
                amount = Utility.RandomMinMax(1, 3);
            }
        }
        else if (weight > 200)
        {
            if (skill < 45.0)
            {
                if (message)
                    from.SendLocalizedMessage(1080422);
                return false;
            }

            if (from.CheckSkill(SkillName.Imbuing, 45.0, 95.0))
            {
                result = new EnchantedEssence();
                amount = Utility.RandomMinMax(1, 2);
            }
            else
            {
                result = new MagicalResidue();
                amount = Utility.RandomMinMax(1, 3);
            }
        }
        else
        {
            if (from.CheckSkill(SkillName.Imbuing, 0.0, 45.0))
            {
                result = new MagicalResidue();
                amount = Utility.RandomMinMax(1, 3);
            }
        }

        if (result != null)
        {
            result.Amount = amount;
            from.AddToBackpack(result);
            from.SendLocalizedMessage(1080429); // You magically unravel the item to produce more resources.
        }
        else
        {
            from.SendLocalizedMessage(1080428); // You failed to unravel the item.
        }

        item.Delete();
        from.PlaySound(0x1F5);

        return true;
    }

    #endregion
}

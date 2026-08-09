using System;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Imbuing;

/// <summary>
/// Imbuing confirmation gump - shows property details, materials needed, and intensity selection.
/// </summary>
public class ImbueGump : DynamicGump
{
    private const int LabelColor = 0x7FFF;
    private const int IceHue = 0x481;
    private const int Green = 0x41;
    private const int Yellow = 0x36;
    private const int DarkYellow = 0x2E;
    private const int Red = 0x26;

    private readonly PlayerMobile _player;
    private readonly Item _item;
    private readonly int _id;
    private int _value;
    private ItemPropertyInfo _info;
    private int _totalItemWeight;
    private int _totalProps;
    private int _maxWeight;

    public override bool Singleton => true;

    public ImbueGump(PlayerMobile pm, Item item, int id, int value) : base(50, 50)
    {
        _player = pm;
        _item = item;
        _id = id;
        _value = value;

        pm.CloseGump<ImbuingGump>();
        pm.CloseGump<ImbueSelectGump>();
    }

    protected override void BuildLayout(ref DynamicGumpBuilder builder)
    {
        if (!Imbuing.CheckSoulForge(_player, 2, true, false, out var bonus))
        {
            return;
        }

        var context = Imbuing.GetContext(_player);

        if (!ItemPropertyInfo.Table.TryGetValue(_id, out _info))
        {
            return;
        }

        var minInt = ItemPropertyInfo.GetMinIntensity(_item, _id);
        var maxInt = ItemPropertyInfo.GetMaxIntensity(_item, _id, true);
        var weight = _info.Weight;

        if (_value < minInt)
        {
            _value = minInt;
        }

        if (_value > maxInt)
        {
            _value = maxInt;
        }

        var currentIntensity = Math.Floor(_value / (double)maxInt * 100);

        // Set context
        context.LastImbued = _item;
        context.Imbue_Mod = _id;
        context.Imbue_ModVal = weight;
        context.ImbMenu_ModInc = ItemPropertyInfo.GetScale(_item, _id);

        // Current Mod Weight
        _totalItemWeight = Imbuing.GetTotalWeight(_item, _id, false, true);
        _totalProps = Imbuing.GetTotalMods(_item, _id);

        if (maxInt <= 1)
        {
            currentIntensity = 100;
        }

        var propWeight = (int)Math.Floor((double)weight / maxInt * _value);

        // Maximum allowed Property Weight & Item Mod Count
        _maxWeight = Imbuing.GetMaxWeight(_item);

        // Times Item has been Imbued
        var timesImbued = Imbuing.TimesImbued(_item);

        // Check Ingredients needed at the current Intensity
        var gemAmount = Imbuing.GetGemAmount(_item, _id, _value);
        var primResAmount = Imbuing.GetPrimaryAmount(_item, _id, _value);
        var specResAmount = Imbuing.GetSpecialAmount(_item, _id, _value);

        builder.AddPage();
        builder.AddBackground(0, 0, 520, 440, 5054);
        builder.AddImageTiled(10, 10, 500, 420, 2624);

        builder.AddImageTiled(10, 30, 500, 10, 5058);
        builder.AddImageTiled(250, 40, 10, 290, 5058);
        builder.AddImageTiled(10, 180, 500, 10, 5058);
        builder.AddImageTiled(10, 330, 500, 10, 5058);
        builder.AddImageTiled(10, 400, 500, 10, 5058);

        builder.AddAlphaRegion(10, 10, 500, 420);

        // <CENTER>IMBUING CONFIRMATION</CENTER>
        builder.AddHtmlLocalized(10, 12, 520, 20, 1079717, LabelColor);
        // PROPERTY INFORMATION
        builder.AddHtmlLocalized(50, 50, 200, 20, 1114269, LabelColor);

        // Property:
        builder.AddHtmlLocalized(25, 80, 80, 20, 1114270, LabelColor);

        if (_info.AttributeName > 0)
        {
            builder.AddHtmlLocalized(95, 80, 150, 20, 1114057, _info.AttributeName.ToString(), LabelColor);
        }

        // Replaces:
        builder.AddHtmlLocalized(25, 100, 80, 20, 1114271, LabelColor);
        var replace = WhatReplacesWhat(_id, _item);

        if (replace != null)
        {
            builder.AddHtmlLocalized(95, 100, 150, 20, 1114057, replace.ToString(), LabelColor);
        }

        // Weight:
        builder.AddHtmlLocalized(25, 120, 80, 20, 1114272, 0xFFFFFF);
        builder.AddLabel(95, 120, IceHue, $"{(double)_info.Weight / 100.0:0.0}x");

        // Intensity:
        builder.AddHtmlLocalized(25, 140, 80, 20, 1114273, LabelColor);
        builder.AddLabel(95, 140, IceHue, $"{currentIntensity}%");

        // <CENTER>MATERIALS</CENTER>
        builder.AddHtmlLocalized(10, 200, 245, 20, 1044055, LabelColor);

        builder.AddHtmlLocalized(40, 230, 180, 20, _info.PrimaryName, LabelColor);
        builder.AddLabel(210, 230, IceHue, primResAmount.ToString());

        builder.AddHtmlLocalized(40, 255, 180, 20, _info.GemName, LabelColor);
        builder.AddLabel(210, 255, IceHue, gemAmount.ToString());

        if (specResAmount > 0)
        {
            builder.AddHtmlLocalized(40, 280, 180, 17, _info.SpecialName, LabelColor);
            builder.AddLabel(210, 280, IceHue, specResAmount.ToString());
        }

        // Mod Description
        builder.AddHtmlLocalized(280, 55, 200, 110, _info.Description, LabelColor);

        // RESULTS
        builder.AddHtmlLocalized(350, 200, 150, 20, 1113650, LabelColor);

        // Properties:
        builder.AddHtmlLocalized(280, 220, 150, 20, 1113645, LabelColor);
        builder.AddLabel(430, 220, GetColor(_totalProps + 1, 5), $"{_totalProps + 1}/{Imbuing.GetMaxProps(_item)}");

        var projWeight = _totalItemWeight + propWeight;
        // Total Property Weight:
        builder.AddHtmlLocalized(280, 240, 150, 20, 1113646, LabelColor);
        builder.AddLabel(430, 240, GetColor(projWeight, _maxWeight), $"{projWeight}/{_maxWeight}");

        // Times Imbued:
        builder.AddHtmlLocalized(280, 260, 150, 20, 1113647, LabelColor);
        builder.AddLabel(430, 260, GetColor(timesImbued, 20), $"{timesImbued}/20");

        // === CALCULATE DIFFICULTY ===
        var truePropWeight = (int)((double)propWeight / weight * 100);
        var trueTotalWeight = Imbuing.GetTotalWeight(_item, -1, true, true);

        var suc = Imbuing.GetSuccessChance(_player, _item, trueTotalWeight, truePropWeight, bonus);

        // Success Chance:
        builder.AddHtmlLocalized(300, 300, 150, 20, 1044057, 0xFFFFFF);
        builder.AddLabel(420, 300, GetSuccessChanceHue(suc), $"{suc:0.0}%");

        // Attribute Level Adjustment
        if (maxInt > 1)
        {
            // New Value:
            builder.AddHtmlLocalized(235, 350, 100, 17, 1062300, LabelColor);

            if (_id == 41) // Mage Weapon
            {
                builder.AddLabel(250, 370, IceHue, $"-{30 - _value}");
            }
            else if (maxInt <= 8 || _id == 21 || _id == 17)
            {
                builder.AddLabel(256, 370, IceHue, $"{_value}");
            }
            else
            {
                var val = _value;

                if (_id >= 51 && _id <= 55)
                {
                    var resistances = Imbuing.GetBaseResists(_item);

                    val += _id switch
                    {
                        51 => resistances[0],
                        52 => resistances[1],
                        53 => resistances[2],
                        54 => resistances[3],
                        55 => resistances[4],
                        _  => 0
                    };
                }

                builder.AddLabel(256, 370, IceHue, $"{val}%");
            }

            // Decrease buttons
            builder.AddButton(179, 372, 0x1464, 0x1464, 10053); // <<<
            builder.AddButton(187, 372, 0x1466, 0x1466, 10053);

            builder.AddButton(199, 372, 0x1464, 0x1464, 10052); // <<
            builder.AddButton(207, 372, 0x1466, 0x1466, 10052);

            builder.AddButton(221, 372, 0x1464, 0x1464, 10051); // <
            builder.AddButton(229, 372, 0x1466, 0x1466, 10051);

            // Increase buttons
            builder.AddButton(280, 372, 0x1464, 0x1464, 10054); // >
            builder.AddButton(288, 372, 0x1466, 0x1466, 10054);

            builder.AddButton(300, 372, 0x1464, 0x1464, 10055); // >>
            builder.AddButton(308, 372, 0x1466, 0x1466, 10055);

            builder.AddButton(320, 372, 0x1464, 0x1464, 10056); // >>>
            builder.AddButton(328, 372, 0x1466, 0x1466, 10056);

            // Labels for buttons
            builder.AddLabel(322, 370, 0, ">");
            builder.AddLabel(326, 370, 0, ">");
            builder.AddLabel(330, 370, 0, ">");

            builder.AddLabel(304, 370, 0, ">");
            builder.AddLabel(308, 370, 0, ">");

            builder.AddLabel(286, 370, 0, ">");

            builder.AddLabel(226, 370, 0, "<");

            builder.AddLabel(203, 370, 0, "<");
            builder.AddLabel(207, 370, 0, "<");

            builder.AddLabel(181, 370, 0, "<");
            builder.AddLabel(185, 370, 0, "<");
            builder.AddLabel(189, 370, 0, "<");
        }

        // Back button
        builder.AddButton(15, 410, 4005, 4007, 10099);
        builder.AddHtmlLocalized(50, 410, 100, 18, 1114268, LabelColor); // Back

        // Imbue Item button
        builder.AddButton(390, 410, 4005, 4007, 10100);
        builder.AddHtmlLocalized(425, 410, 120, 18, 1114267, LabelColor); // Imbue Item
    }

    private static int GetColor(int value, int limit)
    {
        if (value < limit)
        {
            return Green;
        }

        return value == limit ? Yellow : Red;
    }

    private static int GetSuccessChanceHue(double suc)
    {
        return suc switch
        {
            >= 100 => IceHue,
            >= 80  => Green,
            >= 50  => Yellow,
            >= 10  => DarkYellow,
            _      => Red
        };
    }

    public override void OnResponse(NetState sender, in RelayInfo info)
    {
        var context = Imbuing.GetContext(_player);
        var minInt = ItemPropertyInfo.GetMinIntensity(_item, _id);
        var maxInt = ItemPropertyInfo.GetMaxIntensity(_item, _id, true);

        switch (info.ButtonID)
        {
            case 0: // Close
                _player.EndAction<ImbuingActionLock>();
                break;

            case 10051: // Decrease [<]
                _value = Math.Max(minInt, _value - 1);
                _player.SendGump(new ImbueGump(_player, _item, _id, _value));
                break;

            case 10052: // Decrease [<<]
                _value = Math.Max(minInt, _value - 10);
                _player.SendGump(new ImbueGump(_player, _item, _id, _value));
                break;

            case 10053: // Minimum [<<<]
                _value = minInt;
                _player.SendGump(new ImbueGump(_player, _item, _id, _value));
                break;

            case 10054: // Increase [>]
                _value = Math.Min(maxInt, _value + 1);
                _player.SendGump(new ImbueGump(_player, _item, _id, _value));
                break;

            case 10055: // Increase [>>]
                _value = Math.Min(maxInt, _value + 10);
                _player.SendGump(new ImbueGump(_player, _item, _id, _value));
                break;

            case 10056: // Maximum [>>>]
                _value = maxInt;
                _player.SendGump(new ImbueGump(_player, _item, _id, _value));
                break;

            case 10099: // Back
                _player.SendGump(new ImbueSelectGump(_player, context.LastImbued));
                break;

            case 10100: // Imbue Item
                context.Imbue_IWmax = _maxWeight;

                if (Imbuing.OnBeforeImbue(
                        _player, _item, _id, _value, _totalProps, Imbuing.GetMaxProps(_item),
                        _totalItemWeight, _maxWeight))
                {
                    Imbuing.TryImbueItem(_player, _item, _id, _value);
                    SendGumpDelayed(_player);
                }
                break;
        }
    }

    public static void SendGumpDelayed(PlayerMobile pm)
    {
        Timer.StartTimer(TimeSpan.FromSeconds(1.5), () =>
        {
            pm.SendGump(new ImbuingGump(pm));
        });
    }

    /// <summary>
    /// Check if chosen attribute replaces another.
    /// </summary>
    public static TextDefinition? WhatReplacesWhat(int id, Item item)
    {
        if (Imbuing.GetValueForID(item, id) > 0)
        {
            return ItemPropertyInfo.GetAttributeName(id);
        }

        if (item is BaseWeapon weapon)
        {
            // Slayers replace Slayers
            if (id >= 101 && id <= 127)
            {
                if (weapon.Slayer != SlayerName.None)
                {
                    return GetNameForAttribute(weapon.Slayer);
                }

                if (weapon.Slayer2 != SlayerName.None)
                {
                    return GetNameForAttribute(weapon.Slayer2);
                }
            }

            // OnHitEffect replace OnHitEffect
            if (id >= 35 && id <= 39)
            {
                if (weapon.WeaponAttributes.HitMagicArrow > 0)
                {
                    return GetNameForAttribute(AosWeaponAttribute.HitMagicArrow);
                }

                if (weapon.WeaponAttributes.HitHarm > 0)
                {
                    return GetNameForAttribute(AosWeaponAttribute.HitHarm);
                }

                if (weapon.WeaponAttributes.HitFireball > 0)
                {
                    return GetNameForAttribute(AosWeaponAttribute.HitFireball);
                }

                if (weapon.WeaponAttributes.HitLightning > 0)
                {
                    return GetNameForAttribute(AosWeaponAttribute.HitLightning);
                }

                if (weapon.WeaponAttributes.HitDispel > 0)
                {
                    return GetNameForAttribute(AosWeaponAttribute.HitDispel);
                }
            }

            // OnHitArea replace OnHitArea
            if (id >= 30 && id <= 34)
            {
                if (weapon.WeaponAttributes.HitPhysicalArea > 0)
                {
                    return GetNameForAttribute(AosWeaponAttribute.HitPhysicalArea);
                }

                if (weapon.WeaponAttributes.HitColdArea > 0)
                {
                    return GetNameForAttribute(AosWeaponAttribute.HitFireArea);
                }

                if (weapon.WeaponAttributes.HitFireArea > 0)
                {
                    return GetNameForAttribute(AosWeaponAttribute.HitColdArea);
                }

                if (weapon.WeaponAttributes.HitPoisonArea > 0)
                {
                    return GetNameForAttribute(AosWeaponAttribute.HitPoisonArea);
                }

                if (weapon.WeaponAttributes.HitEnergyArea > 0)
                {
                    return GetNameForAttribute(AosWeaponAttribute.HitEnergyArea);
                }
            }
        }

        if (item is BaseJewel jewel)
        {
            if (id >= 151 && id <= 183)
            {
                var bonuses = jewel.SkillBonuses;
                var group = Imbuing.GetSkillGroup((SkillName)ItemPropertyInfo.GetAttribute(id));

                for (var i = 0; i < 5; i++)
                {
                    if (bonuses.GetBonus(i) > 0)
                    {
                        foreach (var sk in group)
                        {
                            if (sk == bonuses.GetSkill(i))
                            {
                                return GetNameForAttribute(bonuses.GetSkill(i));
                            }
                        }
                    }
                }
            }
        }

        return null;
    }

    public static TextDefinition? GetNameForAttribute(object attribute)
    {
        if (attribute is AosArmorAttribute armorAttr)
        {
            if (armorAttr == AosArmorAttribute.LowerStatReq)
            {
                attribute = AosWeaponAttribute.LowerStatReq;
            }

            if (armorAttr == AosArmorAttribute.DurabilityBonus)
            {
                attribute = AosWeaponAttribute.DurabilityBonus;
            }
        }

        foreach (var info in ItemPropertyInfo.Table.Values)
        {
            if (attribute is SlayerName slayer && info.Attribute is SlayerName infoSlayer && slayer == infoSlayer)
            {
                return info.AttributeName;
            }

            if (attribute is AosAttribute aosAttr && info.Attribute is AosAttribute infoAos && aosAttr == infoAos)
            {
                return info.AttributeName;
            }

            if (attribute is AosWeaponAttribute weaponAttr && info.Attribute is AosWeaponAttribute infoWeapon &&
                weaponAttr == infoWeapon)
            {
                return info.AttributeName;
            }

            if (attribute is SkillName skillName && info.Attribute is SkillName infoSkill && skillName == infoSkill)
            {
                return info.AttributeName;
            }

            if (info.Attribute == attribute)
            {
                return info.AttributeName;
            }
        }

        return null;
    }
}

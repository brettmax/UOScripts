using System;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Engines.Imbuing;

/// <summary>
/// Main Imbuing menu gump - displays options for imbuing, unraveling, etc.
/// </summary>
public class ImbuingGump : DynamicGump
{
    private const int LabelColor = 0x7FFF;

    private readonly PlayerMobile _player;

    public override bool Singleton => true;

    public ImbuingGump(PlayerMobile pm) : base(25, 50)
    {
        _player = pm;

        pm.CloseGump<ImbueSelectGump>();
        pm.CloseGump<ImbueGump>();
    }

    protected override void BuildLayout(ref DynamicGumpBuilder builder)
    {
        var context = Imbuing.GetContext(_player);

        context.Imbue_ModVal = 0;
        context.ImbMenu_Cat = 0;

        builder.AddPage();
        builder.AddBackground(0, 0, 520, 310, 5054);
        builder.AddImageTiled(10, 10, 500, 290, 2624);
        builder.AddImageTiled(10, 30, 500, 10, 5058);
        builder.AddImageTiled(10, 270, 500, 10, 5058);
        builder.AddAlphaRegion(10, 10, 500, 290);

        // <CENTER>IMBUING MENU</CENTER>
        builder.AddHtmlLocalized(10, 12, 520, 20, 1079588, LabelColor);

        // Imbue Item
        builder.AddButton(15, 60, 4005, 4007, 10005);
        builder.AddHtmlLocalized(50, 60, 430, 20, 1080432, LabelColor); // Imbue Item - Adds or modifies an item property on an item

        // Reimbue Last
        builder.AddButton(15, 90, 4005, 4007, 10006);
        builder.AddHtmlLocalized(50, 90, 430, 20, 1113622, LabelColor); // Reimbue Last - Repeats the last imbuing attempt

        // Imbue Last Item
        builder.AddButton(15, 120, 4005, 4007, 10007);
        builder.AddHtmlLocalized(50, 120, 430, 20, 1113571, LabelColor); // Imbue Last Item - Auto targets the last imbued item

        // Imbue Last Property
        builder.AddButton(15, 150, 4005, 4007, 10008);
        builder.AddHtmlLocalized(50, 150, 430, 20, 1114274, LabelColor); // Imbue Last Property - Imbues a new item with the last property

        // Unravel Item
        builder.AddButton(15, 180, 4005, 4007, 10010);
        builder.AddHtmlLocalized(50, 180, 470, 20, 1080431, LabelColor); // Unravel Item - Extracts magical ingredients from an item, destroying it

        // Unravel Container
        builder.AddButton(15, 210, 4005, 4007, 10011);
        builder.AddHtmlLocalized(50, 210, 430, 20, 1114275, LabelColor); // Unravel Container - Unravels all items in a container

        // Cancel
        builder.AddButton(15, 280, 4017, 4019, 0);
        builder.AddHtmlLocalized(50, 280, 50, 20, 1011012, LabelColor); // CANCEL
    }

    public override void OnResponse(NetState sender, in RelayInfo info)
    {
        var context = Imbuing.GetContext(_player);

        switch (info.ButtonID)
        {
            case 0: // Close/Cancel
                _player.EndAction<ImbuingActionLock>();
                break;

            case 10005: // Imbue Item
                _player.SendLocalizedMessage(1079589); // Target an item you wish to imbue.
                _player.Target = new ImbueItemTarget();
                break;

            case 10006: // Reimbue Last
                {
                    var item = context.LastImbued;
                    var mod = context.Imbue_Mod;
                    var modInt = context.Imbue_ModInt;

                    if (item == null || mod < 0 || modInt == 0)
                    {
                        _player.SendLocalizedMessage(1113572); // You haven't imbued anything yet!
                        _player.EndAction<ImbuingActionLock>();
                        break;
                    }

                    if (Imbuing.CanImbueItem(_player, item) && Imbuing.OnBeforeImbue(_player, item, mod, modInt))
                    {
                        Imbuing.TryImbueItem(_player, item, mod, modInt);
                        SendGumpDelayed(_player);
                    }
                    break;
                }

            case 10007: // Imbue Last Item
                {
                    var item = context.LastImbued;

                    if (item == null)
                    {
                        _player.SendLocalizedMessage(1113572); // You haven't imbued anything yet!
                        _player.EndAction<ImbuingActionLock>();
                        break;
                    }

                    ImbueStep1(_player, item);
                    break;
                }

            case 10008: // Imbue Last Property
                {
                    context.LastImbued = null;
                    var mod = context.Imbue_Mod;
                    var modInt = context.Imbue_ModInt;

                    if (modInt < 0)
                    {
                        modInt = 0;
                    }

                    if (mod < 0)
                    {
                        _player.SendLocalizedMessage(1113572); // You haven't imbued anything yet!
                        _player.EndAction<ImbuingActionLock>();
                        break;
                    }

                    ImbueLastProp(_player, mod, modInt);
                    break;
                }

            case 10010: // Unravel Item
                _player.SendLocalizedMessage(1080422); // Target an item you wish to magically unravel.
                _player.Target = new UnravelItemTarget();
                break;

            case 10011: // Unravel Container
                _player.SendLocalizedMessage(1080422); // Target an item you wish to magically unravel.
                _player.Target = new UnravelContainerTarget();
                break;
        }
    }

    public static void ImbueStep1(Mobile m, Item item)
    {
        if (m is PlayerMobile pm && Imbuing.CanImbueItem(m, item))
        {
            var context = Imbuing.GetContext(m);
            context.LastImbued = item;

            if (context.ImbMenu_Cat == 0)
            {
                context.ImbMenu_Cat = 1;
            }

            pm.CloseGump<ImbuingGump>();
            pm.SendGump(new ImbueSelectGump(pm, item));
        }
    }

    public static void ImbueLastProp(Mobile m, int mod, int modInt)
    {
        m.Target = new ImbueLastModTarget();
    }

    public static void SendGumpDelayed(PlayerMobile pm)
    {
        Timer.StartTimer(TimeSpan.FromSeconds(1.5), () =>
        {
            pm.SendGump(new ImbuingGump(pm));
        });
    }

    private class ImbueItemTarget : Target
    {
        public ImbueItemTarget() : base(-1, false, TargetFlags.None)
        {
            AllowNonlocal = true;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is not Item item)
            {
                from.SendLocalizedMessage(1079576); // You cannot imbue this item.
                return;
            }

            var itemType = ItemPropertyInfo.GetItemType(item);

            if (itemType == ImbuingItemType.Invalid)
            {
                from.SendLocalizedMessage(1079576); // You cannot imbue this item.
                return;
            }

            ImbueStep1(from, item);
        }

        protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
        {
            from.EndAction<ImbuingActionLock>();
        }
    }

    private class ImbueLastModTarget : Target
    {
        public ImbueLastModTarget() : base(-1, false, TargetFlags.None)
        {
            AllowNonlocal = true;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is not Item item || from is not PlayerMobile pm)
            {
                from.SendLocalizedMessage(1079576); // You cannot imbue this item.
                return;
            }

            var context = Imbuing.GetContext(from);
            var mod = context.Imbue_Mod;
            var modInt = context.Imbue_ModInt;

            if (!Imbuing.CanImbueItem(from, item) || !Imbuing.OnBeforeImbue(from, item, mod, modInt) ||
                !Imbuing.CanImbueProperty(from, item, mod))
            {
                ImbueGump.SendGumpDelayed(pm);
            }
            else
            {
                Imbuing.TryImbueItem(from, item, mod, modInt);
                ImbueGump.SendGumpDelayed(pm);
            }
        }

        protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
        {
            from.EndAction<ImbuingActionLock>();
        }
    }

    private class UnravelItemTarget : Target
    {
        public UnravelItemTarget() : base(-1, false, TargetFlags.None)
        {
            AllowNonlocal = true;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            from.EndAction<ImbuingActionLock>();

            if (targeted is not Item item)
            {
                from.SendLocalizedMessage(1080425); // You cannot magically unravel this item.
                return;
            }

            if (from is PlayerMobile pm && Imbuing.CanUnravelItem(from, item))
            {
                from.BeginAction<ImbuingActionLock>();
                pm.SendGump(new UnravelGump(pm, item));
            }
        }

        protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
        {
            from.EndAction<ImbuingActionLock>();
        }
    }

    private class UnravelContainerTarget : Target
    {
        public UnravelContainerTarget() : base(-1, false, TargetFlags.None)
        {
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            from.EndAction<ImbuingActionLock>();

            if (targeted is not Container cont)
            {
                return;
            }

            if (!cont.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1062334); // This item must be in your backpack to be used.
                from.EndAction<ImbuingActionLock>();
                return;
            }

            if (cont is LockableContainer { Locked: true })
            {
                from.SendLocalizedMessage(1111814, "0\t0"); // Unraveled: ~1_COUNT~/~2_NUM~ items
                from.EndAction<ImbuingActionLock>();
                return;
            }

            if (from is PlayerMobile pm)
            {
                var hasUnravelable = false;

                foreach (var item in cont.Items)
                {
                    if (Imbuing.CanUnravelItem(from, item, false))
                    {
                        hasUnravelable = true;
                        break;
                    }
                }

                if (hasUnravelable)
                {
                    from.BeginAction<ImbuingActionLock>();
                    pm.SendGump(new UnravelContainerGump(pm, cont));
                }
                else
                {
                    TryUnravelContainer(from, cont);
                    from.EndAction<ImbuingActionLock>();
                }
            }
        }

        protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
        {
            from.EndAction<ImbuingActionLock>();
        }

        public static void TryUnravelContainer(Mobile from, Container c)
        {
            foreach (var item in c.Items)
            {
                Imbuing.CanUnravelItem(from, item, true);
            }

            from.SendLocalizedMessage(1111814, $"0\t{c.Items.Count}"); // Unraveled: ~1_COUNT~/~2_NUM~ items
        }
    }
}

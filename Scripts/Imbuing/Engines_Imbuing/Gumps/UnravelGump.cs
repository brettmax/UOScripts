using System.Collections.Generic;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Imbuing;

/// <summary>
/// Confirmation gump for unraveling a single item.
/// </summary>
public class UnravelGump : DynamicGump
{
    private const int LabelColor = 0x7FFF;

    private readonly PlayerMobile _player;
    private readonly Item _item;

    public override bool Singleton => true;

    public UnravelGump(PlayerMobile pm, Item item) : base(60, 36)
    {
        _player = pm;
        _item = item;
    }

    protected override void BuildLayout(ref DynamicGumpBuilder builder)
    {
        builder.AddPage();
        builder.AddBackground(0, 0, 520, 245, 5054);
        builder.AddImageTiled(10, 10, 500, 225, 2624);
        builder.AddImageTiled(10, 30, 500, 10, 5058);
        builder.AddImageTiled(10, 202, 500, 10, 5058);
        builder.AddAlphaRegion(10, 10, 500, 225);

        // <CENTER>UNRAVEL MAGIC ITEM CONFIRMATION</CENTER>
        builder.AddHtmlLocalized(10, 12, 520, 20, 1112402, LabelColor);

        // WARNING! You have targeted an item made out of special material.<BR><BR>This item will be DESTROYED.<BR><BR>Are you sure you wish to unravel this item?
        builder.AddHtmlLocalized(15, 58, 490, 113, 1112403, true, true);

        // Unravel Item
        builder.AddButton(10, 180, 4005, 4007, 1);
        builder.AddHtmlLocalized(45, 180, 430, 20, 1114292, LabelColor);

        // CANCEL
        builder.AddButton(10, 212, 4017, 4019, 0);
        builder.AddHtmlLocalized(45, 212, 50, 20, 1011012, LabelColor);
    }

    public override void OnResponse(NetState sender, in RelayInfo info)
    {
        _player.EndAction<ImbuingActionLock>();

        if (info.ButtonID == 0 || _item.Deleted)
        {
            return;
        }

        if (Imbuing.CanUnravelItem(_player, _item) && Imbuing.UnravelItem(_player, _item))
        {
            _player.FixedParticles(0x375A, 1, 17, 0, EffectLayer.Waist);
            _player.PlaySound(0x1EB);

            _player.SendLocalizedMessage(1080429); // You magically unravel the item!
            _player.SendLocalizedMessage(1072223); // An item has been placed in your backpack.
        }
    }
}

/// <summary>
/// Confirmation gump for unraveling all items in a container.
/// </summary>
public class UnravelContainerGump : DynamicGump
{
    private const int LabelColor = 0x7FFF;

    private readonly PlayerMobile _player;
    private readonly Container _container;
    private readonly List<Item> _items;

    public override bool Singleton => true;

    public UnravelContainerGump(PlayerMobile pm, Container container) : base(25, 50)
    {
        _player = pm;
        _container = container;
        _items = new List<Item>(container.Items);
    }

    protected override void BuildLayout(ref DynamicGumpBuilder builder)
    {
        builder.AddPage();
        builder.AddBackground(0, 0, 520, 245, 5054);
        builder.AddImageTiled(10, 10, 500, 225, 2624);
        builder.AddImageTiled(10, 30, 500, 10, 5058);
        builder.AddImageTiled(10, 202, 500, 10, 5058);
        builder.AddAlphaRegion(10, 10, 500, 225);

        // <CENTER>UNRAVEL MAGIC ITEM CONFIRMATION</CENTER>
        builder.AddHtmlLocalized(10, 12, 520, 20, 1112402, LabelColor);

        // WARNING! The selected container contains items made with a special material.<BR><BR>These items will be DESTROYED.<BR><BR>Do you wish to unravel these items as well?
        builder.AddHtmlLocalized(15, 58, 490, 113, 1112404, true, true);

        // YES
        builder.AddButton(10, 180, 4005, 4007, 1);
        builder.AddHtmlLocalized(45, 180, 430, 20, 1049717, LabelColor);

        // NO
        builder.AddButton(10, 212, 4017, 4019, 0);
        builder.AddHtmlLocalized(45, 212, 50, 20, 1049718, LabelColor);
    }

    public override void OnResponse(NetState sender, in RelayInfo info)
    {
        _player.EndAction<ImbuingActionLock>();

        if (_container == null || _items == null)
        {
            return;
        }

        if (info.ButtonID == 0)
        {
            TryUnravelContainer(_player, _container);
            return;
        }

        var count = 0;

        foreach (var item in _items)
        {
            if (Imbuing.CanUnravelItem(_player, item, true) && Imbuing.UnravelItem(_player, item, true))
            {
                count++;
            }
        }

        if (count > 0)
        {
            _player.SendLocalizedMessage(1080429); // You magically unravel the item!
            _player.SendLocalizedMessage(1072223); // An item has been placed in your backpack.
        }

        _player.SendLocalizedMessage(1111814, $"{count}\t{_items.Count}"); // Unraveled: ~1_COUNT~/~2_NUM~ items
    }

    private static void TryUnravelContainer(Mobile from, Container c)
    {
        foreach (var item in c.Items)
        {
            Imbuing.CanUnravelItem(from, item, true);
        }

        from.SendLocalizedMessage(1111814, $"0\t{c.Items.Count}"); // Unraveled: ~1_COUNT~/~2_NUM~ items
    }
}

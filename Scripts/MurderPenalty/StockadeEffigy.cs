using ModernUO.Serialization;
using Server.Mobiles;

namespace Server.Engines.MurderPenalty;

[SerializationGenerator(0, false)]
public partial class StockadeEffigy : Mobile
{
    [SerializableField(0)]
    private PlayerMobile _owner;

    [SerializableField(1)]
    private bool _isJailEffigy;

    public StockadeEffigy(
        PlayerMobile owner,
        bool isJailEffigy,
        int inmateShirtId,
        int inmatePantsId,
        int inmateHue)
    {
        _owner = owner;
        _isJailEffigy = isJailEffigy;

        Blessed = true;
        Frozen = true;
        AccessLevel = AccessLevel.Counselor;

        CloneBody(owner);

        if (isJailEffigy)
        {
            EquipItem(new Item(inmateShirtId) { Movable = false, Layer = Layer.InnerTorso, Hue = inmateHue });
            EquipItem(new Item(inmatePantsId) { Movable = false, Layer = Layer.Pants, Hue = inmateHue });
        }
        else
        {
            CloneClothes(owner);
        }

        MoveToWorld(owner.Location, owner.Map);
    }

    public void CloneBody(Mobile from)
    {
        Name = from.Name;
        Body = from.Body;
        Female = from.Female;
        HairItemID = from.HairItemID;
        HairHue = from.HairHue;
        FacialHairItemID = from.FacialHairItemID;
        FacialHairHue = from.FacialHairHue;
    }

    public void CloneClothes(Mobile from)
    {
        for (var i = Items.Count - 1; i >= 0; i--)
        {
            Items[i].Delete();
        }

        for (var i = from.Items.Count - 1; i >= 0; i--)
        {
            var item = from.Items[i];

            if (item.Layer != Layer.Backpack && item.Layer != Layer.Mount && item.Layer != Layer.Bank)
            {
                AddItem(CloneItem(item));
            }
        }
    }

    private static Item CloneItem(Item item) =>
        new(item.ItemID)
        {
            Layer = item.Layer,
            Name = item.Name,
            Hue = item.Hue,
            Weight = item.Weight,
            Movable = false
        };

    public override void OnDoubleClick(Mobile from)
    {
        DisplayPaperdollTo(from);
    }

    public override bool CanBeRenamedBy(Mobile from) => false;

    public override bool CanBeDamaged() => false;

    public override void OnAfterDelete()
    {
        _owner = null;

        for (var i = Items.Count - 1; i >= 0; i--)
        {
            Items[i].Delete();
        }

        base.OnAfterDelete();
    }

    [AfterDeserialization]
    private void AfterDeserialization()
    {
        if (_owner == null || _owner.Deleted)
        {
            Delete();
            return;
        }

        Frozen = true;
    }
}

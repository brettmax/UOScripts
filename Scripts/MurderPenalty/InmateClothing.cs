using ModernUO.Serialization;

namespace Server.Engines.MurderPenalty;

[SerializationGenerator(0, false)]
public partial class InmateShirt : Item
{
    public InmateShirt(int itemId, int hue) : base(itemId)
    {
        Movable = false;
        LootType = LootType.Blessed;
        Layer = Layer.InnerTorso;
        Hue = hue;
    }
}

[SerializationGenerator(0, false)]
public partial class InmatePants : Item
{
    public InmatePants(int itemId, int hue) : base(itemId)
    {
        Movable = false;
        LootType = LootType.Blessed;
        Layer = Layer.Pants;
        Hue = hue;
    }
}

public static class InmateClothing
{
    public static void Dress(Mobile m, int shirtId, int pantsId, int hue)
    {
        Undress(m);

        m.EquipItem(new InmateShirt(shirtId, hue));
        m.EquipItem(new InmatePants(pantsId, hue));
    }

    public static void Undress(Mobile m)
    {
        for (var i = m.Items.Count - 1; i >= 0; i--)
        {
            var item = m.Items[i];

            if (item is InmateShirt or InmatePants)
            {
                item.Delete();
            }
        }
    }
}

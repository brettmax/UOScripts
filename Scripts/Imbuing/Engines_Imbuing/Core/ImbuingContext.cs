using Server.Items;

namespace Server.Engines.Imbuing;

/// <summary>
/// Stores the current imbuing session state for a player
/// </summary>
public class ImbuingContext
{
    public Mobile Player { get; }

    /// <summary>
    /// The last item that was imbued
    /// </summary>
    public Item? LastImbued { get; set; }

    /// <summary>
    /// The ID of the last property that was imbued
    /// </summary>
    public int Imbue_Mod { get; set; }

    /// <summary>
    /// The intensity value of the last property
    /// </summary>
    public int Imbue_ModInt { get; set; }

    /// <summary>
    /// The weight value of the last property
    /// </summary>
    public int Imbue_ModVal { get; set; }

    /// <summary>
    /// Current category selected in the imbuing menu
    /// </summary>
    public int ImbMenu_Cat { get; set; }

    /// <summary>
    /// Current increment value selected in the menu
    /// </summary>
    public int ImbMenu_ModInc { get; set; }

    /// <summary>
    /// Maximum weight allowed for the current item
    /// </summary>
    public int Imbue_IWmax { get; set; }

    public ImbuingContext(Mobile player)
    {
        Player = player;
    }

    /// <summary>
    /// Resets the context to default values
    /// </summary>
    public void Reset()
    {
        LastImbued = null;
        Imbue_Mod = 0;
        Imbue_ModInt = 0;
        Imbue_ModVal = 0;
        ImbMenu_Cat = 0;
        ImbMenu_ModInc = 0;
        Imbue_IWmax = 0;
    }
}

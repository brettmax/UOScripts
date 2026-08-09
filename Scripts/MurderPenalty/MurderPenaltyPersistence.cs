using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Engines.MurderPenalty;

public class MurderPenaltyPersistence : GenericPersistence
{
    private static MurderPenaltyPersistence _instance;

    public static Dictionary<PlayerMobile, DateTime> OfflineDecayRecords { get; } = new();
    public static Dictionary<PlayerMobile, StockadeEffigy> Effigies { get; } = new();

    public static void Configure()
    {
        _instance = new MurderPenaltyPersistence();
    }

    private MurderPenaltyPersistence() : base("MurderPenalty", 10)
    {
    }

    public override void Serialize(IGenericWriter writer)
    {
        writer.WriteEncodedInt(0); // version

        writer.WriteEncodedInt(OfflineDecayRecords.Count);
        foreach (var (player, disconnectTime) in OfflineDecayRecords)
        {
            writer.Write(player);
            writer.Write(disconnectTime);
        }

        writer.WriteEncodedInt(Effigies.Count);
        foreach (var (player, effigy) in Effigies)
        {
            writer.Write(player);
            writer.Write(effigy);
        }
    }

    public override void Deserialize(IGenericReader reader)
    {
        var version = reader.ReadEncodedInt();

        var decayCount = reader.ReadEncodedInt();
        for (var i = 0; i < decayCount; i++)
        {
            var player = reader.ReadEntity<PlayerMobile>();
            var disconnectTime = reader.ReadDateTime();

            if (player != null && !player.Deleted)
            {
                OfflineDecayRecords[player] = disconnectTime;
            }
        }

        var effigyCount = reader.ReadEncodedInt();
        for (var i = 0; i < effigyCount; i++)
        {
            var player = reader.ReadEntity<PlayerMobile>();
            var effigy = reader.ReadEntity<StockadeEffigy>();

            if (player != null && !player.Deleted && effigy != null && !effigy.Deleted)
            {
                Effigies[player] = effigy;
            }
            else
            {
                effigy?.Delete();
            }
        }
    }
}

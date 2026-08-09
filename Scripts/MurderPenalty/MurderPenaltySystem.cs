using System;
using System.Collections.Generic;
using ModernUO.CodeGeneratedEvents;
using Server.Collections;
using Server.Logging;
using Server.Mobiles;
using MurderSystem = Server.Engines.PlayerMurderSystem.PlayerMurderSystem;

namespace Server.Engines.MurderPenalty;

public static class MurderPenaltySystem
{
    private static readonly ILogger logger = LogFactory.GetLogger(typeof(MurderPenaltySystem));

    private static bool _jailOnDeath;
    private static bool _offlineDecay;
    private static bool _jailEffigies;
    private static bool _decayEffigies;
    private static bool _autoResurrect;

    private static int _releaseKillThreshold;
    private static Point3D _releaseLocation;
    private static Map _releaseMap;

    private static int _inmateShirtItemId;
    private static int _inmatePantsItemId;
    private static int _inmateHue;
    private static int _periodicDecayMinutes;

    private static List<MurdererJailRegion> _jailRegions;
    private static readonly HashSet<PlayerMobile> _jailedPlayers = new();
    private static TimerExecutionToken _releaseTimerToken;
    private static TimerExecutionToken _decayTimerToken;

    public static void Configure()
    {
        _jailOnDeath = ServerConfiguration.GetOrUpdateSetting("murderPenalty.jailOnDeath", false);
        _offlineDecay = ServerConfiguration.GetOrUpdateSetting("murderPenalty.offlineDecay", false);
        _jailEffigies = ServerConfiguration.GetOrUpdateSetting("murderPenalty.jailEffigies", false);
        _decayEffigies = ServerConfiguration.GetOrUpdateSetting("murderPenalty.decayEffigies", false);
        _autoResurrect = ServerConfiguration.GetOrUpdateSetting("murderPenalty.autoResurrect", true);

        _releaseKillThreshold = ServerConfiguration.GetOrUpdateSetting("murderPenalty.releaseKillThreshold", 0);

        var releaseX = ServerConfiguration.GetOrUpdateSetting("murderPenalty.releaseX", 1438);
        var releaseY = ServerConfiguration.GetOrUpdateSetting("murderPenalty.releaseY", 1690);
        var releaseZ = ServerConfiguration.GetOrUpdateSetting("murderPenalty.releaseZ", 0);
        _releaseLocation = new Point3D(releaseX, releaseY, releaseZ);

        var releaseMapName = ServerConfiguration.GetOrUpdateSetting("murderPenalty.releaseMap", "Felucca");
        _releaseMap = Map.Parse(releaseMapName) ?? Map.Felucca;

        _inmateShirtItemId = ServerConfiguration.GetOrUpdateSetting("murderPenalty.inmateShirtItemId", 0x1517);
        _inmatePantsItemId = ServerConfiguration.GetOrUpdateSetting("murderPenalty.inmatePantsItemId", 0x152E);
        _inmateHue = ServerConfiguration.GetOrUpdateSetting("murderPenalty.inmateHue", 0);
        _periodicDecayMinutes = ServerConfiguration.GetOrUpdateSetting("murderPenalty.periodicDecayMinutes", 30);
    }

    public static void Initialize()
    {
        if (!_jailOnDeath && !_offlineDecay && !_jailEffigies && !_decayEffigies)
        {
            return;
        }

        _jailRegions = new List<MurdererJailRegion>();

        foreach (var region in Region.Regions)
        {
            if (region is MurdererJailRegion jailRegion)
            {
                _jailRegions.Add(jailRegion);
            }
        }

        if (_jailOnDeath && _jailRegions.Count == 0)
        {
            logger.Warning("MurderPenalty jail-on-death is enabled but no MurdererJailRegion regions are defined.");
        }

        EventSink.Disconnected += OnDisconnected;
        EventSink.Connected += OnConnected;

        if (_jailOnDeath)
        {
            Timer.StartTimer(
                TimeSpan.FromMinutes(5),
                TimeSpan.FromMinutes(5),
                CheckOnlineRelease,
                out _releaseTimerToken
            );
        }

        if (_offlineDecay)
        {
            Timer.StartTimer(
                TimeSpan.FromMinutes(_periodicDecayMinutes),
                TimeSpan.FromMinutes(_periodicDecayMinutes),
                PeriodicOfflineProcessing,
                out _decayTimerToken
            );
        }
    }

    [OnEvent(nameof(PlayerMobile.PlayerDeathEvent))]
    public static void OnPlayerDeath(PlayerMobile player)
    {
        if (!_jailOnDeath || !player.Murderer)
        {
            return;
        }

        if (player.Region.IsPartOf<MurdererJailRegion>())
        {
            return;
        }

        if (!WasPvPDeath(player))
        {
            return;
        }

        if (_jailRegions == null || _jailRegions.Count == 0)
        {
            return;
        }

        var jailRegion = _jailRegions[Utility.Random(_jailRegions.Count)];
        var jailLocation = jailRegion.GoLocation;
        var jailMap = jailRegion.Map;

        player.MoveToWorld(jailLocation, jailMap);
        player.SendMessage("You have been sent to jail for your crimes.");

        if (_autoResurrect && !player.Alive)
        {
            player.Resurrect();
        }

        InmateClothing.Dress(player, _inmateShirtItemId, _inmatePantsItemId, _inmateHue);
        _jailedPlayers.Add(player);
    }

    [OnEvent(nameof(PlayerMobile.PlayerLoginEvent))]
    public static void OnLogin(PlayerMobile player)
    {
        // Delete effigy on login
        DeleteEffigy(player);

        // Settle offline decay for this player (others settled in OnConnected)
        if (_offlineDecay)
        {
            SettleRecord(player);
        }

        // Check jail release
        if (_jailOnDeath && player.Region.IsPartOf<MurdererJailRegion>())
        {
            if (player.Kills <= _releaseKillThreshold)
            {
                ReleasePlayer(player);
            }
            else
            {
                _jailedPlayers.Add(player);
            }
        }
    }

    private static void OnConnected(Mobile m)
    {
        if (m is not PlayerMobile player)
        {
            return;
        }

        if (_offlineDecay)
        {
            SettleOtherAccountCharacters(player);
        }
    }

    private static void OnDisconnected(Mobile m)
    {
        if (m is not PlayerMobile player)
        {
            return;
        }

        _jailedPlayers.Remove(player);

        var inJail = player.Region.IsPartOf<MurdererJailRegion>();
        var inDecay = player.Region.IsPartOf<MurderDecayRegion>();

        // Offline decay recording
        if (_offlineDecay && (inJail || inDecay))
        {
            RecordOfflineDecay(player);
        }

        // Effigy logic — jail takes priority over decay
        if (_jailEffigies && inJail)
        {
            SpawnEffigy(player, isJail: true);
        }
        else if (_decayEffigies && (inDecay || inJail))
        {
            SpawnEffigy(player, isJail: false);
        }
    }

    private static void RecordOfflineDecay(PlayerMobile player)
    {
        if (player.Kills <= 0)
        {
            if (!MurderSystem.GetMurderContext(player, out var context) || context.ShortTermMurders <= 0)
            {
                return;
            }
        }

        if (HasOtherOnlineCharacter(player))
        {
            return;
        }

        var account = player.Account;
        if (account != null)
        {
            for (var i = 0; i < account.Length; i++)
            {
                if (account[i] is PlayerMobile other && other != player)
                {
                    MurderPenaltyPersistence.OfflineDecayRecords.Remove(other);
                }
            }
        }

        MurderPenaltyPersistence.OfflineDecayRecords[player] = Core.Now;
    }

    private static void SpawnEffigy(PlayerMobile player, bool isJail)
    {
        DeleteEffigy(player);

        var effigy = new StockadeEffigy(player, isJail, _inmateShirtItemId, _inmatePantsItemId, _inmateHue);
        MurderPenaltyPersistence.Effigies[player] = effigy;
    }

    private static void DeleteEffigy(PlayerMobile player)
    {
        if (MurderPenaltyPersistence.Effigies.Remove(player, out var effigy))
        {
            effigy.Delete();
        }
    }

    private static void CheckOnlineRelease()
    {
        if (_jailedPlayers.Count == 0)
        {
            return;
        }

        using var toRelease = PooledRefQueue<PlayerMobile>.Create();

        foreach (var player in _jailedPlayers)
        {
            if (player.Kills <= _releaseKillThreshold)
            {
                toRelease.Enqueue(player);
            }
        }

        while (toRelease.Count > 0)
        {
            ReleasePlayer(toRelease.Dequeue());
        }
    }

    private static void PeriodicOfflineProcessing()
    {
        var records = MurderPenaltyPersistence.OfflineDecayRecords;
        var effigies = MurderPenaltyPersistence.Effigies;

        if (records.Count == 0 && effigies.Count == 0)
        {
            return;
        }

        using var toRemove = PooledRefQueue<PlayerMobile>.Create();

        foreach (var (player, disconnectTime) in records)
        {
            if (player == null || player.Deleted)
            {
                toRemove.Enqueue(player);
                continue;
            }

            var elapsed = Core.Now - disconnectTime;
            if (elapsed <= TimeSpan.Zero)
            {
                continue;
            }

            if (!ApplyOfflineDecay(player, elapsed))
            {
                toRemove.Enqueue(player);
                continue;
            }

            // Update timestamp to prevent double-counting
            records[player] = Core.Now;

            if (player.Kills <= _releaseKillThreshold)
            {
                // Release offline player
                player.MoveToWorld(_releaseLocation, _releaseMap);
                InmateClothing.Undress(player);
                toRemove.Enqueue(player);
            }
        }

        while (toRemove.Count > 0)
        {
            var player = toRemove.Dequeue();
            records.Remove(player);
            DeleteEffigy(player);
        }

        // Scan effigies for orphans
        using var orphanEffigies = PooledRefQueue<PlayerMobile>.Create();

        foreach (var (player, effigy) in effigies)
        {
            if (player == null || player.Deleted || effigy == null || effigy.Deleted)
            {
                orphanEffigies.Enqueue(player);
                continue;
            }

            // Player is online (not on Internal map)
            if (player.Map != Map.Internal)
            {
                orphanEffigies.Enqueue(player);
                continue;
            }

            if (player.Kills <= _releaseKillThreshold)
            {
                orphanEffigies.Enqueue(player);
            }
        }

        while (orphanEffigies.Count > 0)
        {
            DeleteEffigy(orphanEffigies.Dequeue());
        }
    }

    public static void ReleasePlayer(PlayerMobile player)
    {
        _jailedPlayers.Remove(player);
        InmateClothing.Undress(player);
        DeleteEffigy(player);
        player.MoveToWorld(_releaseLocation, _releaseMap);
        player.SendMessage("You have served your sentence and are free to go.");
    }

    private static void SettleRecord(PlayerMobile player)
    {
        if (!MurderPenaltyPersistence.OfflineDecayRecords.Remove(player, out var disconnectTime))
        {
            return;
        }

        var elapsed = Core.Now - disconnectTime;
        if (elapsed <= TimeSpan.Zero)
        {
            return;
        }

        ApplyOfflineDecay(player, elapsed);
    }

    private static bool ApplyOfflineDecay(PlayerMobile player, TimeSpan elapsed)
    {
        if (!MurderSystem.GetMurderContext(player, out var context))
        {
            return false;
        }

        context.ShortTermElapse -= elapsed;
        context.LongTermElapse -= elapsed;

        while (context.ShortTermMurders > 0 || player.Kills > 0)
        {
            var prevShort = context.ShortTermMurders;
            var prevKills = player.Kills;
            context.DecayKills();
            if (context.ShortTermMurders == prevShort && player.Kills == prevKills)
            {
                break;
            }
        }

        return true;
    }

    private static void SettleOtherAccountCharacters(Mobile loggedInPlayer)
    {
        var account = loggedInPlayer.Account;
        if (account == null)
        {
            return;
        }

        for (var i = 0; i < account.Length; i++)
        {
            if (account[i] is PlayerMobile other && other != loggedInPlayer)
            {
                SettleRecord(other);
            }
        }
    }

    private static bool HasOtherOnlineCharacter(PlayerMobile player)
    {
        var account = player.Account;
        if (account == null)
        {
            return false;
        }

        for (var i = 0; i < account.Length; i++)
        {
            if (account[i] is Mobile m && m != player && m.Map != Map.Internal)
            {
                return true;
            }
        }

        return false;
    }

    private static bool WasPvPDeath(PlayerMobile player)
    {
        foreach (var entry in player.DamageEntries)
        {
            if (entry.HasExpired)
            {
                continue;
            }

            var damager = entry.Damager;

            if (damager is PlayerMobile pm && pm != player)
            {
                return true;
            }

            if (damager is BaseCreature creature)
            {
                var master = creature.GetMaster();
                if (master is PlayerMobile masterPm && masterPm != player)
                {
                    return true;
                }
            }
        }

        return false;
    }
}

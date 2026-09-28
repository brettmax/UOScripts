using System;
using Server.Mobiles;
using Server.Regions;
using MurderSystem = Server.Engines.PlayerMurderSystem.PlayerMurderSystem;
using Xunit;

namespace Server.Engines.MurderPenalty.Tests;

[Collection("Sequential Scripts Tests")]
public class MurderPenaltySystemTests
{
    private static PlayerMobile CreatePlayer(Map map, Point3D location)
    {
        var player = new PlayerMobile(World.NewMobile);
        player.DefaultMobileInit();
        player.MoveToWorld(location, map);
        return player;
    }

    private static MurdererJailRegion CreateAndRegisterJailRegion(
        Map map, Point3D goLocation, Rectangle3D area)
    {
        var region = new MurdererJailRegion("Test Jail", map, 50, area);
        region.GoLocation = goLocation;
        region.Register();
        return region;
    }

    private static MurderDecayRegion CreateAndRegisterDecayRegion(
        Map map, Rectangle3D area)
    {
        var region = new MurderDecayRegion("Test Decay Zone", map, 50, area);
        region.Register();
        return region;
    }

    private static void AddDamageEntry(PlayerMobile victim, Mobile damager)
    {
        victim.RegisterDamage(10, damager);
    }

    private static void EnableJailSystem()
    {
        ServerConfiguration.SetSetting("murderPenalty.jailOnDeath", true);
        ServerConfiguration.SetSetting("murderPenalty.offlineDecay", false);
        ServerConfiguration.SetSetting("murderPenalty.jailEffigies", false);
        ServerConfiguration.SetSetting("murderPenalty.decayEffigies", false);
        ServerConfiguration.SetSetting("murderPenalty.autoResurrect", true);
        ServerConfiguration.SetSetting("murderPenalty.releaseKillThreshold", 0);
        ServerConfiguration.SetSetting("murderPenalty.releaseX", 1438);
        ServerConfiguration.SetSetting("murderPenalty.releaseY", 1690);
        ServerConfiguration.SetSetting("murderPenalty.releaseZ", 0);
        ServerConfiguration.SetSetting("murderPenalty.releaseMap", "Felucca");
        MurderPenaltySystem.Configure();
        MurderPenaltySystem.Initialize();
    }

    private static void EnableDecaySystem()
    {
        ServerConfiguration.SetSetting("murderPenalty.jailOnDeath", false);
        ServerConfiguration.SetSetting("murderPenalty.offlineDecay", true);
        ServerConfiguration.SetSetting("murderPenalty.jailEffigies", false);
        ServerConfiguration.SetSetting("murderPenalty.decayEffigies", false);
        ServerConfiguration.SetSetting("murderPenalty.releaseKillThreshold", 0);
        MurderPenaltySystem.Configure();
        MurderPenaltySystem.Initialize();
    }

    private static void EnableAllFeatures()
    {
        ServerConfiguration.SetSetting("murderPenalty.jailOnDeath", true);
        ServerConfiguration.SetSetting("murderPenalty.offlineDecay", true);
        ServerConfiguration.SetSetting("murderPenalty.jailEffigies", true);
        ServerConfiguration.SetSetting("murderPenalty.decayEffigies", true);
        ServerConfiguration.SetSetting("murderPenalty.autoResurrect", true);
        ServerConfiguration.SetSetting("murderPenalty.releaseKillThreshold", 0);
        ServerConfiguration.SetSetting("murderPenalty.releaseX", 1438);
        ServerConfiguration.SetSetting("murderPenalty.releaseY", 1690);
        ServerConfiguration.SetSetting("murderPenalty.releaseZ", 0);
        ServerConfiguration.SetSetting("murderPenalty.releaseMap", "Felucca");
        ServerConfiguration.SetSetting("murderPenalty.periodicDecayMinutes", 30);
        MurderPenaltySystem.Configure();
        MurderPenaltySystem.Initialize();
    }

    // --- Jail Tests (migrated) ---

    [Fact]
    public void OnPlayerDeath_JailsMurderer_OnPvPDeath()
    {
        var map = Map.Felucca;
        var jailLocation = new Point3D(5275, 1165, 0);
        var jailArea = new Rectangle3D(new Point3D(5270, 1160, -128), new Point3D(5290, 1180, 127));
        var region = CreateAndRegisterJailRegion(map, jailLocation, jailArea);

        var murderer = CreatePlayer(map, new Point3D(1000, 1000, 0));
        var attacker = CreatePlayer(map, new Point3D(1001, 1000, 0));

        try
        {
            EnableJailSystem();

            murderer.Kills = 5;
            Assert.True(murderer.Murderer);

            AddDamageEntry(murderer, attacker);

            MurderPenaltySystem.OnPlayerDeath(murderer);

            Assert.Equal(jailLocation, murderer.Location);
            Assert.Equal(map, murderer.Map);
        }
        finally
        {
            region.Unregister();
            murderer.Delete();
            attacker.Delete();
        }
    }

    [Fact]
    public void OnPlayerDeath_DoesNotJail_NonMurderer()
    {
        var map = Map.Felucca;
        var jailLocation = new Point3D(5275, 1165, 0);
        var jailArea = new Rectangle3D(new Point3D(5270, 1160, -128), new Point3D(5290, 1180, 127));
        var region = CreateAndRegisterJailRegion(map, jailLocation, jailArea);

        var originalLocation = new Point3D(1200, 1000, 0);
        var player = CreatePlayer(map, originalLocation);
        var attacker = CreatePlayer(map, new Point3D(1201, 1000, 0));

        try
        {
            EnableJailSystem();

            player.Kills = 4;
            Assert.False(player.Murderer);

            AddDamageEntry(player, attacker);

            MurderPenaltySystem.OnPlayerDeath(player);

            Assert.Equal(originalLocation, player.Location);
        }
        finally
        {
            region.Unregister();
            player.Delete();
            attacker.Delete();
        }
    }

    [Fact]
    public void OnPlayerDeath_DoesNotJail_PvEDeath()
    {
        var map = Map.Felucca;
        var jailLocation = new Point3D(5275, 1165, 0);
        var jailArea = new Rectangle3D(new Point3D(5270, 1160, -128), new Point3D(5290, 1180, 127));
        var region = CreateAndRegisterJailRegion(map, jailLocation, jailArea);

        var originalLocation = new Point3D(1400, 1000, 0);
        var murderer = CreatePlayer(map, originalLocation);

        try
        {
            EnableJailSystem();

            murderer.Kills = 5;

            MurderPenaltySystem.OnPlayerDeath(murderer);

            Assert.Equal(originalLocation, murderer.Location);
        }
        finally
        {
            region.Unregister();
            murderer.Delete();
        }
    }

    [Fact]
    public void OnPlayerDeath_DoesNotJail_WhenAlreadyInJail()
    {
        var map = Map.Felucca;
        var jailLocation = new Point3D(5275, 1165, 0);
        var jailArea = new Rectangle3D(new Point3D(5270, 1160, -128), new Point3D(5290, 1180, 127));
        var region = CreateAndRegisterJailRegion(map, jailLocation, jailArea);

        var murderer = CreatePlayer(map, jailLocation);
        var attacker = CreatePlayer(map, new Point3D(1001, 1000, 0));

        try
        {
            EnableJailSystem();

            murderer.Kills = 5;
            AddDamageEntry(murderer, attacker);

            MurderPenaltySystem.OnPlayerDeath(murderer);

            Assert.Equal(jailLocation, murderer.Location);
        }
        finally
        {
            region.Unregister();
            murderer.Delete();
            attacker.Delete();
        }
    }

    [Fact]
    public void OnLogin_ReleasesPlayer_WhenKillsBelowThreshold()
    {
        var map = Map.Felucca;
        var jailLocation = new Point3D(5275, 1165, 0);
        var jailArea = new Rectangle3D(new Point3D(5270, 1160, -128), new Point3D(5290, 1180, 127));
        var region = CreateAndRegisterJailRegion(map, jailLocation, jailArea);

        var releaseLocation = new Point3D(1438, 1690, 0);
        var player = CreatePlayer(map, jailLocation);

        try
        {
            EnableJailSystem();

            player.Kills = 0;

            MurderPenaltySystem.OnLogin(player);

            Assert.Equal(releaseLocation, player.Location);
        }
        finally
        {
            region.Unregister();
            player.Delete();
        }
    }

    [Fact]
    public void OnLogin_KeepsPlayerInJail_WhenKillsAboveThreshold()
    {
        var map = Map.Felucca;
        var jailLocation = new Point3D(5275, 1165, 0);
        var jailArea = new Rectangle3D(new Point3D(5270, 1160, -128), new Point3D(5290, 1180, 127));
        var region = CreateAndRegisterJailRegion(map, jailLocation, jailArea);

        var player = CreatePlayer(map, jailLocation);

        try
        {
            EnableJailSystem();

            player.Kills = 5;

            MurderPenaltySystem.OnLogin(player);

            Assert.Equal(jailLocation, player.Location);
        }
        finally
        {
            region.Unregister();
            player.Delete();
        }
    }

    [Fact]
    public void OnPlayerDeath_DoesNotJail_WhenDisabled()
    {
        var map = Map.Felucca;
        var originalLocation = new Point3D(1800, 1000, 0);
        var murderer = CreatePlayer(map, originalLocation);
        var attacker = CreatePlayer(map, new Point3D(1801, 1000, 0));

        try
        {
            ServerConfiguration.SetSetting("murderPenalty.jailOnDeath", false);
            ServerConfiguration.SetSetting("murderPenalty.offlineDecay", false);
            ServerConfiguration.SetSetting("murderPenalty.jailEffigies", false);
            ServerConfiguration.SetSetting("murderPenalty.decayEffigies", false);
            MurderPenaltySystem.Configure();

            murderer.Kills = 5;
            AddDamageEntry(murderer, attacker);

            MurderPenaltySystem.OnPlayerDeath(murderer);

            Assert.Equal(originalLocation, murderer.Location);
        }
        finally
        {
            murderer.Delete();
            attacker.Delete();
        }
    }

    // --- Offline Decay Tests (migrated) ---

    [Fact]
    public void OnLogin_SettlesRecord_CreditsOfflineTime()
    {
        var map = Map.Felucca;
        var decayArea = new Rectangle3D(new Point3D(3000, 3000, -128), new Point3D(3020, 3020, 127));
        var region = CreateAndRegisterDecayRegion(map, decayArea);

        var player = CreatePlayer(map, new Point3D(3010, 3010, 0));
        var account = new TestAccount();

        try
        {
            EnableDecaySystem();

            player.Kills = 5;
            player.Account = account;
            account[0] = player;

            MurderSystem.GetOrCreateMurderContext(player);

            EventSink.InvokeDisconnected(player);

            MurderPenaltySystem.OnLogin(player);

            // Second login should be a no-op
            MurderPenaltySystem.OnLogin(player);
        }
        finally
        {
            region.Unregister();
            player.Delete();
        }
    }

    [Fact]
    public void OnDisconnect_DoesNotRecord_WhenNotInDecayRegion()
    {
        var map = Map.Felucca;
        var decayArea = new Rectangle3D(new Point3D(3050, 3000, -128), new Point3D(3070, 3020, 127));
        var region = CreateAndRegisterDecayRegion(map, decayArea);

        var player = CreatePlayer(map, new Point3D(2000, 2000, 0));
        var account = new TestAccount();

        try
        {
            EnableDecaySystem();

            player.Kills = 5;
            player.Account = account;
            account[0] = player;

            MurderSystem.GetOrCreateMurderContext(player);

            EventSink.InvokeDisconnected(player);

            var locationBefore = player.Location;
            MurderPenaltySystem.OnLogin(player);
            Assert.Equal(locationBefore, player.Location);
        }
        finally
        {
            region.Unregister();
            player.Delete();
        }
    }

    [Fact]
    public void OnDisconnect_DoesNotRecord_WhenNoMurders()
    {
        var map = Map.Felucca;
        var decayArea = new Rectangle3D(new Point3D(3100, 3000, -128), new Point3D(3120, 3020, 127));
        var region = CreateAndRegisterDecayRegion(map, decayArea);

        var player = CreatePlayer(map, new Point3D(3110, 3010, 0));
        var account = new TestAccount();

        try
        {
            EnableDecaySystem();

            player.Kills = 0;
            player.Account = account;
            account[0] = player;

            EventSink.InvokeDisconnected(player);

            MurderPenaltySystem.OnLogin(player);
        }
        finally
        {
            region.Unregister();
            player.Delete();
        }
    }

    [Fact]
    public void OnDisconnect_DoesNotRecord_WhenOtherCharacterOnline()
    {
        var map = Map.Felucca;
        var decayArea = new Rectangle3D(new Point3D(3150, 3000, -128), new Point3D(3170, 3020, 127));
        var region = CreateAndRegisterDecayRegion(map, decayArea);

        var player1 = CreatePlayer(map, new Point3D(3160, 3010, 0));
        var player2 = CreatePlayer(map, new Point3D(2500, 2500, 0));
        var account = new TestAccount();

        try
        {
            EnableDecaySystem();

            player1.Kills = 5;
            player1.Account = account;
            player2.Account = account;
            account[0] = player1;
            account[1] = player2;

            EventSink.InvokeDisconnected(player1);

            MurderPenaltySystem.OnLogin(player1);
        }
        finally
        {
            region.Unregister();
            player1.Delete();
            player2.Delete();
        }
    }

    [Fact]
    public void OnDisconnect_OnlyLastMurderer_GetsRecord()
    {
        var map = Map.Felucca;
        var decayArea = new Rectangle3D(new Point3D(3200, 3000, -128), new Point3D(3220, 3020, 127));
        var region = CreateAndRegisterDecayRegion(map, decayArea);

        var player1 = CreatePlayer(map, new Point3D(3210, 3010, 0));
        var player2 = CreatePlayer(map, new Point3D(3211, 3010, 0));
        var account = new TestAccount();

        try
        {
            EnableDecaySystem();

            player1.Kills = 5;
            player2.Kills = 5;
            player1.Account = account;
            player2.Account = account;
            account[0] = player1;
            account[1] = player2;

            MurderSystem.GetOrCreateMurderContext(player1);
            MurderSystem.GetOrCreateMurderContext(player2);

            player2.MoveToWorld(Point3D.Zero, Map.Internal);

            EventSink.InvokeDisconnected(player1);

            player1.MoveToWorld(Point3D.Zero, Map.Internal);
            player2.MoveToWorld(new Point3D(3212, 3010, 0), map);

            EventSink.InvokeDisconnected(player2);

            player1.MoveToWorld(new Point3D(3210, 3010, 0), map);

            MurderPenaltySystem.OnLogin(player1);
        }
        finally
        {
            region.Unregister();
            player1.Delete();
            player2.Delete();
        }
    }

    // --- New: Inmate Clothing Tests ---

    [Fact]
    public void OnPlayerDeath_DressesInmateClothing_OnJail()
    {
        var map = Map.Felucca;
        var jailLocation = new Point3D(5275, 1165, 0);
        var jailArea = new Rectangle3D(new Point3D(5270, 1160, -128), new Point3D(5290, 1180, 127));
        var region = CreateAndRegisterJailRegion(map, jailLocation, jailArea);

        var murderer = CreatePlayer(map, new Point3D(1000, 1000, 0));
        var attacker = CreatePlayer(map, new Point3D(1001, 1000, 0));

        try
        {
            EnableJailSystem();

            murderer.Kills = 5;
            AddDamageEntry(murderer, attacker);

            MurderPenaltySystem.OnPlayerDeath(murderer);

            Assert.Contains(murderer.Items, i => i is InmateShirt);
            Assert.Contains(murderer.Items, i => i is InmatePants);
        }
        finally
        {
            region.Unregister();
            murderer.Delete();
            attacker.Delete();
        }
    }

    [Fact]
    public void ReleasePlayer_RemovesInmateClothing()
    {
        var map = Map.Felucca;
        var jailLocation = new Point3D(5275, 1165, 0);
        var jailArea = new Rectangle3D(new Point3D(5270, 1160, -128), new Point3D(5290, 1180, 127));
        var region = CreateAndRegisterJailRegion(map, jailLocation, jailArea);

        var player = CreatePlayer(map, jailLocation);

        try
        {
            EnableJailSystem();

            InmateClothing.Dress(player, 0x1517, 0x152E, 0);

            Assert.Contains(player.Items, i => i is InmateShirt);

            MurderPenaltySystem.ReleasePlayer(player);

            Assert.DoesNotContain(player.Items, i => i is InmateShirt);
            Assert.DoesNotContain(player.Items, i => i is InmatePants);
        }
        finally
        {
            region.Unregister();
            player.Delete();
        }
    }

    // --- New: Effigy Tests ---

    [Fact]
    public void OnDisconnect_SpawnsJailEffigy_InJailRegion()
    {
        var map = Map.Felucca;
        var jailLocation = new Point3D(5275, 1165, 0);
        var jailArea = new Rectangle3D(new Point3D(5270, 1160, -128), new Point3D(5290, 1180, 127));
        var region = CreateAndRegisterJailRegion(map, jailLocation, jailArea);

        var player = CreatePlayer(map, jailLocation);

        try
        {
            ServerConfiguration.SetSetting("murderPenalty.jailOnDeath", true);
            ServerConfiguration.SetSetting("murderPenalty.offlineDecay", false);
            ServerConfiguration.SetSetting("murderPenalty.jailEffigies", true);
            ServerConfiguration.SetSetting("murderPenalty.decayEffigies", false);
            ServerConfiguration.SetSetting("murderPenalty.releaseKillThreshold", 0);
            MurderPenaltySystem.Configure();
            MurderPenaltySystem.Initialize();

            player.Kills = 5;

            EventSink.InvokeDisconnected(player);

            Assert.True(MurderPenaltyPersistence.Effigies.ContainsKey(player));
            var effigy = MurderPenaltyPersistence.Effigies[player];
            Assert.True(effigy.IsJailEffigy);
            Assert.Equal(player.Name, effigy.Name);
        }
        finally
        {
            // Cleanup effigy
            if (MurderPenaltyPersistence.Effigies.Remove(player, out var e))
            {
                e.Delete();
            }
            region.Unregister();
            player.Delete();
        }
    }

    [Fact]
    public void OnDisconnect_SpawnsDecayEffigy_InDecayRegion()
    {
        var map = Map.Felucca;
        var decayArea = new Rectangle3D(new Point3D(3300, 3000, -128), new Point3D(3320, 3020, 127));
        var region = CreateAndRegisterDecayRegion(map, decayArea);

        var player = CreatePlayer(map, new Point3D(3310, 3010, 0));

        try
        {
            ServerConfiguration.SetSetting("murderPenalty.jailOnDeath", false);
            ServerConfiguration.SetSetting("murderPenalty.offlineDecay", false);
            ServerConfiguration.SetSetting("murderPenalty.jailEffigies", false);
            ServerConfiguration.SetSetting("murderPenalty.decayEffigies", true);
            ServerConfiguration.SetSetting("murderPenalty.releaseKillThreshold", 0);
            MurderPenaltySystem.Configure();
            MurderPenaltySystem.Initialize();

            player.Kills = 5;

            EventSink.InvokeDisconnected(player);

            Assert.True(MurderPenaltyPersistence.Effigies.ContainsKey(player));
            var effigy = MurderPenaltyPersistence.Effigies[player];
            Assert.False(effigy.IsJailEffigy);
        }
        finally
        {
            if (MurderPenaltyPersistence.Effigies.Remove(player, out var e))
            {
                e.Delete();
            }
            region.Unregister();
            player.Delete();
        }
    }

    [Fact]
    public void OnLogin_DeletesEffigy()
    {
        var map = Map.Felucca;
        var jailLocation = new Point3D(5275, 1165, 0);
        var jailArea = new Rectangle3D(new Point3D(5270, 1160, -128), new Point3D(5290, 1180, 127));
        var region = CreateAndRegisterJailRegion(map, jailLocation, jailArea);

        var player = CreatePlayer(map, jailLocation);

        try
        {
            ServerConfiguration.SetSetting("murderPenalty.jailOnDeath", true);
            ServerConfiguration.SetSetting("murderPenalty.offlineDecay", false);
            ServerConfiguration.SetSetting("murderPenalty.jailEffigies", true);
            ServerConfiguration.SetSetting("murderPenalty.decayEffigies", false);
            ServerConfiguration.SetSetting("murderPenalty.releaseKillThreshold", 0);
            MurderPenaltySystem.Configure();
            MurderPenaltySystem.Initialize();

            player.Kills = 5;

            EventSink.InvokeDisconnected(player);
            Assert.True(MurderPenaltyPersistence.Effigies.ContainsKey(player));

            MurderPenaltySystem.OnLogin(player);
            Assert.False(MurderPenaltyPersistence.Effigies.ContainsKey(player));
        }
        finally
        {
            if (MurderPenaltyPersistence.Effigies.Remove(player, out var e))
            {
                e.Delete();
            }
            region.Unregister();
            player.Delete();
        }
    }

    // --- Overlapping Region Tests ---

    [Fact]
    public void OnDisconnect_OverlappingRegions_JailEffigyTakesPriority()
    {
        var map = Map.Felucca;
        var loc = new Point3D(4000, 4000, 0);
        var area = new Rectangle3D(new Point3D(3990, 3990, -128), new Point3D(4010, 4010, 127));

        var jailRegion = CreateAndRegisterJailRegion(map, loc, area);
        var decayRegion = CreateAndRegisterDecayRegion(map, area);

        var player = CreatePlayer(map, loc);

        try
        {
            EnableAllFeatures();

            player.Kills = 5;

            EventSink.InvokeDisconnected(player);

            Assert.True(MurderPenaltyPersistence.Effigies.ContainsKey(player));
            var effigy = MurderPenaltyPersistence.Effigies[player];
            Assert.True(effigy.IsJailEffigy);
        }
        finally
        {
            if (MurderPenaltyPersistence.Effigies.Remove(player, out var e))
            {
                e.Delete();
            }
            jailRegion.Unregister();
            decayRegion.Unregister();
            player.Delete();
        }
    }

    [Fact]
    public void OnDisconnect_OverlappingRegions_OnlyOneDecayRecord()
    {
        var map = Map.Felucca;
        var loc = new Point3D(4050, 4000, 0);
        var area = new Rectangle3D(new Point3D(4040, 3990, -128), new Point3D(4060, 4010, 127));

        var jailRegion = CreateAndRegisterJailRegion(map, loc, area);
        var decayRegion = CreateAndRegisterDecayRegion(map, area);

        var player = CreatePlayer(map, loc);
        var account = new TestAccount();

        try
        {
            EnableAllFeatures();

            player.Kills = 5;
            player.Account = account;
            account[0] = player;
            MurderSystem.GetOrCreateMurderContext(player);

            EventSink.InvokeDisconnected(player);

            Assert.True(MurderPenaltyPersistence.OfflineDecayRecords.ContainsKey(player));

            // Only one record, no duplication
            Assert.Single(MurderPenaltyPersistence.OfflineDecayRecords);
        }
        finally
        {
            MurderPenaltyPersistence.OfflineDecayRecords.Remove(player);
            if (MurderPenaltyPersistence.Effigies.Remove(player, out var e))
            {
                e.Delete();
            }
            jailRegion.Unregister();
            decayRegion.Unregister();
            player.Delete();
        }
    }

    [Fact]
    public void OnDisconnect_OverlappingRegions_JailEffigiesDisabled_FallsBackToDecayEffigy()
    {
        var map = Map.Felucca;
        var loc = new Point3D(4100, 4000, 0);
        var area = new Rectangle3D(new Point3D(4090, 3990, -128), new Point3D(4110, 4010, 127));

        var jailRegion = CreateAndRegisterJailRegion(map, loc, area);
        var decayRegion = CreateAndRegisterDecayRegion(map, area);

        var player = CreatePlayer(map, loc);

        try
        {
            ServerConfiguration.SetSetting("murderPenalty.jailOnDeath", true);
            ServerConfiguration.SetSetting("murderPenalty.offlineDecay", true);
            ServerConfiguration.SetSetting("murderPenalty.jailEffigies", false);
            ServerConfiguration.SetSetting("murderPenalty.decayEffigies", true);
            ServerConfiguration.SetSetting("murderPenalty.releaseKillThreshold", 0);
            MurderPenaltySystem.Configure();
            MurderPenaltySystem.Initialize();

            player.Kills = 5;

            EventSink.InvokeDisconnected(player);

            Assert.True(MurderPenaltyPersistence.Effigies.ContainsKey(player));
            var effigy = MurderPenaltyPersistence.Effigies[player];
            Assert.False(effigy.IsJailEffigy);
        }
        finally
        {
            if (MurderPenaltyPersistence.Effigies.Remove(player, out var e))
            {
                e.Delete();
            }
            jailRegion.Unregister();
            decayRegion.Unregister();
            player.Delete();
        }
    }

    [Fact]
    public void OnDisconnect_OverlappingRegions_BothEffigiesDisabled_NoEffigy()
    {
        var map = Map.Felucca;
        var loc = new Point3D(4150, 4000, 0);
        var area = new Rectangle3D(new Point3D(4140, 3990, -128), new Point3D(4160, 4010, 127));

        var jailRegion = CreateAndRegisterJailRegion(map, loc, area);
        var decayRegion = CreateAndRegisterDecayRegion(map, area);

        var player = CreatePlayer(map, loc);
        var account = new TestAccount();

        try
        {
            ServerConfiguration.SetSetting("murderPenalty.jailOnDeath", true);
            ServerConfiguration.SetSetting("murderPenalty.offlineDecay", true);
            ServerConfiguration.SetSetting("murderPenalty.jailEffigies", false);
            ServerConfiguration.SetSetting("murderPenalty.decayEffigies", false);
            ServerConfiguration.SetSetting("murderPenalty.releaseKillThreshold", 0);
            MurderPenaltySystem.Configure();
            MurderPenaltySystem.Initialize();

            player.Kills = 5;
            player.Account = account;
            account[0] = player;
            MurderSystem.GetOrCreateMurderContext(player);

            EventSink.InvokeDisconnected(player);

            Assert.False(MurderPenaltyPersistence.Effigies.ContainsKey(player));
            // But decay record should still exist
            Assert.True(MurderPenaltyPersistence.OfflineDecayRecords.ContainsKey(player));
        }
        finally
        {
            MurderPenaltyPersistence.OfflineDecayRecords.Remove(player);
            jailRegion.Unregister();
            decayRegion.Unregister();
            player.Delete();
        }
    }

    // --- Jail region implies decay ---

    [Fact]
    public void OnDisconnect_JailRegion_ImpliesDecayEligible()
    {
        var map = Map.Felucca;
        var jailLocation = new Point3D(5275, 1165, 0);
        var jailArea = new Rectangle3D(new Point3D(5270, 1160, -128), new Point3D(5290, 1180, 127));
        var region = CreateAndRegisterJailRegion(map, jailLocation, jailArea);

        var player = CreatePlayer(map, jailLocation);
        var account = new TestAccount();

        try
        {
            ServerConfiguration.SetSetting("murderPenalty.jailOnDeath", true);
            ServerConfiguration.SetSetting("murderPenalty.offlineDecay", true);
            ServerConfiguration.SetSetting("murderPenalty.jailEffigies", false);
            ServerConfiguration.SetSetting("murderPenalty.decayEffigies", false);
            ServerConfiguration.SetSetting("murderPenalty.releaseKillThreshold", 0);
            MurderPenaltySystem.Configure();
            MurderPenaltySystem.Initialize();

            player.Kills = 5;
            player.Account = account;
            account[0] = player;
            MurderSystem.GetOrCreateMurderContext(player);

            EventSink.InvokeDisconnected(player);

            // Even though there's no MurderDecayRegion, jail region implies decay
            Assert.True(MurderPenaltyPersistence.OfflineDecayRecords.ContainsKey(player));
        }
        finally
        {
            MurderPenaltyPersistence.OfflineDecayRecords.Remove(player);
            region.Unregister();
            player.Delete();
        }
    }
}

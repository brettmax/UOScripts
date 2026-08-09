using System;
using System.Reflection;
using Server.Items;
using Server.Tests;
using Server.Tests.Maps;
using Xunit;

namespace Server.Engines.MurderPenalty.Tests;

[CollectionDefinition("Sequential Scripts Tests", DisableParallelization = true)]
public class ScriptsFixture : ICollectionFixture<ScriptsFixture>, IDisposable
{
    public ScriptsFixture()
    {
        Core.ApplicationAssembly = Assembly.GetExecutingAssembly();
        Core.LoopContext = new EventLoopContext();
        Core.Expansion = Expansion.EJ;

        ServerConfiguration.Load(true);

        ServerConfiguration.AssemblyDirectories.Add(Core.BaseDirectory);
        AssemblyHandler.LoadAssemblies(["Server.dll", "UOContent.dll"]);

        Server.Network.NetState.Configure();
        TestMapDefinitions.ConfigureTestMapDefinitions();
        World.Configure();
        DecayScheduler.Configure();
        Timer.Init(0);
        World.Load();
        World.ExitSerializationThreads();
    }

    private static int _counter;

    public void Dispose()
    {
        _counter++;
        if (_counter > 1)
        {
            throw new Exception("NO!");
        }
        Timer.Init(0);
    }
}

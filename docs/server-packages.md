# AxmolUO server package set

This repository is the server half of AxmolUO: an Ultima Online: The Second Age
shard that pairs with the Axmol-based client. It composes the server from
packages rather than forking the engine.

## What goes into the server

| Package | Where | Notes |
|---|---|---|
| ModernUO core (`Server`, `UOContent`, `Application`, `Logger`) | `ModernUO/` submodule | Upstream ModernUO, unmodified. Engine changes go upstream. |
| ModernSpawner | `Modules/ModernSpawner/` submodule | Built against this repo's `ModernUO/` through `ModernSpawner.Host.props`. |
| Drop-in scripts | `Scripts/` | `MurderPenalty`, `Staff Hide Crystals`, `HeadlessOwner`. `Template` is the starting point for new ones. |
| Serialization generator | NuGet `ModernUO.Serialization.Generator` | Same version ModernUO pins, so generated code matches the engine. |
| Socket I/O | NuGet `IORingGroup` | Consumed by ModernUO's `Server` project. |

`Distribution/` holds the package overlay applied on top of ModernUO's own
`Distribution/` when staging:

- `Configuration/expansion.json` locks the shard to **The Second Age**
  (expansion id 1) with **Felucca only**, since Trammel arrived with UO:
  Renaissance. It is written on first stage only, so a shard can change it later.
- `Data/assemblies.json` loads `UOContent.dll`, `ModernSpawner.dll` and the
  script assemblies.

## Build, test, stage

```sh
git submodule update --init
dotnet build AxmolUO.Server.slnx
dotnet test AxmolUO.Server.slnx          # ModernSpawner + script tests
tools/stage-server.sh Release Staging    # runnable server in Staging/
dotnet Staging/ModernUO.dll
```

On Windows without Git Bash or WSL, `tools\stage-server.ps1 -Configuration Release`
does the same from PowerShell 5.1 or 7 (run it with `-ExecutionPolicy Bypass` if
local scripts are blocked).

For a headless first boot (containers, services), set `UO_DATA_DIR` to the UO
data folder when staging; the script then writes `Configuration/modernuo.json`,
so the server starts without console prompts. It is written on first stage only;
delete it to pick up new values.

| Variable | Default | Effect |
|---|---|---|
| `LISTEN` | `127.0.0.1:2593` | Local only, no public-IP lookup. `0.0.0.0:2593` accepts LAN and internet clients. |
| `SERVER_NAME` | `AxmolUO` | Name in the shard list. |
| `UO_CLIENT_VERSION` | `1.25.35` | Client version the data files come from. Old `client.exe` files carry no version, and an unknown version is treated as post-High Seas, which misreads T2A houses and boats. It is also the minimum client version the server accepts. |

The first boot of a headless server with no accounts needs the owner account
from `OWNER_USERNAME` and `OWNER_PASSWORD`, read at run time by the
`HeadlessOwner` script and never written to configuration:

```powershell
$env:UO_DATA_DIR='C:\UO'; powershell -ExecutionPolicy Bypass -File tools\stage-server.ps1; $env:OWNER_USERNAME='admin'; $env:OWNER_PASSWORD='change-me'; dotnet Staging\ModernUO.dll
```

Warnings such as `map2.mul was not found` are expected with Felucca-only data:
ModernUO registers every map, and UOContent code refers to all of them.

On first boot the server asks for the Ultima Online data directory (maps,
statics, tiledata, multis). It needs the original client data files; see below.

## Not in the T2A package

These scripts stay in the repository but are not built or loaded for T2A:

- `Scripts/Imbuing`: a Stygian Abyss skill, installed by copying files into
  and patching UOContent rather than as a drop-in assembly.
- `Scripts/HorseBarding`: overwrites UOContent files (`AOS.cs`, `CraftItem.cs`,
  `DefBlacksmithy.cs`) and depends on AOS item properties.

## Contract with the Axmol client

The server is .NET and does not use Axmol, which is a client engine. What the
two halves share is:

- **Wire protocol.** The server speaks the standard UO protocol that ModernUO
  implements (`ModernUO/dev-docs/networking-packets.md`). The T2A preset sends
  T2A feature and character-list flags, so the client must accept a pre-AOS
  feature set (no AOS tooltips, no object property lists).
- **Game data.** The server reads the original UO data files for maps, statics,
  tiledata and multis. Converted Axmol-side assets must not replace the files
  the server reads; the server data directory keeps the originals.
- **Client version.** `RequiredClient` in the expansion preset is unset, so any
  client version the protocol layer supports can log in. Set it once the Axmol
  client reports a fixed version.

# MurderPenalty

A configurable murder penalty system for ModernUO with four independent features that can be mixed and matched.

## Why Use This

### Offline Count Decay

Players can burn murder counts while logged off in a designated region, saving electricity and real-world time. The mechanic still requires dedication: the account is locked to the decaying character (you can't play an alt while counts tick down). This gives murderers a path to redemption without requiring them to keep a PC online for days doing nothing.

### Jail on Death

When a murderer dies in PvP, they are forcibly teleported to jail. There is no option to accept stat loss, buy a resurrection, or otherwise shortcut the penalty. The character is simply removed from play until their counts decay below the release threshold.

This is superficially a harsher penalty than the default system, but it can be tuned with faster count-decay times to suit your shard's balance. It also:

- **Guarantees victims a reprieve.** The PK who killed you is gone for a while, not back in 24 hours after accepting statloss / buying a pardon.
- **Incentivizes fighting back.** If killing the PK sends them to jail, victims have a reason to try instead of always fleeing.
- **Applies equally to everyone.** A new player with 5 kills gets the same treatment as a veteran with a bank full of gold. Wealth and connections don't buy a way out.

### Stockade Effigies

When a jailed or decaying murderer logs off, a cosmetic clone of their character is spawned at their location and remains visible to other players. This serves as a public display for as long as it takes their counts to decay. Whether the community sees it as a pillory of shame or a trophy wall of infamy is up to your shard's culture.

Jail effigies wear inmate clothing. Decay effigies wear the player's real gear.

## Installation

### Build

MurderPenalty is a Script assembly. Build it with:

```sh
dotnet build Scripts/MurderPenalty/MurderPenalty.csproj
```

This outputs `MurderPenalty.dll` to `Scripts/Distribution/Assembiles/`.

### Deploy

Copy `MurderPenalty.dll` into your server's `Distribution/Assemblies/` directory (the same directory that contains `UOContent.dll`).

### Register the Assembly

Add `MurderPenalty.dll` to `Distribution/Data/assemblies.json` so the server loads it at startup:

```json
[
  "UOContent.dll",
  "MurderPenalty.dll"
]
```

## Configuration

All settings live in `Distribution/Configuration/modernuo.json` under the `settings` key. The system reads them on startup via `ServerConfiguration.GetOrUpdateSetting`, so any missing keys are automatically created with their defaults on first run.

### Settings Reference

| Key | Type | Default | Description |
|---|---|---|---|
| `murderPenalty.jailOnDeath` | bool | `false` | Teleport murderers to jail on PvP death |
| `murderPenalty.offlineDecay` | bool | `false` | Apply kill decay while logged out in a decay-eligible region |
| `murderPenalty.jailEffigies` | bool | `false` | Spawn inmate-garb effigy on logout in jail |
| `murderPenalty.decayEffigies` | bool | `false` | Spawn real-gear effigy on logout in decay region |
| `murderPenalty.autoResurrect` | bool | `true` | Auto-resurrect players on jail arrival (see note below) |
| `murderPenalty.releaseKillThreshold` | int | `0` | Kill count at or below which jailed players are released |
| `murderPenalty.releaseX` | int | `1438` | Release location X coordinate |
| `murderPenalty.releaseY` | int | `1690` | Release location Y coordinate |
| `murderPenalty.releaseZ` | int | `0` | Release location Z coordinate |
| `murderPenalty.releaseMap` | string | `"Felucca"` | Release location map |
| `murderPenalty.inmateShirtItemId` | int | `0x1517` | Item ID for inmate shirt (default: sleeveless Shirt) |
| `murderPenalty.inmatePantsItemId` | int | `0x152E` | Item ID for inmate pants (default: ShortPants) |
| `murderPenalty.inmateHue` | int | `0` | Hue applied to inmate clothing (0 = natural/unhued) |
| `murderPenalty.periodicDecayMinutes` | int | `30` | Interval in minutes for the offline decay timer |

**Auto-resurrect does not trigger stat loss.** The pre-AOS stat loss penalty is applied inside the `ResurrectGump` response handler, not by `Mobile.Resurrect()` itself. The forced auto-resurrect on jail arrival calls `Resurrect()` directly, bypassing the gump entirely, so no stat loss is applied. This is intentional — jail time *is* the penalty.

### Example modernuo.json

```json
{
  "assemblyDirectories": ["./Assemblies"],
  "dataDirectories": ["/path/to/uo/client"],
  "listeners": [
    { "Address": "0.0.0.0", "Port": 2593 }
  ],
  "settings": {
    "murderPenalty.jailOnDeath": "True",
    "murderPenalty.offlineDecay": "True",
    "murderPenalty.jailEffigies": "True",
    "murderPenalty.decayEffigies": "True",
    "murderPenalty.autoResurrect": "True",
    "murderPenalty.releaseKillThreshold": "0",
    "murderPenalty.releaseX": "1438",
    "murderPenalty.releaseY": "1690",
    "murderPenalty.releaseZ": "0",
    "murderPenalty.releaseMap": "Felucca",
    "murderPenalty.inmateShirtItemId": "5399",
    "murderPenalty.inmatePantsItemId": "5422",
    "murderPenalty.inmateHue": "0",
    "murderPenalty.periodicDecayMinutes": "30"
  }
}
```

## Region Setup

The system uses two region types. Define them in `Distribution/Data/regions.json`.

### MurdererJailRegion

Extends `JailRegion` — blocks spells, skills, combat, and travel. Used as the jail destination. The `GoLocation` is where players are teleported when jailed.

```json
{
  "$type": "MurdererJailRegion",
  "Map": "Felucca",
  "Name": "Murderer Jail",
  "Priority": 50,
  "Area": [
    {"x1": 5270, "y1": 1160, "z1": -128, "x2": 5290, "y2": 1180, "z2": 127}
  ],
  "GoLocation": {"x": 5275, "y": 1165, "z": 0}
}
```

If multiple jail regions are defined, the system picks one at random when jailing a player.

### MurderDecayRegion

Extends `BaseRegion` — a pure marker with no restrictions. Players who log out inside it get offline decay.

```json
{
  "$type": "MurderDecayRegion",
  "Map": "Felucca",
  "Name": "Murder Decay Zone",
  "Priority": 50,
  "Area": [
    {"x1": 5270, "y1": 1160, "z1": -128, "x2": 5290, "y2": 1180, "z2": 127}
  ]
}
```

### Overlap Behavior

- **Jail regions are implicit decay regions.** If `offlineDecay` is enabled, logging out in a `MurdererJailRegion` creates a decay record even without a `MurderDecayRegion` present.
- **Only one effigy per player.** When both region types overlap spatially, jail effigies take priority. If jail effigies are disabled but decay effigies are enabled, the decay effigy is used instead.
- **Only one decay record per player.** Overlapping regions do not create duplicate records.

You can define them on the same area for combined jail + decay, or separately (e.g., a jail in one location and a decay-only campfire area elsewhere).

## Inmate Clothing

Jailed players are dressed in two non-removable, blessed items: a shirt and pants. These are cosmetic markers, not real clothing — players cannot unequip or loot them.

The default visuals are:
- **Shirt**: Item ID `0x1517` (sleeveless Shirt), layer InnerTorso
- **Pants**: Item ID `0x152E` (ShortPants), layer Pants

To customize the look, change `inmateShirtItemId` and `inmatePantsItemId` to any valid art tile ID. The `inmateHue` setting applies to both pieces. Set it to `0` for the item's natural color, or use a hue value for a uniform look (e.g., `0x497` for bright orange).

Inmate clothing is automatically removed when the player is released.

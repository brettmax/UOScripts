# Imbuing System for ModernUO

Complete implementation of the Imbuing skill system for ModernUO.

## Installation

### 1. Copy Imbuing Engine Files

Copy `Engines_Imbuing/` contents to:
```
Projects/UOContent/Engines/Imbuing/
```

### 2. Copy Imbuing Resource Files

Copy `Items_Resources_Imbuing/` contents to:
```
Projects/UOContent/Items/Resources/Imbuing/
```

### 3. Copy SoulForge

Copy `SoulForge.cs` to:
```
Projects/UOContent/Items/Addons/SoulForge.cs
```

### 4. Apply Required Patches

#### 4.1 AOS.cs - Add BalancedWeapon Attribute

In `Projects/UOContent/Misc/AOS.cs`, add `BalancedWeapon` to the `AosAttribute` enum (around line 304):

```csharp
IncreasedKarmaLoss = 0x00800000,
BalancedWeapon = 0x01000000    // Add this line
```

#### 4.2 BaseWeapon.cs - Add Balanced Tooltip

In `Projects/UOContent/Items/Weapons/BaseWeapon.cs`, find the `GetProperties` method and add the Balanced tooltip display (around line 1073, before the Velocity check):

```csharp
// Balanced property - works on ALL two-handed weapons (melee and ranged)
if (Core.ML && Attributes[AosAttribute.BalancedWeapon] > 0 && Layer == Layer.TwoHanded)
{
    list.Add(1072792); // Balanced
}
```

## Features

- **Imbuing Properties**: 61+ imbuable properties for weapons, armor, and jewelry
- **Balanced Weapon (ID 61)**: Works on ALL two-handed weapons (melee AND ranged), not just bows
- **SoulForge Variants**: Large, Small (Mini), and Royal Soul Forges
- **Unravel System**: Extract magical ingredients from items
- **Full Ingredient System**: 31 imbuing ingredients including essences

## Soul Forge Variants

| Type | Size | Description |
|------|------|-------------|
| SoulForge | 4x4 | Standard large soul forge |
| SmallSoulForge | 1x1 | Mini soul forge (17607) |
| RoyalSoulForge | 4x4 | Royal variant |

## GM Commands

- `[ImbuingKit` - Adds 999 of each imbuing ingredient for testing

## Weight System

| Item Type | Max Weight |
|-----------|------------|
| Melee Weapon | 450 |
| Ranged Weapon | 500 (+50) |
| Two-Handed Weapon | 550 (+100) |
| Exceptional | +50 |
| Jewelry | 500 (fixed) |

## Credits

Based on ServUO implementation, adapted for ModernUO patterns:
- SerializationGenerator for serialization
- DynamicGump for UI
- Modern C# patterns

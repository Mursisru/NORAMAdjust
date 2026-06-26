# NORAM Adjust

[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![NOLoader](https://img.shields.io/badge/loader-NOLoader-blue)](https://github.com/Mursisru/NOLoader)
[![Nuclear Option](https://img.shields.io/badge/game-Nuclear%20Option-darkgreen)](https://store.steampowered.com/app/2168680/Nuclear_Option/)
[![Version](https://img.shields.io/badge/version-1.0.0%20Build%20DEV1P1-informational)](CHANGELOG.md)

A lightweight [NOLoader](https://github.com/Mursisru/NOLoader) mod for **Nuclear Option** that pre-allocates a **5300 MB** managed RAM reservoir at the main menu. The goal is to reduce GC stutter by keeping a stable managed memory footprint during gameplay.

Works for **solo**, **host**, and **dedicated client** sessions. No IL patches — pure `INOMod` reservoir only.

---

## Requirements

- [Nuclear Option](https://store.steampowered.com/app/2168680/Nuclear_Option/) (Steam)
- [NOLoader](https://github.com/Mursisru/NOLoader) installed (`DEV_SDK` or `RDYTU`)
- ~5.3 GB free RAM above the game's baseline usage

---

## Installation

1. Deploy NOLoader first (from `NOLoader_Engine`):

   ```powershell
   .\scripts\deploy-noloader.ps1 -Configuration DEV_SDK
   ```

2. Build and deploy this mod from the repo root:

   ```powershell
   .\scripts\deploy-noram-adjust-mod.ps1
   ```

3. Launch the game. At the main menu, check `NOLoader/logs/noloader_ring.log` or the Unity console for:

   ```
   [NORAMAdjust] reserved=5300MB (1.0.0 Build DEV1P1)
   ```

### Deploy layout

```
Nuclear Option/NOLoader/mods/NORAMAdjust/
  mod.json
  mod.ini
  NOLoader.NORAMAdjust.dll
  NOLoader.ModConfig.dll
```

---

## Configuration

Edit `NOLoader/mods/NORAMAdjust/mod.ini`:

| Key | Default | Description |
|-----|---------|-------------|
| `enabled` | `1` | `1` = allocate reservoir on load, `0` = mod loads but skips allocation |
| `memory_reservoir_mb` | `5300` | Target managed reservoir size in megabytes (decimal GB: 5.3 GB = 5300 MB) |
| `chunk_mb` | `64` | Allocation chunk size in megabytes |

Example — disable without removing the mod:

```ini
[NORAM]
enabled=0
```

---

## Compatibility

| Mod | Note |
|-----|------|
| **MpClientOpt** (`memory_budget=1`) | **Do not run both.** MpClientOpt reserves ~5120 MB by default; running both can reserve ~10+ GB. Disable `memory_budget` in MpClientOpt or remove one mod. |
| Other NOLoader mods | No patch conflicts (this mod has zero IL patches). |

---

## How it works

On `MainMenu` load stage, `NoramAdjustMod.OnLoad` reads `mod.ini` and allocates `byte[]` chunks (default 64 MB each) until the configured target is reached. Chunk endpoints are touched so pages are committed. References are held until `OnUnload` (game exit or F11 hot-reload in DEV_SDK).

---

## Build from source

```powershell
dotnet build NOLoader.NORAMAdjust.csproj -c DEV_SDK
```

Sibling dependency: `../NOLoader_Engine` (for `NOLoader.API` and `NOLoader.ModConfig`).

RDYTU release pack:

```powershell
dotnet build NOLoader.NORAMAdjust.csproj -c RDYTU
.\scripts\deploy-noram-adjust-mod.ps1 -Configuration RDYTU
```

---

## License

[MIT](LICENSE). Nuclear Option is property of its respective owners.

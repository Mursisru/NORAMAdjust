**Developer:** Mursisru

# NORAM Adjust

[![Nuclear Option](https://img.shields.io/badge/Game-Nuclear%20Option-blue)](https://store.steampowered.com/app/2168680/Nuclear_Option/)
[![NOLoader](https://img.shields.io/badge/Loader-NOLoader-purple)](https://github.com/Mursisru/NOLoader)
[![BepInEx 5](https://img.shields.io/badge/Loader-BepInEx%205-orange)](https://docs.bepinex.dev/)
[![Version](https://img.shields.io/badge/Version-1.1.0-green)](https://github.com/Mursisru/NORAMAdjust/releases/tag/v1.1.0)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow)](https://github.com/Mursisru/NORAMAdjust/blob/main/LICENSE)
[![.NET Framework 4.8](https://img.shields.io/badge/.NET%20Framework-4.8-512BD4)](https://dotnet.microsoft.com/download/dotnet-framework/net48)

Pre-allocates a **5300 MB** managed RAM reservoir for **[Nuclear Option](https://store.steampowered.com/app/2168680/Nuclear_Option/)** to reduce GC stutter during gameplay.

Shipped as a **[NOLoader](https://github.com/Mursisru/NOLoader)** mod (recommended) or a **BepInEx 5** plugin. Both builds share the same core (`RamReservoir`, `NoramConfig`) via linked source files in `NORAMAdjust.sln`.

Works for **solo**, **host**, and **dedicated client** — no session gate, no IL patches.

---

## Table of contents

- [What it does](#what-it-does)
- [Requirements](#requirements)
- [Install](#install)
- [Configuration](#configuration)
- [Compatibility](#compatibility)
- [Repository layout](#repository-layout)
- [Build from source](#build-from-source)
- [Verification](#verification)
- [License](#license)

---

## What it does

At load time the mod reads `mod.ini` and allocates `byte[]` chunks (default **64 MB** each) until **5300 MB** total is reserved. The first and last byte of each chunk are touched so memory pages are committed. References are held until unload (game exit, or F11 hot-reload in NOLoader DEV_SDK).

| Loader | When allocation runs |
|--------|----------------------|
| **NOLoader** | `MainMenu` load stage — `NoramAdjustMod.OnLoad` |
| **BepInEx** | Plugin `Awake` |

Expected log line:

```text
[NORAMAdjust] reserved=5300MB (1.1.0 Build DEV1P3)
```

---

## Requirements

| Item | NOLoader path | BepInEx path |
|------|---------------|--------------|
| Game | [Nuclear Option](https://store.steampowered.com/app/2168680/Nuclear_Option/) (Steam) | Same |
| Loader | [NOLoader](https://github.com/Mursisru/NOLoader) `DEV_SDK` or `RDYTU` | [BepInEx 5](https://docs.bepinex.dev/) for Unity Mono |
| Free RAM | ~5.3 GB above game baseline | Same |
| Build dep | Sibling repo `../NOLoader_Engine` | Same + `scripts/fetch-bepinex-libs.ps1` once |

> **Warning:** Do **not** install NOLoader and BepInEx in the same game folder — they conflict on `winhttp.dll`. Pick one loader.

---

## Install

> [!IMPORTANT]
> **Pick one loader** — install either [NOLoader](https://github.com/Mursisru/NOLoader/releases) **or** [BepInEx 5](https://docs.bepinex.dev/) before this mod. Do not run both in the same game folder.

> [!WARNING]
> **~5.3 GB free RAM required** — the mod pre-allocates a managed memory reservoir. Insufficient RAM may cause allocation failure or system paging.

### Option A — release zip (recommended)

1. Open [Release v1.1.0](https://github.com/Mursisru/NORAMAdjust/releases/tag/v1.1.0).
2. Download the zip for your loader:
   - **NOLoader:** `NORAMAdjust-v1.1.0-NOLoader.zip`
   - **BepInEx:** `NORAMAdjust-v1.1.0-BepInEx.zip`
3. Extract into the matching folder (see layouts below).
4. Launch the game and confirm the log line above.

**NOLoader layout:**

```text
Nuclear Option/NOLoader/mods/NORAMAdjust/
  mod.json
  mod.ini
  NOLoader.NORAMAdjust.dll
  NOLoader.ModConfig.dll
  rdytu/                  # RDYTU hash-only manifest (included in release zip)
```

**BepInEx layout:**

```text
Nuclear Option/BepInEx/plugins/com.at747.noramadjust/
  NORAMAdjust.dll
  mod.ini
```

No PatchTool step — this mod has **zero IL patches**.

### Option B — build and deploy from source

Close the game before copying files.

**NOLoader** (requires NOLoader deployed first):

```powershell
# From NOLoader_Engine (one-time loader setup)
.\scripts\deploy-noloader.ps1 -Configuration DEV_SDK

# From this repo
.\scripts\deploy-noram-adjust-mod.ps1
```

**BepInEx:**

```powershell
.\scripts\fetch-bepinex-libs.ps1          # once per machine
.\scripts\deploy-noram-adjust-bepinex.ps1
```

---

## Configuration

Edit `mod.ini` in the mod/plugin folder. Defaults are created on first run if missing.

```ini
[NORAM]
enabled=1
memory_reservoir_mb=5300
chunk_mb=64
```

| Key | Default | Description |
|-----|---------|-------------|
| `enabled` | `1` | `1` = allocate reservoir; `0` = mod loads but skips allocation |
| `memory_reservoir_mb` | `5300` | Target size in MB (decimal GB: **5.3 GB = 5300 MB**) |
| `chunk_mb` | `64` | Allocation chunk size in MB |

Disable without removing the mod:

```ini
[NORAM]
enabled=0
```

Set `memory_reservoir_mb=0` to skip allocation while keeping `enabled=1`.

---

## Compatibility

| Combination | Result |
|-------------|--------|
| **MpClientOpt** + NORAMAdjust (`memory_budget=1`) | **Avoid.** MpClientOpt reserves ~5120 MB by default; combined usage can exceed **10 GB**. Disable `memory_budget` in MpClientOpt or use only one RAM mod. |
| NORAMAdjust + other NOLoader mods | Safe — no IL patch overlap. |
| NOLoader + BepInEx in same game | **Unsupported** — loader conflict. |

---

## Repository layout

```text
NORAMAdjust/
  NORAMAdjust.sln                 # Visual Studio solution
  NOLoader.NORAMAdjust.csproj     # NOLoader mod (INOMod)
  BepInEx/NORAMAdjust/            # BepInEx plugin project
  src/
    NoramAdjustMod.cs             # NOLoader entry
    RamReservoir.cs               # Shared RAM logic
    NoramConfig.cs                # Shared INI parsing
  mod.json                        # NOLoader manifest (id: com.at747.noramadjust)
  mod.ini                         # Default config template
  scripts/
    deploy-noram-adjust-mod.ps1
    deploy-noram-adjust-bepinex.ps1
    fetch-bepinex-libs.ps1
  Directory.Build.props           # NuclearOptionRoot, NOLoaderEngineRoot
```

**Mod identity:** `com.at747.noramadjust` · **GUID:** `b3c4d5e6-f7a8-4901-9283-7465a6b7c8d9`

---

## Build from source

Open `NORAMAdjust.sln` in Visual Studio 2022+ or use the CLI:

```powershell
dotnet build NORAMAdjust.sln -c DEV_SDK    # NOLoader dev
dotnet build NORAMAdjust.sln -c RDYTU      # NOLoader player channel
dotnet build NORAMAdjust.sln -c Release    # BepInEx plugin
```

| Project | Configuration | Output DLL |
|---------|---------------|------------|
| `NOLoader.NORAMAdjust` | `DEV_SDK` / `RDYTU` | `bin/<cfg>/net48/NOLoader.NORAMAdjust.dll` |
| `NORAMAdjust.BepInEx` | `Release` | `BepInEx/NORAMAdjust/bin/Release/net48/NORAMAdjust.dll` |

**Prerequisites:**

- [.NET SDK](https://dotnet.microsoft.com/download) (builds `net48`)
- `../NOLoader_Engine` checked out beside this repo
- Optional: run `fetch-bepinex-libs.ps1` before building the BepInEx project (or copy `BepInEx.dll` / `0Harmony.dll` from another local BepInEx mod into `BepInEx/lib/`)

Override game path via `Directory.Build.user.props`:

```xml
<Project>
  <PropertyGroup>
    <NuclearOptionRoot>C:\Path\To\Nuclear Option</NuclearOptionRoot>
    <NOLoaderEngineRoot>C:\Path\To\NOLoader_Engine</NOLoaderEngineRoot>
  </PropertyGroup>
</Project>
```

---

## Verification

- [ ] Main menu loads without loader errors
- [ ] Log contains `[NORAMAdjust] reserved=5300MB`
- [ ] Task Manager: managed memory ~**+5.3 GB** vs baseline at menu
- [ ] `enabled=0` in `mod.ini` → no extra reservation after restart
- [ ] Game exit → reserved memory released

**NOLoader logs:** `NOLoader/logs/noloader_ring.log`  
**BepInEx logs:** `BepInEx/LogOutput.log`

---

## License

[MIT](LICENSE). Nuclear Option is property of its respective owners.

---

## Keywords

nuclear-option, bepinex, harmony, mod, noramadjust, csharp, unity

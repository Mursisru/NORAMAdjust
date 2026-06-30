# Changelog

## [1.1.0] - 2026-06-30

### Changed
- Documentation refresh: Developer header, badges, GitHub Alerts, Keywords, gitignore hygiene.


All notable changes to this project are documented in this file.

Format follows [Keep a Changelog](https://keepachangelog.com/). Versions use [Semantic Versioning](https://semver.org/).

## [1.1.0] — 2026-06-26

### Added

- **BepInEx 5** plugin (`NORAMAdjust.BepInEx`) — same RAM reservoir as NOLoader build.
- Visual Studio solution **`NORAMAdjust.sln`** (NOLoader + BepInEx projects).
- Scripts: `fetch-bepinex-libs.ps1`, `deploy-noram-adjust-bepinex.ps1`.
- Release zip for BepInEx: `NORAMAdjust-v1.1.0-BepInEx.zip`.
- Full English documentation (README, verification checklist, repo layout).

### Changed

- README restructured: install options, compatibility matrix, build table.
- NOLoader release zip updated to `NORAMAdjust-v1.1.0-NOLoader.zip`.

### Build

- Display version: `1.1.0 Build DEV1P3`
- BepInEx `[BepInPlugin]` version: `1.1.0`

---

## [1.0.0] — 2026-06-26

### Added

- Initial NOLoader mod (`com.at747.noramadjust`).
- Pre-allocates **5300 MB** managed RAM at `MainMenu` load stage.
- Configurable `mod.ini`: `enabled`, `memory_reservoir_mb`, `chunk_mb`.
- Active for solo, host, and dedicated client (no session gate).
- Zero IL patches — PatchTool not required.
- Deploy script: `scripts/deploy-noram-adjust-mod.ps1`.
- GitHub release: `NORAMAdjust-v1.0.0-NOLoader.zip`.

### Build

- Display version: `1.0.0 Build DEV1P1`

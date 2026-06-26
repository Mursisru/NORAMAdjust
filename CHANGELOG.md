# Changelog

All notable changes to this project are documented in this file.

## [1.0.0] — 2026-06-26

### Added

- Initial release: NORAM Adjust NOLoader mod (`com.at747.noramadjust`).
- Pre-allocates **5300 MB** managed RAM reservoir at `MainMenu` load stage.
- Configurable via `mod.ini`: `enabled`, `memory_reservoir_mb`, `chunk_mb`.
- Works for solo, host, and dedicated client (no session gate).
- Zero IL patches — no PatchTool required on deploy.
- Deploy script: `scripts/deploy-noram-adjust-mod.ps1`.

### Build

- Display version: `1.0.0 Build DEV1P1`

param(
    [string]$GameRoot = "C:\Program Files (x86)\Steam\steamapps\common\Nuclear Option",
    [string]$Configuration = "DEV_SDK"
)

$ErrorActionPreference = "Stop"
$RepoRoot = Split-Path -Parent $PSScriptRoot
$EngineRoot = Join-Path (Split-Path -Parent $RepoRoot) "NOLoader_Engine"

if (-not (Test-Path $EngineRoot)) {
    Write-Error "NOLoader_Engine not found at $EngineRoot"
}

if (Get-Process -Name "NuclearOption" -ErrorAction SilentlyContinue) {
    Write-Error "Close Nuclear Option before deploy."
}

$project = Join-Path $RepoRoot "NOLoader.NORAMAdjust.csproj"
dotnet build $project -c $Configuration --verbosity minimal
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$dll = Join-Path $RepoRoot "bin\$Configuration\net48\NOLoader.NORAMAdjust.dll"
$modConfigProject = Join-Path $EngineRoot "DEV.SDK\shared\NOLoader.ModConfig\NOLoader.ModConfig.csproj"
$modConfigDll = Join-Path $EngineRoot "DEV.SDK\shared\NOLoader.ModConfig\bin\$Configuration\net48\NOLoader.ModConfig.dll"
if (-not (Test-Path $modConfigDll)) {
    dotnet build $modConfigProject -c $Configuration --verbosity minimal
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}

$modRoot = Join-Path $GameRoot "NOLoader\mods\NORAMAdjust"
New-Item -ItemType Directory -Path $modRoot -Force | Out-Null

Copy-Item -Force $dll (Join-Path $modRoot "NOLoader.NORAMAdjust.dll")
Copy-Item -Force $modConfigDll (Join-Path $modRoot "NOLoader.ModConfig.dll")
Copy-Item -Force (Join-Path $RepoRoot "mod.json") (Join-Path $modRoot "mod.json")

$iniDst = Join-Path $modRoot "mod.ini"
if (-not (Test-Path $iniDst)) {
    Copy-Item -Force (Join-Path $RepoRoot "mod.ini") $iniDst
    Write-Host "Created default mod.ini at $iniDst"
} else {
    Write-Host "Keeping existing mod.ini at $iniDst"
}

& (Join-Path $EngineRoot "scripts\pack-mod-rdytu.ps1") -ModFolder $modRoot

$deployedDll = Get-Item (Join-Path $modRoot "NOLoader.NORAMAdjust.dll")
Write-Host "NORAMAdjust deployed to $modRoot"
Write-Host "DLL: $($deployedDll.FullName) ($([math]::Round($deployedDll.Length / 1KB, 1)) KB)"

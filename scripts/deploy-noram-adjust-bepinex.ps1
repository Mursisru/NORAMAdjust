param(
    [string]$GameRoot = "C:\Program Files (x86)\Steam\steamapps\common\Nuclear Option",
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$RepoRoot = Split-Path -Parent $PSScriptRoot
$project = Join-Path $RepoRoot "BepInEx\NORAMAdjust\NORAMAdjust.BepInEx.csproj"

$libDir = Join-Path $RepoRoot "BepInEx\lib"
if (-not (Test-Path (Join-Path $libDir "BepInEx.dll"))) {
    Write-Host "BepInEx build refs missing - running fetch-bepinex-libs.ps1"
    & (Join-Path $PSScriptRoot "fetch-bepinex-libs.ps1")
}

dotnet build $project -c $Configuration --verbosity minimal
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$dll = Join-Path $RepoRoot "BepInEx\NORAMAdjust\bin\$Configuration\net48\NORAMAdjust.dll"
$pluginRoot = Join-Path $GameRoot "BepInEx\plugins\com.at747.noramadjust"
New-Item -ItemType Directory -Path $pluginRoot -Force | Out-Null

Copy-Item -Force $dll (Join-Path $pluginRoot "NORAMAdjust.dll")

$iniDst = Join-Path $pluginRoot "mod.ini"
if (-not (Test-Path $iniDst)) {
    Copy-Item -Force (Join-Path $RepoRoot "mod.ini") $iniDst
    Write-Host "Created default mod.ini at $iniDst"
} else {
    Write-Host "Keeping existing mod.ini at $iniDst"
}

$deployedDll = Get-Item (Join-Path $pluginRoot "NORAMAdjust.dll")
Write-Host "BepInEx plugin deployed to $pluginRoot"
$sizeKb = [math]::Round($deployedDll.Length / 1024, 1)
Write-Host ('DLL: ' + $deployedDll.FullName + ' (' + $sizeKb + ' KiB)')

param(
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$Root = Resolve-Path (Join-Path $PSScriptRoot "..")
$Project = Join-Path $Root "runtime\DressmakerLATAM\DressmakerLATAM.csproj"
$PluginDir = Join-Path $Root "build\pilot\BepInEx\plugins\DressmakerLATAM"

$RequiredDlls = @(
    "runtime\DressmakerLATAM\lib\BepInEx\BepInEx.dll",
    "runtime\DressmakerLATAM\lib\BepInEx\0Harmony.dll",
    "runtime\DressmakerLATAM\lib\Game\UnityEngine.CoreModule.dll",
    "runtime\DressmakerLATAM\lib\Game\Unity.Localization.dll",
    "runtime\DressmakerLATAM\lib\Game\Unity.Addressables.dll",
    "runtime\DressmakerLATAM\lib\Game\Unity.ResourceManager.dll"
)

foreach ($RelativePath in $RequiredDlls) {
    $FullPath = Join-Path $Root $RelativePath
    if (-not (Test-Path -LiteralPath $FullPath)) {
        throw "Missing required reference: $RelativePath"
    }
}

$dotnet = Get-Command dotnet -ErrorAction SilentlyContinue
if (-not $dotnet) {
    throw "dotnet was not found on PATH. Install a .NET SDK before building."
}

$Sdks = & dotnet --list-sdks
if (-not $Sdks) {
    throw "dotnet is available, but no .NET SDK is installed. Install a .NET SDK that can build netstandard2.1."
}

Write-Host "Using .NET SDK:"
$Sdks | ForEach-Object { Write-Host "  $_" }

& dotnet build $Project -c $Configuration
if ($LASTEXITCODE -ne 0) {
    throw "dotnet build failed with exit code $LASTEXITCODE"
}

$BuiltDll = Join-Path $Root "runtime\DressmakerLATAM\bin\$Configuration\netstandard2.1\DressmakerLATAM.dll"
if (-not (Test-Path -LiteralPath $BuiltDll)) {
    throw "Build succeeded, but output DLL was not found: $BuiltDll"
}

New-Item -ItemType Directory -Force -Path $PluginDir | Out-Null
Copy-Item -LiteralPath $BuiltDll -Destination (Join-Path $PluginDir "DressmakerLATAM.dll") -Force

Write-Host "Copied DressmakerLATAM.dll to:"
Write-Host "  $PluginDir"

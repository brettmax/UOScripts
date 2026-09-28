<#
Stages a runnable T2A server from this package set. PowerShell counterpart of
tools/stage-server.sh for Windows without Git Bash or WSL; works on Windows
PowerShell 5.1 and PowerShell 7.

  tools\stage-server.ps1 [-Configuration Debug|Release] [-Output <dir>]

Builds AxmolUO.Server.slnx, then assembles <Output> (default: Staging\) from
ModernUO's Distribution, the ModernSpawner module and the script assemblies,
and overlays this repository's Distribution\ (T2A expansion preset and
assemblies.json). Saves, Logs and existing Configuration in <Output> are left
alone, so re-staging does not wipe a shard.

Headless first boot: set UO_DATA_DIR to the Ultima Online data folder (maps,
statics, tiledata, multis) and the first stage also writes
Configuration\modernuo.json, so the server boots without console prompts.
LISTEN (default 0.0.0.0:2593) and SERVER_NAME (default AxmolUO) tune it.
#>
param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',
    [string]$Output
)

$ErrorActionPreference = 'Stop'

function Invoke-Native {
    param([string]$Command, [string[]]$Arguments)
    & $Command @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "$Command $($Arguments -join ' ') failed with exit code $LASTEXITCODE"
    }
}

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
if (-not $Output) {
    $Output = Join-Path $repoRoot 'Staging'
}

Invoke-Native git @('-C', $repoRoot, 'submodule', 'update', '--init')
Invoke-Native dotnet @('build', (Join-Path $repoRoot 'AxmolUO.Server.slnx'), '-c', $Configuration)

# ModernSpawner is a library; publish it so YamlDotNet travels with it.
$spawnerPub = Join-Path ([IO.Path]::GetTempPath()) ([IO.Path]::GetRandomFileName())
try {
    $spawnerProject = Join-Path $repoRoot 'Modules/ModernSpawner/Projects/ModernSpawner/ModernSpawner.csproj'
    Invoke-Native dotnet @('publish', $spawnerProject, '-c', $Configuration, '--no-build', '-o', $spawnerPub)

    New-Item -ItemType Directory -Force -Path (Join-Path $Output 'Assemblies') | Out-Null
    $distribution = Join-Path $repoRoot 'ModernUO/Distribution'
    Get-ChildItem -LiteralPath $distribution -Force |
        Where-Object { @('Saves', 'Logs', 'Configuration') -notcontains $_.Name } |
        ForEach-Object { Copy-Item -LiteralPath $_.FullName -Destination $Output -Recurse -Force }

    foreach ($dll in 'ModernSpawner.dll', 'YamlDotNet.dll') {
        Copy-Item -LiteralPath (Join-Path $spawnerPub $dll) -Destination (Join-Path $Output 'Assemblies') -Force
    }
}
finally {
    Remove-Item -LiteralPath $spawnerPub -Recurse -Force -ErrorAction SilentlyContinue
}

# Package overlay. The expansion preset is only written on first stage, so a
# shard that later edits its Configuration keeps its choice.
Copy-Item -LiteralPath (Join-Path $repoRoot 'Distribution/Data/assemblies.json') `
    -Destination (Join-Path $Output 'Data/assemblies.json') -Force
$configDir = Join-Path $Output 'Configuration'
New-Item -ItemType Directory -Force -Path $configDir | Out-Null
$expansionFile = Join-Path $configDir 'expansion.json'
if (-not (Test-Path -LiteralPath $expansionFile)) {
    Copy-Item -LiteralPath (Join-Path $repoRoot 'Distribution/Configuration/expansion.json') -Destination $expansionFile
}

$serverConfig = Join-Path $configDir 'modernuo.json'
if ($env:UO_DATA_DIR -and -not (Test-Path -LiteralPath $serverConfig)) {
    if (-not (Test-Path -LiteralPath $env:UO_DATA_DIR -PathType Container)) {
        throw "UO_DATA_DIR '$env:UO_DATA_DIR' does not exist"
    }
    $listen = if ($env:LISTEN) { $env:LISTEN } else { '0.0.0.0:2593' }
    $serverName = if ($env:SERVER_NAME) { $env:SERVER_NAME } else { 'AxmolUO' }
    $settings = [ordered]@{
        assemblyDirectories = @()
        dataDirectories     = @((Resolve-Path -LiteralPath $env:UO_DATA_DIR).Path)
        listeners           = @($listen)
        settings            = [ordered]@{ 'serverListing.serverName' = $serverName }
    }
    # No BOM: Windows PowerShell 5.1's Set-Content -Encoding utf8 would add one.
    [IO.File]::WriteAllText($serverConfig, ($settings | ConvertTo-Json -Depth 4), (New-Object Text.UTF8Encoding $false))
}

Write-Host "Staged T2A server in $Output (run: dotnet `"$(Join-Path $Output 'ModernUO.dll')`")"

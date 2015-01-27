[CmdletBinding()]
param (
)

function CheckLastExitCode {
    param ([int[]]$SuccessCodes = @(0), [scriptblock]$CleanupScript=$null)

    if ($SuccessCodes -notcontains $LastExitCode) {
        if ($CleanupScript) {
            "Executing cleanup script: $CleanupScript"
            &$CleanupScript
        }
        $msg = @"
EXE RETURNED EXIT CODE $LastExitCode
CALLSTACK:$(Get-PSCallStack | Out-String)
"@
        throw $msg
    }
}

function MSBuild
{
    param (
        [String] $pathToMSBuild,
        [String] $pathToSolution,
        [String[]] $arguments
    )

    &$pathToMSBuild $pathToSolution $arguments
    CheckLastExitCode
}

function MSBuildNet40
{
    param (
        [String] $pathToSolution,
        [String[]] $arguments
    )
    $pathToMSBuild = "C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe"

    MSBuild $pathToMSBuild $pathToSolution $arguments
}

$ErrorActionPreference = "Stop"

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptName = Split-Path -Leaf $MyInvocation.MyCommand.Path

Write-Verbose ("Executing " + $scriptName)

Import-Module DevFacto.Automation -Force

$rootDir = Split-Path -Parent $scriptRoot

$version = Get-Content ($rootDir + "\version.txt")

$packageDir = "$rootDir/buildResult/"


pushd $rootDir
MSBuildNet40 'source\SiSystems.ClientApp.WebOnly.sln' @('/m', '/t:Build', '/p:Configuration=Release', '/p:RunOctoPack=true', '/p:WebConfigTransform=true')

mkdir -Force $packageDir

Get-ChildItem -recurse -filter *$version.nupkg | ? { -not $_.Directory.ToString().ToLower().Contains("\packages\")} | % { copy $_.FullName $packageDir }

popd

Write-Verbose ($scriptName + " complete")

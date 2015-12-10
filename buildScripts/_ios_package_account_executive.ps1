[CmdletBinding()]
param (
)

Import-Module DevFacto.Automation -Force

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptName = Split-Path -Leaf $MyInvocation.MyCommand.Path
$rootDir = Split-Path -Parent $scriptRoot
$packageDir = "$rootDir/buildResult/"

function PackageBuild
{
    param (
        [String] $configuration
    )

    pushd $rootDir\source\AccountExecutiveApp\AccountExecutiveApp.iOS\Octopus

    copy $packageDir\*$configuration.ipa .\

    ls *.ipa | % { $_.Name -match '.*AccountExecutive.*?(?<version>\d+(\.\d+)+).*'}

    Update-Nuspec -FilePath ".\$configuration.nuspec" -Version $Matches['version']

    nuget pack ".\$configuration.nuspec"

    mv *.nupkg $packageDir\

    rm *.ipa

    popd
}

$ErrorActionPreference = "Stop"

Write-Verbose ("Executing " + $scriptName)

PackageBuild "Test"
PackageBuild "Release"

Write-Verbose ($scriptName + " complete")

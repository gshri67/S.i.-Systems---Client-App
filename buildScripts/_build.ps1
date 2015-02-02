[CmdletBinding()]
param (
)

$ErrorActionPreference = "Stop"

$scriptName = Split-Path $MyInvocation.MyCommand.Path -Leaf

Write-Verbose ("Executing " + $scriptName)

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

function MSBuildNet35
{
    param (
        [String] $pathToSolution,
        [String[]] $arguments
    )
    $pathToMSBuild = "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe"

    MSBuild $pathToMSBuild $pathToSolution $arguments
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

#Import-Module DevFacto.Automation -Force

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptName = Split-Path -Leaf $MyInvocation.MyCommand.Path

$rootDir = Split-Path -Parent $scriptRoot
$version = Get-Content ($rootDir + "\version.txt")

pushd $rootDir

#download nuget.exe for package restore
$sourceNugetExe = "http://nuget.org/nuget.exe"
$targetNugetExe = "$rootDir\nuget.exe"

if(-Not (Test-Path $targetNugetExe)){
	Invoke-WebRequest $sourceNugetExe -OutFile $targetNugetExe
}
Write-Output "Restoring packages"
& .\nuget.exe restore 'source\SiSystems.ClientApp.sln'

Write-Output "Building Solution"

#Build Web Project
MSBuildNet40 'source\Web\Web.csproj' @('/m', '/t:Build', '/p:Configuration=Release', '/p:RunCodeAnalysis=true')
MSBuildNet40 'source\Database\Database.csproj' @('/m', '/t:Build', '/p:Configuration=Release')

#Build All Included Test Projects (any *.Tests.csproj)
$testProjectFiles = Get-ChildItem -r *.Tests.csproj
forEach($projectFile in $testProjectFiles){
	MSBuildNet40 "$projectFile" @('/m', '/t:Build', '/p:Configuration=Release', '/p:RunCodeAnalysis=true')	
}

popd

Write-Verbose ($scriptName + " complete")

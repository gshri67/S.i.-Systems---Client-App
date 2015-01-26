[CmdletBinding()]
param (
)

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptName = Split-Path -Leaf $MyInvocation.MyCommand.Path

Write-Verbose ("Executing " + $scriptName)

Import-Module DevFacto.Automation -Force

$rootDir = Split-Path -Parent $scriptRoot
$version = Get-Content ($rootDir + "\version.txt")

Write-Verbose ($scriptName + " complete")

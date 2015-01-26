[CmdletBinding()]
param (
       [Parameter(Mandatory=$TRUE)]
       [Int] $BuildNumber
)

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptName = Split-Path -Leaf $MyInvocation.MyCommand.Path

Write-Verbose ("Executing " + $scriptName)

Import-Module DevFacto.Automation -Force

$version = (Get-Content ($scriptRoot + "\..\version.txt")) + "." + $BuildNumber
Set-Content ($scriptRoot + "\..\version.txt") $version

#Update Assembly Info files
$files = Get-ChildItem($scriptRoot + "\..\source\") -Recurse -Filter AssemblyInfo.cs

if ($files) {
    $files | % {
        $filename = $_.Directory.ToString() + '\' + $_.Name
        Set-AssemblyInfoConfig -FilePath $filename -Version $version -FileVersion $version -Copyright "Copyright © S.i. Systems $((get-date).Year)" -Company "S.i. Systems"
    }
}

#Update Nuspec files
$files = Get-ChildItem($scriptRoot + "\..\source\") -Recurse -Filter *.nuspec

if ($files) {
    $files | % {
        $filename = $_.Directory.ToString() + '\' + $_.Name
        Update-Nuspec -FilePath $filename -Version $version
    }
}

Write-Verbose ($scriptName + " complete")

[CmdletBinding()]
param (
)

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptName = Split-Path -Leaf $MyInvocation.MyCommand.Path
$rootDir = Split-Path -Parent $scriptRoot

pushd (Split-Path -Parent $scriptRoot)

& "$rootDir\nuget.exe" install NUnit.Runners -Version 2.6.3
& "$rootDir\nuget.exe" install OpenCover -Version 4.5.3427

$testPath = ".\_build-testing"
mkdir -f $testPath

$opencoverCommand = "..\OpenCover.4.5.3427\OpenCover.Console.exe"
$nunitCommand = "..\NUnit.Runners.2.6.3\tools\nunit-console-x86.exe"
$nunitArgs = "/xml:..\nunitResults.xml /framework:net-4.0 /exclude:Database"

Get-ChildItem -Recurse -Include *.Tests.dll source\ |
    Where { $_.FullName -notlike "*\obj\*" } |
    ForEach-Object {
        $testPathExt = {"$($_.Name)" -replace ".Tests.dll", ""}
        $nunitArgs += {" {0}\{1}" -f "$testPathExt", "$($_.Name)"}
        split-path $_.FullName
    } |
    Select-Object -unique |
    ForEach-Object {
        copy-item -force "$_\*" {"{0}\{1}" -f "$testPath", "$testPathExt"}
    }


Write-Verbose "Running nunit: $nunitCommand"
Write-Verbose "with arguments: $nunitArgs"

$xmlReportName = "CodeCoverageResult.xml"

pushd "$testPath"
cmd /C "$opencoverCommand -target:`"$nunitCommand`" -targetargs:`"$nunitArgs`" -targetdir:`"$pwd`" -filter:`"+[DevFacto*]* -[*]Tests.*`" -register:user -output:`"..\$xmlReportName`""
popd

& "$rootDir\nuget.exe" install ReportGenerator -Version 2.0.2
& "$rootDir\nuget.exe" install OpenCoverToCoberturaConverter -Version 0.2.0

.\ReportGenerator.2.0.2.0\ReportGenerator.exe -reports:$xmlReportName -targetDir:CodeCoverageHTML
.\OpenCoverToCoberturaConverter.0.2.0.0\OpenCoverToCoberturaConverter.exe -input:$xmlReportName -output:CodeCoverageCobertura.xml -sources:"$pwd"

Write-Verbose "nunit complete"


popd

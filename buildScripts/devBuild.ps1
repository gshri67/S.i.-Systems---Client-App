Import-Module DevFacto.Automation -Force

$BuildNumber = 0

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptName = Split-Path -Leaf $MyInvocation.MyCommand.Path

pushd $scriptRoot

.\_prep_build.ps1 $BuildNumber -Verbose
.\_build.ps1 -Verbose
.\_post_build.ps1 -Verbose
.\_package.ps1 -Verbose

popd

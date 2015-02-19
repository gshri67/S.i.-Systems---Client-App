#!/bin/bash

resultsDir=./buildResult

#update version numbers
build_number=${1:-"0"}
read -r version < version.txt

find ./source -name 'Info.plist' | while read plist; do
	/usr/libexec/PlistBuddy -c "Set :CFBundleVersion '${version}.${build_number}'" $plist
	/usr/libexec/PlistBuddy -c "Set :CFBundleShortVersionString '${version}'" $plist
done

#unlock encryption keys for signing
security default-keychain -s si-systems-xcode.keychain
security unlock-keychain -p $SI_SIGN_KEY si-systems-xcode.keychain

mkdir $resultsDir

function CreateBuild {
	/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build "--configuration:${1}|iPhone" ./source/SiSystems.ClientApp.sln

	find ./source -path "*/${1}/*" -name "*.ipa" | while read package; do
		short_name=${package//*\///}
		short_name=${short_name/%.ipa/-$1.ipa}

		echo "Archiving ${package} to ${resultsDir}/${short_name}"
		cp $package $resultsDir/$short_name
	done
}

CreateBuild "Test"
CreateBuild "Release"

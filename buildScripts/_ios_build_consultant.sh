#!/bin/bash
#NOTE: INSIGHTS_API_KEY_* and SI_SIGN_KEY are set up in the 'Consultant App - iOS App Jenkins' Configuration

resultsDir=./buildResult

#update version numbers
build_number=${1:-"0"}
read -r version < version.txt


#unlock encryption keys for signing
security default-keychain -s si-systems-xcode.keychain
security unlock-keychain -p $SI_SIGN_KEY si-systems-xcode.keychain

mkdir $resultsDir

function CreateBuild {
	find ./source -name 'Info.plist' -not -path '*/bin/*' | while read plist; do
		/usr/libexec/PlistBuddy -c "Set :CFBundleVersion '${version}.${build_number}${3+.$3}'" $plist
		/usr/libexec/PlistBuddy -c "Set :CFBundleShortVersionString '${version}'" $plist
		/usr/libexec/PlistBuddy -c "Set :SIAlumniXamarinInsightsAPIKey '${1}'" $plist
	done

	mono ./source/.nuget/nuget.exe restore ./source/SiSystems.ClientApp.sln
	/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build "--configuration:${2}|iPhone" ./source/SiSystems.ClientApp.sln

	find ./source -path "*/${2}/*" -name "*Consultant*.ipa" | while read package; do
		short_name=${package//*\///}
		short_name=${short_name/%.ipa/-$2.ipa}

		echo "Archiving ${package} to ${resultsDir}/${short_name}"
		cp $package $resultsDir/$short_name
	done
}

CreateBuild $INSIGHTS_API_KEY_TEST "Test" 0
CreateBuild $INSIGHTS_API_KEY_RELEASE "Release" 1

security default-keychain -s login.keychain

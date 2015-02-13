#!/bin/bash

#unlock encryption keys for signing
security default-keychain -s si-systems-xcode.keychain
security unlock-keychain -p $SI_SIGN_KEY si-systems-xcode.keychain

mkdir results/

#Build Beta App for Test Flight
/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build "--configuration:Test|iPhone" ./source/SiSystems.ClientApp.sln

find ./source -path '*/Test/*' -name '*.ipa' | while read package; do
	short_name=${package//*\///}
	beta_name=${short_name/%.ipa/-beta.ipa}
	mv $package ./results/$beta_name
done

#Build AppStore version
/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build "--configuration:Release|iPhone" ./source/SiSystems.ClientApp.sln

find ./source -path '*/Release/*' -name '*.ipa' | while read package; do
	short_name=${package//*\///}
	mv $package ./results/$short_name
done


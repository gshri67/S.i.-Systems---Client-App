#!/bin/bash

#unlock encryption keys for signing
security default-keychain -s si-systems-xcode.keychain
security unlock-keychain -p $SI_SIGN_KEY si-systems-xcode.keychain

#ship it!
apple_id=it.infrastructure@sisystems.com
app_id=1039202852

find . -name "*.ipa" | while read package; do
    echo "Attempting to upload $package"
    deliver testflight -u $apple_id -a $app_id $package
done

#restore default keychain
security default-keychain -s login.keychain

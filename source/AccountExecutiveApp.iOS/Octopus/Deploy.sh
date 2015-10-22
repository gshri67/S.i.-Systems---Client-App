#!/bin/bash

export SI_SIGN_KEY=#{SI_KEYCHAIN_PASSWORD}

echo "Fixing file endings for shell scripts..."
find . -name '*.sh' ! -name "Deploy.sh" | while read file; do
    file2=file+'2';
    cat $file | col -b > $file2;
    mv -f $file2 $file;
    chmod +x $file;
done;
echo "done"

./_ios_publish.sh


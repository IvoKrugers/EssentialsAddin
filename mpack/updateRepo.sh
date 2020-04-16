#!/bin/sh

SCRIPTFILE=$0

#Get the absolute path to the containing folder
PROJECTFOLDER=${SCRIPTFILE%/*}

cd ${PROJECTFOLDER}

pwd

cd EssentialsAddin

rm *.mpack
mono /Applications/Visual\ Studio.app/Contents/Resources/lib/monodevelop/bin/vstool.exe setup pack ../../EssentialsAddin/bin/Release/EssentialsAddin.dll

cd ..

/Applications/Visual\ Studio.app/Contents/MacOS/vstool setup rep-build .
rm index.html
#xml ed -i "/Repository" -t text -n "Name" -v "Essentials Addin by Ivo Krugers" main.mrep
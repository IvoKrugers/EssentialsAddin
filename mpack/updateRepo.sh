#!/bin/sh

SCRIPTFILE=$0

#Get the absolute path to the containing folder
PROJECTFOLDER=${SCRIPTFILE%/*}

cd ${PROJECTFOLDER}

pwd

rm *.mpack
mono /Applications/Visual\ Studio.app/Contents/Resources/lib/monodevelop/bin/vstool.exe setup pack ../EssentialsAddin/bin/Release/EssentialsAddin.dll


/Applications/Visual\ Studio.app/Contents/MacOS/vstool setup rep-build .
#rm index.html
#xml ed -i "/Repository" -t text -n "Name" -v "Extensions by Arthur Demanuele" main.mrep
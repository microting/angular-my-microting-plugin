#!/bin/bash

if [ ! -d "/var/www/microting/angular-my-microting-plugin" ]; then
  cd /var/www/microting
  su ubuntu -c \
  "git clone https://github.com/microting/angular-my-microting-plugin.git -b stable"
fi

cd /var/www/microting/angular-my-microting-plugin
git pull
su ubuntu -c \
"dotnet restore eFormAPI/Plugins/MyMicroting.Pn/MyMicroting.Pn.sln"

echo "################## START GITVERSION ##################"
export GITVERSION=`git describe --abbrev=0 --tags | cut -d "v" -f 2`
echo $GITVERSION
echo "################## END GITVERSION ##################"
su ubuntu -c \
"dotnet publish eFormAPI/Plugins/MyMicroting.Pn/MyMicroting.Pn.sln -o out /p:Version=$GITVERSION --runtime linux-x64 --configuration Release"

su ubuntu -c \
"rm -fR /var/www/microting/eform-angular-frontend/eform-client/src/app/plugins/modules/my-microting-pn"

su ubuntu -c \
"cp -av /var/www/microting/angular-my-microting-plugin/eform-client/src/app/plugins/modules/my-microting-pn /var/www/microting/eform-angular-frontend/eform-client/src/app/plugins/modules/my-microting-pn"

su ubuntu -c \
"mkdir -p /var/www/microting/eform-angular-frontend/eFormAPI/eFormAPI.Web/out/Plugins/"

su ubuntu -c \
"rm -fR /var/www/microting/eform-angular-frontend/eFormAPI/eFormAPI.Web/out/Plugins/MyMicroting"

su ubuntu -c \
"cp -av /var/www/microting/angular-my-microting-plugin/out /var/www/microting/eform-angular-frontend/eFormAPI/eFormAPI.Web/out/Plugins/MyMicroting"

echo "Recompile angular"
cd /var/www/microting/eform-angular-frontend/eform-client
su ubuntu -c \
"/var/www/microting/angular-my-microting-plugin/testinginstallpn.sh"
su ubuntu -c \
"export NODE_OPTIONS=--max_old_space_size=8192 && time GENERATE_SOURCEMAP=false npm run build"
echo "Recompiling angular done"

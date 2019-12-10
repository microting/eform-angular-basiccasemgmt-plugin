#!/bin/bash

if [ ! -d "/var/www/microting/eform-angular-basiccasemgmt-plugin" ]; then
  cd /var/www/microting
  su ubuntu -c \
  "git clone https://github.com/microting/eform-angular-basiccasemgmt-plugin.git -b stable"
fi

cd /var/www/microting/eform-angular-basiccasemgmt-plugin
git pull
su ubuntu -c \
"dotnet restore eFormAPI/Plugins/CaseManagement.Pn/CaseManagement.Pn.sln"

echo "################## START GITVERSION ##################"
export GITVERSION=`git describe --abbrev=0 --tags | cut -d "v" -f 2`
echo $GITVERSION
echo "################## END GITVERSION ##################"
su ubuntu -c \
"dotnet publish eFormAPI/Plugins/CaseManagement.Pn/CaseManagement.Pn.sln -o out /p:Version=$GITVERSION --runtime linux-x64 --configuration Release"

if [ -d "/var/www/microting/eform-angular-frontend/eform-client/src/app/plugins/modules/case-management-pn"]; then
	su ubuntu -c \
	"rm -fR /var/www/microting/eform-angular-frontend/eform-client/src/app/plugins/modules/case-management-pn"
fi

su ubuntu -c \
"cp -av /var/www/microting/eform-angular-basiccasemgmt-plugin/eform-client/src/app/plugins/modules/case-management-pn /var/www/microting/eform-angular-frontend/eform-client/src/app/plugins/modules/case-management-pn"
su ubuntu -c \
"mkdir -p /var/www/microting/eform-angular-frontend/eFormAPI/eFormAPI.Web/out/Plugins/"

if [ -d "/var/www/microting/eform-angular-frontend/eFormAPI/eFormAPI.Web/out/Plugins/CaseManagement"]; then
	su ubuntu -c \
	"rm -fR /var/www/microting/eform-angular-frontend/eFormAPI/eFormAPI.Web/out/Plugins/CaseManagement"
fi

su ubuntu -c \
"cp -av /var/www/microting/eform-angular-basiccasemgmt-plugin/eFormAPI/Plugins/CaseManagement.Pn/CaseManagement.Pn/out /var/www/microting/eform-angular-frontend/eFormAPI/eFormAPI.Web/out/Plugins/CaseManagement"


echo "Recompile angular"
cd /var/www/microting/eform-angular-frontend/eform-client
su ubuntu -c \
"/var/www/microting/eform-angular-basiccasemgmt-plugin/testinginstallpn.sh"
su ubuntu -c \
"npm run build"
echo "Recompiling angular done"



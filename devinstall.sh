#!/bin/bash
cd ~
pwd

if [ -d "Documents/workspace/microting/eform-angular-frontend/eform-client/src/app/plugins/modules/case-management-pn" ]; then
	rm -fR Documents/workspace/microting/eform-angular-frontend/eform-client/src/app/plugins/modules/case-management-pn
fi

cp -av Documents/workspace/microting/eform-angular-basiccasemgmt-plugin/eform-client/src/app/plugins/modules/case-management-pn Documents/workspace/microting/eform-angular-frontend/eform-client/src/app/plugins/modules/case-management-pn

if [ -d "Documents/workspace/microting/eform-angular-frontend/eFormAPI/Plugins/CaseManagement.Pn" ]; then
	rm -fR Documents/workspace/microting/eform-angular-frontend/eFormAPI/Plugins/CaseManagement.Pn
fi

cp -av Documents/workspace/microting/eform-angular-basiccasemgmt-plugin/eFormAPI/Plugins/CaseManagement.Pn Documents/workspace/microting/eform-angular-frontend/eFormAPI/Plugins/CaseManagement.Pn

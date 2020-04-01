#!/bin/bash
cd ~
pwd

mkdir -p Documents/workspace/microting/eform-angular-trashinspection-plugin/eFormAPI/Plugins

rm -fR Documents/workspace/microting/eform-angular-frontend/eform-client/src/app/plugins/modules/case-management-pn
rm -fR Documents/workspace/microting/eform-angular-frontend/eform-client/e2e/Tests/case-management-settings
rm -fR Documents/workspace/microting/eform-angular-frontend/eform-client/e2e/Tests/case-management-general
rm -fR Documents/workspace/microting/eform-angular-frontend/eform-client/e2e/Page\ objects/case-management
rm -fR Documents/workspace/microting/eform-angular-frontend/eform-client/wdio-plugin-step2.conf.js

cp -a Documents/workspace/microting/eform-angular-basiccasemgmt-plugin/eform-client/src/app/plugins/modules/case-management-pn Documents/workspace/microting/eform-angular-frontend/eform-client/src/app/plugins/modules/case-management-pn
cp -a Documents/workspace/microting/eform-angular-basiccasemgmt-plugin/eform-client/e2e/Tests/case-management-settings Documents/workspace/microting/eform-angular-frontend/eform-client/e2e/Tests/case-managements-settings
cp -a Documents/workspace/microting/eform-angular-basiccasemgmt-plugin/eform-client/e2e/Tests/case-management-general Documents/workspace/microting/eform-angular-frontend/eform-client/e2e/Tests/case-management-general
cp -a Documents/workspace/microting/eform-angular-basiccasemgmt-plugin/eform-client/e2e/Page\ objects/case-management Documents/workspace/microting/eform-angular-frontend/eform-client/e2e/Page\ objects/case-management
cp -a Documents/workspace/microting/eform-angular-basiccasemgmt-plugin/eform-client/wdio-headless-plugin-step2.conf.js Documents/workspace/microting/eform-angular-frontend/eform-client/wdio-plugin-step2.conf.js

rm -fR Documents/workspace/microting/eform-angular-frontend/eFormAPI/Plugins/CaseManagement.Pn

cp -a Documents/workspace/microting/eform-angular-basiccasemgmt-plugin/eFormAPI/Plugins/CaseManagement.Pn Documents/workspace/microting/eform-angular-frontend/eFormAPI/Plugins/CaseManagement.Pn

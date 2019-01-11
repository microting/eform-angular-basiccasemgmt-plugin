#!/bin/bash
sed '/\/\/ INSERT ROUTES HERE/i {' src/app/plugins/plugins.routing.ts -i
sed '/\/\/ INSERT ROUTES HERE/i path: "case-management-pn",' src/app/plugins/plugins.routing.ts -i
sed '/\/\/ INSERT ROUTES HERE/i canActivate: [AuthGuard],' src/app/plugins/plugins.routing.ts -i
sed '/\/\/ INSERT ROUTES HERE/i loadChildren: "./modules/case-management-pn/case-management-pn.module#CaseManagementPnModule"' src/app/plugins/plugins.routing.ts -i
sed '/\/\/ INSERT ROUTES HERE/i }' src/app/plugins/plugins.routing.ts -i


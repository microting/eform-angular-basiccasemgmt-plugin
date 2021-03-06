import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AdminGuard, AuthGuard, PermissionGuard} from 'src/app/common/guards';

import {
  CaseManagementPnCalendarComponent,
  CaseManagementPnCalendarUsersComponent,
  CaseManagementPnCasesComponent, CaseManagementPnSettingsComponent
} from './components';
import {CaseManagementPnLayoutComponent} from './layouts';
import {CaseManagementPnClaims} from './enums';

export const routes: Routes = [
  {
    path: '',
    component: CaseManagementPnLayoutComponent,
    canActivate: [PermissionGuard],
    data: {requiredPermission: CaseManagementPnClaims.accessCaseManagementPlugin},
    children: [
      {
        path: 'calendar',
        canActivate: [AuthGuard],
        component: CaseManagementPnCalendarComponent
      },
      {
        path: 'calendar-users',
        canActivate: [AuthGuard],
        component: CaseManagementPnCalendarUsersComponent
      },
      {
        path: 'cases',
        canActivate: [AuthGuard],
        component: CaseManagementPnCasesComponent
      },
      {
        path: 'settings',
        canActivate: [AdminGuard],
        component: CaseManagementPnSettingsComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CaseManagementPnRouting {}

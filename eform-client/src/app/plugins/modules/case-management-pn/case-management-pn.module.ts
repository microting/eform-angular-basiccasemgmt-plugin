import { NgModule } from '@angular/core';
import {CommonModule, registerLocaleData} from '@angular/common';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {NgSelectModule} from '@ng-select/ng-select';
import {TranslateModule} from '@ngx-translate/core';
import {CalendarModule, DateAdapter} from 'angular-calendar';
import {adapterFactory} from 'node_modules/angular-calendar/date-adapters/date-fns';
import localeDa from '@angular/common/locales/da';
import {MDBRootModule} from 'port/angular-bootstrap-md';

import {CaseManagementPnLayoutComponent} from './layouts';
import {CaseManagementPnCalendarService, CaseManagementPnService} from './services';
import {CaseManagementPnRouting} from './case-management-pn.routing';
import {SharedPnModule} from '../shared/shared-pn.module';
import {
  CaseManagementPnCalendarComponent,
  CaseManagementPnCalendarUsersComponent,
  CaseManagementPnCalendarUserEditComponent,
  CaseManagementPnCasesComponent, CaseManagementPnCalendarUserCreateComponent
} from './components';

registerLocaleData(localeDa);

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedPnModule,
    CaseManagementPnRouting,
    TranslateModule,
    MDBRootModule,
    NgSelectModule,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    })
  ],
  declarations: [
    CaseManagementPnLayoutComponent,
    CaseManagementPnCalendarComponent,
    CaseManagementPnCalendarUsersComponent,
    CaseManagementPnCalendarUserEditComponent,
    CaseManagementPnCasesComponent,
    CaseManagementPnCalendarUserCreateComponent
  ],
  providers: [
    CaseManagementPnService,
    CaseManagementPnCalendarService
  ]
})
export class CaseManagementPnModule { }

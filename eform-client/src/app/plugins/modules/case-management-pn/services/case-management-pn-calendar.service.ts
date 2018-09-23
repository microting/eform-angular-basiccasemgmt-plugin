import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {ToastrService} from 'ngx-toastr';

import {Observable} from 'rxjs';
import {Router} from '@angular/router';
import {OperationDataResult, OperationResult} from 'src/app/common/models/operation.models';
import {BaseService} from 'src/app/common/services/base.service';
import {CalendarEventModel, CalendarEventsRequestModel,
  CalendarUserModel, CalendarUsersModel, CalendarUsersRequestModel} from '../models';

export let CaseManagementPnCalendarMethods = {
  CaseManagementPnCalendar: 'api/case-management-pn/calendar',
};

@Injectable()
export class CaseManagementPnCalendarService  extends BaseService{
  constructor(private _http: HttpClient, router: Router, toastrService: ToastrService) {
    super(_http, router, toastrService);
  }

  getCalendarEvents(model: CalendarEventsRequestModel):
    Observable<OperationDataResult<Array<CalendarEventModel>>> {
    return this.post(CaseManagementPnCalendarMethods.CaseManagementPnCalendar,
      model);
  }

  getCalendarUsers(model: CalendarUsersRequestModel):
    Observable<OperationDataResult<CalendarUsersModel>> {
    return this.get(CaseManagementPnCalendarMethods.CaseManagementPnCalendar, model);
  }

  createCalendarUser(model: CalendarUserModel): Observable<OperationResult> {
    return this.post(CaseManagementPnCalendarMethods.CaseManagementPnCalendar, model);
  }

  updateCalendarUser(model: CalendarUserModel): Observable<OperationResult> {
    return this.post(CaseManagementPnCalendarMethods.CaseManagementPnCalendar + '/update', model);
  }

  deleteCalendarUser(id: number): Observable<OperationResult> {
    return this.get(CaseManagementPnCalendarMethods.CaseManagementPnCalendar + '/delete/' + id);
  }
}

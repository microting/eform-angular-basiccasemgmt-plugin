import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {ToastrService} from 'ngx-toastr';

import {Observable} from 'rxjs';
import {Router} from '@angular/router';
import {SiteDto} from 'src/app/common/models/dto';
import {OperationDataResult, OperationResult} from 'src/app/common/models/operation.models';
import {BaseService} from 'src/app/common/services/base.service';
import {CaseManagementPnSettingsModel} from '../models';
import {CaseListModel, CasesRequestModel} from '../../../../common/models/cases';

export let CaseManagementPnMethods = {
  CaseManagementPn: 'api/case-management-pn',
  GetCases: 'api/case-management-pn/cases',
};

@Injectable()
export class CaseManagementPnService extends BaseService {
  constructor(private _http: HttpClient, router: Router, toastrService: ToastrService) {
    super(_http, router, toastrService);
  }

  getSettings(): Observable<OperationDataResult<CaseManagementPnSettingsModel>> {
    return this.get(CaseManagementPnMethods.CaseManagementPn + '/settings');
  }

  updateSettings(model: CaseManagementPnSettingsModel): Observable<OperationResult> {
    return this.post(CaseManagementPnMethods.CaseManagementPn + '/settings', model);
  }

  getCases(model: CasesRequestModel): Observable<OperationDataResult<CaseListModel>> {
    return this.post(CaseManagementPnMethods.GetCases, model);
  }
}

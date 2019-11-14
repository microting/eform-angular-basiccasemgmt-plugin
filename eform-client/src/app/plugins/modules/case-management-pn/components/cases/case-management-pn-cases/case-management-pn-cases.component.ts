import {Component, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';
import {ToastrService} from 'ngx-toastr';
import {CaseListModel, CaseModel, CasesRequestModel} from 'src/app/common/models/cases';
import {TemplateDto} from 'src/app/common/models/dto';
import {TemplateRequestModel} from 'src/app/common/models/eforms';
import {PageSettingsModel} from 'src/app/common/models/settings';
import {AuthService} from 'src/app/common/services/auth';
import {CasesService} from 'src/app/common/services/cases';
import {EFormService} from 'src/app/common/services/eform';
import {SharedPnService} from 'src/app/plugins/modules/shared/services';
import {CaseManagementPnSettingsModel} from '../../../models';
import {CaseManagementPnService} from '../../../services';
import {saveAs} from 'file-saver';
import {UserClaimsEnum} from '../../../../../../common/const';
import {SecurityGroupEformsPermissionsService} from '../../../../../../common/services/security';
import {EformPermissionsSimpleModel} from '../../../../../../common/models/security/group-permissions/eform';

@Component({
  selector: 'app-case-management-pn-cases',
  templateUrl: './case-management-pn-cases.component.html',
  styleUrls: ['./case-management-pn-cases.component.scss']
})
export class CaseManagementPnCasesComponent implements OnInit {

  @ViewChild('modalRemoveCase') modalRemoveCase;
  casesRequestModel: CasesRequestModel = new CasesRequestModel();
  caseListModel: CaseListModel = new CaseListModel();
  currentTemplate: TemplateDto = new TemplateDto;
  eformPermissionsSimpleModel: EformPermissionsSimpleModel = new EformPermissionsSimpleModel();
  vaelgKundeCase: CaseModel = new CaseModel();
  settingsModel: CaseManagementPnSettingsModel = new CaseManagementPnSettingsModel();
  localPageSettings: PageSettingsModel = new PageSettingsModel();
  spinnerStatus = false;

  get role() { return this.authService.currentRole; }
  get userClaims() { return this.authService.userClaims; }
  get userClaimsEnum() { return UserClaimsEnum; }

  constructor(private activateRoute: ActivatedRoute,
              private casesService: CasesService,
              private caseManagementService: CaseManagementPnService,
              private authService: AuthService,
              private translateService: TranslateService,
              private toastrService: ToastrService,
              private router: Router,
              private eFormService: EFormService,
              private sharedPnService: SharedPnService,
              private securityGroupEformsService: SecurityGroupEformsPermissionsService) {
  }

  ngOnInit() {
    this.getLocalPageSettings();
  }

  getLocalPageSettings() {
    this.localPageSettings = this.sharedPnService.getLocalPageSettings
    ('caseManagementPnSettings').settings;
    this.getCaseManagementSettings();
  }

  updateLocalPageSettings() {
    this.sharedPnService.updateLocalPageSettings
    ('caseManagementPnSettings', this.localPageSettings);
    this.loadAllCases();
  }

  getCaseManagementSettings() {
    this.spinnerStatus = true;
    this.caseManagementService.getSettings().subscribe((data) => {
      this.settingsModel = data.model;
      this.spinnerStatus = false;
      if (!this.settingsModel.selectedTemplateId) {
        if (this.role === 'admin') {
          this.router.navigate(['/plugins/case-management-pn/settings']);
          this.toastrService.error(
            this.translateService.instant('Select template to proceed'));
        } else {
          this.toastrService.error(
            this.translateService.instant('Contact admin to select template'));
        }
      } else {
        this.loadTemplateData();
        this.loadAllCases();
        this.loadVaelgKundeTemplate();
      }
    });
  }

  onLabelInputChanged(label: string) {
    this.casesRequestModel.nameFilter = label;
    this.loadAllCases();
  }

  onDeleteClicked(caseModel: CaseModel) {
    this.modalRemoveCase.show(caseModel);
  }

  sortTable(sort: string) {
    if (this.localPageSettings.sort === sort) {
      this.localPageSettings.isSortDsc = !this.localPageSettings.isSortDsc;
    } else {
      this.localPageSettings.isSortDsc = false;
      this.localPageSettings.sort = sort;
    }
    this.updateLocalPageSettings();
  }

  loadAllCases() {
    this.spinnerStatus = true;
    this.casesRequestModel.sort = this.localPageSettings.sort;
    this.casesRequestModel.isSortDsc = this.localPageSettings.isSortDsc;
    this.casesRequestModel.templateId = this.settingsModel.selectedTemplateId;
    this.casesRequestModel.pageSize = this.localPageSettings.pageSize;
    this.caseManagementService.getCases(this.casesRequestModel).subscribe(operation => {
      if (operation && operation.success) {
        this.caseListModel = operation.model;
      }
      this.spinnerStatus = false;
    });
  }

  loadTemplateData() {
    this.eFormService.getSingle(this.settingsModel.selectedTemplateId).subscribe(operation => {
      this.spinnerStatus = true;
      if (operation && operation.success) {
        this.currentTemplate = operation.model;
      }
      this.spinnerStatus = false;
    });
  }

  loadVaelgKundeTemplate() {
    const requestModel = new CasesRequestModel();
    requestModel.nameFilter = '...';
    this.caseManagementService.getCases(requestModel).subscribe(operation => {
      this.spinnerStatus = true;
      if (operation && operation.success && operation.model.cases[0]) {
        this.vaelgKundeCase = operation.model.cases[0];
      }
      this.spinnerStatus = false;
    });
  }

  downloadFile(caseId: number, fileType: string) {
    this.spinnerStatus = true;
    this.eFormService.downloadEformPDF(this.currentTemplate.id, caseId, fileType).subscribe(data => {
      const blob = new Blob([data]);
      saveAs(blob, `template_${this.currentTemplate.id}.${fileType}`);
      this.spinnerStatus = false;
    });
  }

  loadEformPermissions(templateId: number) {
    if (this.securityGroupEformsService.mappedPermissions.length) {
      this.eformPermissionsSimpleModel = this.securityGroupEformsService.mappedPermissions.find(x => x.templateId === templateId);
    } else {
      this.spinnerStatus = true;
      this.securityGroupEformsService.getEformsSimplePermissions().subscribe((data => {
        if (data && data.success) {
          const foundTemplates = this.securityGroupEformsService.mapEformsSimplePermissions(data.model);
          if (foundTemplates.length) {
            this.eformPermissionsSimpleModel = foundTemplates.find(x => x.templateId === templateId);
          }
          this.spinnerStatus = false;
        }
      }));
    }
  }

  checkEformPermissions(permissionIndex: number) {
    if (this.eformPermissionsSimpleModel.templateId) {
      return this.eformPermissionsSimpleModel.permissionsSimpleList.find(x => x === UserClaimsEnum[permissionIndex].toString());
    } else {
      return this.userClaims[UserClaimsEnum[permissionIndex].toString()];
    }
  }

  changePage(e: any) {
    if (e || e === 0) {
      this.casesRequestModel.offset = e;
      if (e === 0) {
        this.casesRequestModel.pageIndex = 0;
      } else {
        this.casesRequestModel.pageIndex
          = Math.floor(e / this.casesRequestModel.pageSize);
      }
      this.loadAllCases();
    }
  }
}

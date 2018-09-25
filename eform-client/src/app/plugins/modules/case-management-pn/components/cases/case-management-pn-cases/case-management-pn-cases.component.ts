import {Component, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';
import {ToastrService} from 'ngx-toastr';
import {CaseListModel, CaseModel, CasesRequestModel} from 'src/app/common/models/cases';
import {TemplateDto} from 'src/app/common/models/dto';
import {AuthService} from 'src/app/common/services/auth';
import {CasesService} from 'src/app/common/services/cases';
import {EFormService} from 'src/app/common/services/eform';
import {CaseManagementPnSettingsModel} from '../../../models';
import {CaseManagementPnService} from '../../../services';

@Component({
  selector: 'app-case-management-pn-cases',
  templateUrl: './case-management-pn-cases.component.html',
  styleUrls: ['./case-management-pn-cases.component.scss']
})
export class CaseManagementPnCasesComponent implements OnInit {

  @ViewChild('modalRemoveCase') modalRemoveCase;
  casesRequestModel: CasesRequestModel = new CasesRequestModel();
  caseListModel: CaseListModel = new CaseListModel();
  settingsModel: CaseManagementPnSettingsModel = new CaseManagementPnSettingsModel();
  spinnerStatus = false;

  get role() { return this.authService.currentRole; }

  constructor(private activateRoute: ActivatedRoute,
              private casesService: CasesService,
              private caseManagementService: CaseManagementPnService,
              private authService: AuthService,
              private translateService: TranslateService,
              private toastrService: ToastrService,
              private router: Router) {
  }

  ngOnInit() {
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
        this.loadAllCases();
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

  sortByColumn(columnName: string, sortedByDsc: boolean) {
    this.casesRequestModel.sort = columnName;
    this.casesRequestModel.isSortDsc = sortedByDsc;
    this.loadAllCases();
  }

  loadAllCases() {
    this.spinnerStatus = true;
    this.casesRequestModel.templateId = this.settingsModel.selectedTemplateId;
    this.casesService.getCases(this.casesRequestModel).subscribe(operation => {
      if (operation && operation.success) {
        this.caseListModel = operation.model;
      }
      this.spinnerStatus = false;
    });
  }

  downloadPDF(caseId: number) {
    window.open('/api/template-files/download-case-pdf/' +
      this.settingsModel.selectedTemplateId + '?caseId=' + caseId, '_blank');
  }
}

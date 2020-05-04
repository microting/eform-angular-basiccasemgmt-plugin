import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {CaseModel} from 'src/app/common/models/cases';
import {CasesService} from 'src/app/common/services/cases';

@Component({
  selector: 'app-case-management-pn-case-remove',
  templateUrl: './case-management-pn-case-remove.component.html',
  styleUrls: ['./case-management-pn-case-remove.component.scss']
})
export class CaseManagementPnCaseRemove implements OnInit {
  @ViewChild('frame') frame;
  @Input() templateId: number;
  @Output() onCaseDeleted: EventEmitter<void> = new EventEmitter<void>();
  selectedCaseModel: CaseModel = new CaseModel();

  constructor(private casesService: CasesService) { }

  ngOnInit() {
  }

  show(caseModel: CaseModel) {
    this.selectedCaseModel = caseModel;
    this.frame.show();
  }

  submitCaseDelete() {
    this.casesService.deleteCase(this.selectedCaseModel.id, this.templateId).subscribe((data => {
      if (data && data.success) {
        this.onCaseDeleted.emit();
        this.frame.hide();
      }

    }));
  }
}

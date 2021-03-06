import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {CalendarUserModel} from '../../../models';
import {CaseManagementPnCalendarService} from '../../../services';

@Component({
  selector: 'app-case-management-pn-calendar-user-delete',
  templateUrl: './case-management-pn-calendar-user-delete.component.html',
  styleUrls: ['./case-management-pn-calendar-user-delete.component.scss']
})
export class CaseManagementPnCalendarUserDeleteComponent implements OnInit {
  @Output() onUserDeleted: EventEmitter<void> = new EventEmitter<void>();
  selectedCalendarUser: CalendarUserModel = new CalendarUserModel();
  @ViewChild('frame') frame;

  constructor(private calendarService: CaseManagementPnCalendarService) { }

  ngOnInit() {

  }

  show(model: CalendarUserModel) {
    this.selectedCalendarUser = model;
    this.frame.show();
  }

  deleteCalendarUser() {
    this.calendarService.deleteCalendarUser(this.selectedCalendarUser.id).subscribe(operation => {
      if (operation && operation.success) {
        this.onUserDeleted.emit();
        this.frame.hide();
      }

    });
  }

}

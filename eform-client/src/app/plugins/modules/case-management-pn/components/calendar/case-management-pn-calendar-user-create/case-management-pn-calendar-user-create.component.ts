import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {SiteDto} from 'src/app/common/models/dto';
import {CalendarUserModel} from '../../../models';
import {CaseManagementPnCalendarService} from '../../../services';

@Component({
  selector: 'app-case-management-pn-calendar-user-create',
  templateUrl: './case-management-pn-calendar-user-create.component.html',
  styleUrls: ['./case-management-pn-calendar-user-create.component.scss']
})
export class CaseManagementPnCalendarUserCreateComponent implements OnInit {
  @Input() deviceUsers: Array<SiteDto> = [];
  newCalendarUser: CalendarUserModel = new CalendarUserModel();
  @Output() onUserCreated: EventEmitter<void> = new EventEmitter<void>();
  @ViewChild('frame') frame;

  constructor(private calendarService: CaseManagementPnCalendarService) { }

  ngOnInit() {
    this.newCalendarUser.color = '#d1f7bd';
  }

  show() {
    this.frame.show();
  }

  createCalendarUser() {
    debugger;
    this.calendarService.createCalendarUser(this.newCalendarUser).subscribe(operation => {
      if (operation && operation.success) {
        this.onUserCreated.emit();
        this.newCalendarUser = new CalendarUserModel();
        this.frame.hide();
      }

    });
  }

}

import {Component, OnInit, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {SiteDto} from 'src/app/common/models/dto';
import {DeviceUserService} from 'src/app/common/services/device-users';
import {CalendarUserModel, CalendarUsersModel, CalendarUsersRequestModel} from '../../../models/calendar/users';
import {CaseManagementPnCalendarService} from '../../../services';

@Component({
  selector: 'app-case-management-pn-calendar-users',
  templateUrl: './case-management-pn-calendar-users.component.html',
  styleUrls: ['./case-management-pn-calendar-users.component.scss']
})
export class CaseManagementPnCalendarUsersComponent implements OnInit {
  @ViewChild('editCalendarUserModal') editCalendarUserModal;
  @ViewChild('deleteCalendarUserModal') deleteCalendarUserModal;
  @ViewChild('createCalendarUserModal') createCalendarUserModal;
  selectedCalendarUser: CalendarUserModel = new CalendarUserModel();
  calendarUsers: CalendarUsersModel = new CalendarUsersModel();
  calendarUsersRequestModel: CalendarUsersRequestModel = new CalendarUsersRequestModel();
  spinnerStatus = false;
  sitesDto: Array<SiteDto>;

  constructor(
    private deviceUsersService: DeviceUserService,
    private calendarService: CaseManagementPnCalendarService,
    private router: Router) {
  }

  ngOnInit() {
    this.getCalendarUsers();
    this.loadAllSimpleSites();
  }

  openEditModal(selectedUser: CalendarUserModel) {
    this.selectedCalendarUser = selectedUser;
    this.editCalendarUserModal.show(this.selectedCalendarUser);
  }

  openDeleteModal(selectedUser: CalendarUserModel) {
    this.selectedCalendarUser = selectedUser;
    this.deleteCalendarUserModal.show(this.selectedCalendarUser);
  }

  openCreateCalendarUserModal() {
    this.createCalendarUserModal.show();
  }

  loadAllSimpleSites() {
    this.spinnerStatus = true;
    this.deviceUsersService.getAllDeviceUsers().subscribe(operation => {
      if (operation && operation.success) {
        this.sitesDto = operation.model.map(function(x) {
          x.fullName = x.firstName + ' ' + x.lastName;
          return x;
        });
      }
      this.spinnerStatus = false;
    });
  }

  getCalendarUsers() {
    this.spinnerStatus = true;
    this.calendarService.getCalendarUsers(this.calendarUsersRequestModel).subscribe((data) => {
      if (data && data.success) {
        this.calendarUsers = data.model;
      } this.spinnerStatus = false;
    });
  }

}

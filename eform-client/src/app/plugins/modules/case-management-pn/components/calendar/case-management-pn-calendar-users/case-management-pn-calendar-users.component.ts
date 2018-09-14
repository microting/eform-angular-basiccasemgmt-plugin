import {Component, OnInit, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {SimpleSiteModel} from 'src/app/common/models/device-users';
import {SiteDto} from 'src/app/common/models/dto';
import {DeviceUserService} from 'src/app/common/services/device-users';

@Component({
  selector: 'app-case-management-pn-calendar-users',
  templateUrl: './case-management-pn-calendar-users.component.html',
  styleUrls: ['./case-management-pn-calendar-users.component.scss']
})
export class CaseManagementPnCalendarUsersComponent implements OnInit {
  @ViewChild('editDeviceUserModal') editDeviceUserModal;
  selectedSimpleSite: SimpleSiteModel = new SimpleSiteModel;
  spinnerStatus = true;
  sitesDto: Array<SiteDto>;

  constructor(
    private deviceUsersService: DeviceUserService,
    private router: Router) {
  }

  ngOnInit() {
    this.loadAllSimpleSites();
  }

  openEditModal(simpleSiteDto: SiteDto) {
    this.selectedSimpleSite.userFirstName = simpleSiteDto.firstName;
    this.selectedSimpleSite.userLastName = simpleSiteDto.lastName;
    this.selectedSimpleSite.id = simpleSiteDto.siteId;
    this.editDeviceUserModal.show();
  }

  loadAllSimpleSites() {
    this.spinnerStatus = true;
    this.deviceUsersService.getAllSimpleSites().subscribe(operation => {
      if (operation && operation.success) {
        this.sitesDto = operation.model;
      }
      this.spinnerStatus = false;
    });
  }

}

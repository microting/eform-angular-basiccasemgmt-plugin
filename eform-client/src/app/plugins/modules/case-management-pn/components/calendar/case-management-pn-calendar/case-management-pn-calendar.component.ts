import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';
import {isSameDay, isSameMonth, toDate} from 'date-fns';
import {ToastrService} from 'ngx-toastr';
import {Subject} from 'rxjs';
import {
  CalendarEvent,
  CalendarEventTitleFormatter,
  CalendarView
} from 'angular-calendar';
import {AuthService, LocaleService} from 'src/app/common/services/auth';
import {CalendarEventsRequestModel, CaseManagementPnSettingsModel} from '../../../models';
import {CustomEventTitleFormatter} from '../../../services/calendar/custom-event-title-formatter.provider';
import {CaseManagementPnCalendarService, CaseManagementPnService} from '../../../services';

@Component({
  selector: 'app-case-management-pn-calendar',
  templateUrl: './case-management-pn-calendar.component.html',
  providers: [
    {
      provide: CalendarEventTitleFormatter,
      useClass: CustomEventTitleFormatter
    }
  ],
  styleUrls: ['./case-management-pn-calendar.component.scss']
})
export class CaseManagementPnCalendarComponent implements OnInit {
  spinnerStatus = false;
  settingsModel: CaseManagementPnSettingsModel = new CaseManagementPnSettingsModel();
  calendarEventsRequestModel: CalendarEventsRequestModel = new CalendarEventsRequestModel();
  view: CalendarView = CalendarView.Month;

  get role() {
    return this.authService.currentRole;
  }

  CalendarView = CalendarView;

  viewDate: Date = new Date();
  locale: string;

  refresh: Subject<any> = new Subject();

  events: Array<CalendarEvent<any>> = [];

  activeDayIsOpen = false;

  constructor(
    private calendarService: CaseManagementPnCalendarService,
    private caseManagementService: CaseManagementPnService,
    private localeService: LocaleService,
    private translateService: TranslateService,
    private toastrService: ToastrService,
    private router: Router,
    private authService: AuthService
  ) {
  }

  ngOnInit() {
    let userLocale = this.localeService.getCurrentUserLocale();
    if (userLocale === 'da-DK') {
      this.locale = 'da';
    } else {
      this.locale = 'en';
    }
    this.caseManagementService.getSettings().subscribe((data) => {
      this.settingsModel = data.model;
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
        this.calendarEventsRequestModel.templateId =
          this.settingsModel.selectedTemplateId;
        this.getEvents();
      }
    });
  }

  dayClicked({date, events}: { date: Date; events: CalendarEvent[] }): void {
    if (isSameMonth(date, this.viewDate)) {
      this.viewDate = date;
      if (
        (isSameDay(this.viewDate, date) && this.activeDayIsOpen === true) ||
        events.length === 0
      ) {
        this.activeDayIsOpen = false;
      } else {
        this.activeDayIsOpen = true;
      }
    }
  }

  getEvents() {
    this.spinnerStatus = true;
    this.calendarService.getCalendarEvents(this.calendarEventsRequestModel).subscribe((data) => {
      if (data && data.success) {
        if (data.model.length > 0) {
          for (let calendarEvent of data.model) {
            this.events.push(
              {
                start: toDate(calendarEvent.start),
                end: toDate(calendarEvent.end),
                title: calendarEvent.title,
                color: {
                  primary: calendarEvent.color,
                  secondary: calendarEvent.color
                },
                allDay: true,
                meta: {
                  calendarUserName: calendarEvent.meta.calendarUserName,
                  caseId: calendarEvent.meta.caseId
                }
              }
            );
          }
        }

        this.refresh.next();
      }
      this.spinnerStatus = false;
    });
  }

  eventClicked({event}: { event: CalendarEvent }): void {
    debugger;
    this.router.navigate(['/cases/edit', event.meta.caseId,
      this.settingsModel.selectedTemplateId
    ]).then();
  }
}

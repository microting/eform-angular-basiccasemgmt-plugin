import { Component, OnInit } from '@angular/core';
import {Router} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';
import {addDays, addHours, endOfMonth, isSameDay, isSameMonth, startOfDay, subDays} from 'date-fns';
import {ToastrService} from 'ngx-toastr';
import {Subject} from 'rxjs';
import {
  CalendarEvent,
  CalendarEventAction,
  CalendarEventTimesChangedEvent, CalendarEventTitleFormatter,
  CalendarView,
  DAYS_OF_WEEK
} from 'angular-calendar';
import {AuthService, LocaleService} from 'src/app/common/services/auth';
import {CalendarEventsRequestModel, CaseManagementPnSettingsModel} from '../../../models';
import {CustomEventTitleFormatter} from '../../../services/calendar/custom-event-title-formatter.provider';
import {CaseManagementPnCalendarService, CaseManagementPnService} from '../../../services';

const colors: any = {
  red: {
    primary: '#ad2121',
    secondary: '#FAE3E3'
  },
  blue: {
    primary: '#1e90ff',
    secondary: '#D1E8FF'
  },
  yellow: {
    primary: '#e3bc08',
    secondary: '#FDF1BA'
  }
};

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
  calendarEvents = [];

  get role() { return this.authService.currentRole }

  CalendarView = CalendarView;

  viewDate: Date = new Date();

  modalData: {
    action: string;
    event: CalendarEvent;
  };

  excludeDays: number[] = [0, 6];
  locale: string;

  weekStartsOn = DAYS_OF_WEEK.SUNDAY;

  actions: CalendarEventAction[] = [
    {
      label: '<i class="fa fa-fw fa-pencil"></i>',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        this.handleEvent('Edited', event);
      }
    },
    {
      label: '<i class="fa fa-fw fa-times"></i>',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        this.events = this.events.filter(iEvent => iEvent !== event);
        this.handleEvent('Deleted', event);
      }
    }
  ];

  refresh: Subject<any> = new Subject();

  events: CalendarEvent<any>[] = [
    {
      start: subDays(startOfDay(new Date()), 1),
      end: addDays(new Date(), 1),
      title: 'A 3 day event',
      color: colors.red,
      actions: this.actions,
      allDay: true,
      meta: {
        calendarUserName: '',
        caseId: 1,
      }
    }
  ];

  activeDayIsOpen = true;
  constructor(
              private calendarService: CaseManagementPnCalendarService,
              private caseManagementService: CaseManagementPnService,
              private localeService: LocaleService,
              private translateService: TranslateService,
              private toastrService: ToastrService,
              private router: Router,
              private authService: AuthService
  ) { }

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
       // this.getEvents();
      }
    });
  }

  dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }): void {
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

  handleEvent(action: string, event: CalendarEvent): void {
    this.modalData = { event, action };
  }

  getEvents() {
    this.spinnerStatus = true;
    this.calendarService.getCalendarEvents(this.calendarEventsRequestModel).subscribe((data) => {
       if (data && data.success) {
          if (data.model.length > 0) {
            for (let calendarEvent of data.model) {
              this.calendarEvents.push(
                {
                  start: calendarEvent.fromDate,
                  end: calendarEvent.toDate,
                  title: calendarEvent.title,
                  color: calendarEvent.color,
                  meta: {}
                }
              );
            }
          }
          this.calendarEvents = data.model;
       }
    });
  }

  eventClicked({ event }: { event: CalendarEvent }): void {
    this.router.navigate(['/edit', event.meta.caseId,
      this.settingsModel.selectedTemplateId
      ]).then();
  }
}

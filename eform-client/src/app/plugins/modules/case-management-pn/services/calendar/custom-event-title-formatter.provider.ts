import { LOCALE_ID, Inject } from '@angular/core';
import { CalendarEventTitleFormatter, CalendarEvent } from 'angular-calendar';
import { DatePipe } from '@angular/common';

export class CustomEventTitleFormatter extends CalendarEventTitleFormatter {
  constructor(@Inject(LOCALE_ID) private locale: string) {
    super();
  }

  // you can override any of the methods defined in the parent class

  month(event: CalendarEvent<any>): string {
    return `<b>${event.meta.calendarUserName}</b> - ${event.title}`;
  }

  week(event: CalendarEvent): string {
    return `<b>${event.meta.calendarUserName}</b> - ${event.title}`;
  }

  day(event: CalendarEvent): string {
    return `<b>${event.meta.calendarUserName}</b> - ${event.title}`;
  }
}

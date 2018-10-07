using System.Collections.Generic;
using CaseManagement.Pn.Infrastructure.Models.Calendar;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace CaseManagement.Pn.Abstractions
{
    public interface ICalendarService
    {
        OperationDataResult<List<CalendarEventModel>> GetCalendarEvents(CalendarEventsRequestModel requestModel);
    }
}
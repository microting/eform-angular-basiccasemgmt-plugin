using System.Collections.Generic;
using System.Threading.Tasks;
using CaseManagement.Pn.Infrastructure.Models.Calendar;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace CaseManagement.Pn.Abstractions
{
    public interface ICalendarService
    {
        Task<OperationDataResult<List<CalendarEventModel>>> GetCalendarEvents(CalendarEventsRequestModel requestModel);
    }
}
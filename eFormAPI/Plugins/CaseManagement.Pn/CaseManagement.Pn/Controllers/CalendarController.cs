using System.Collections.Generic;
using System.Threading.Tasks;
using CaseManagement.Pn.Abstractions;
using CaseManagement.Pn.Infrastructure.Models.Calendar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace CaseManagement.Pn.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [HttpPost]
        [Route("api/case-management-pn/calendar/events")]
        public async Task<OperationDataResult<List<CalendarEventModel>>> Index(CalendarEventsRequestModel requestModel)
        {
            return await _calendarService.Index(requestModel);
        }
    }
}
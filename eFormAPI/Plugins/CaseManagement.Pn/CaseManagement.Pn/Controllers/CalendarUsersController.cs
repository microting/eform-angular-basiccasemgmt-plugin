using CaseManagement.Pn.Abstractions;
using CaseManagement.Pn.Infrastructure.Models.Calendar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace CaseManagement.Pn.Controllers
{
    [Authorize]
    public class CalendarUsersController : Controller
    {
        private readonly ICalendarUsersService _calendarUsersService;

        public CalendarUsersController(ICalendarUsersService calendarUsersService)
        {
            _calendarUsersService = calendarUsersService;
        }

        [HttpPost]
        [Route("api/case-management-pn/calendar/get-all")]
        public OperationDataResult<CalendarUsersModel> GetCalendarUsers(CalendarUsersRequestModel requestModel)
        {
            return _calendarUsersService.GetCalendarUsers(requestModel);
        }

        [HttpPost]
        [Route("api/case-management-pn/calendar")]
        public OperationResult CreateCalendarUser(CalendarUserModel requestModel)
        {
            return _calendarUsersService.CreateCalendarUser(requestModel);
        }

        [HttpPost]
        [Route("api/case-management-pn/calendar/update")]
        public OperationResult UpdateCalendarUser(CalendarUserModel requestModel)
        {
            return _calendarUsersService.UpdateCalendarUser(requestModel);
        }

        [HttpGet]
        [Route("api/case-management-pn/calendar/delete/{id}")]
        public OperationResult DeleteCalendarUser(int id)
        {
            return _calendarUsersService.DeleteCalendarUser(id);
        }
    }
}
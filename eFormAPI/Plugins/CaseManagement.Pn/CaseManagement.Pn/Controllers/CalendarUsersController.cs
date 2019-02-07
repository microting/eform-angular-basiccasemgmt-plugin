using System.Threading.Tasks;
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
        public async Task<OperationDataResult<CalendarUsersModel>> GetCalendarUsers(CalendarUsersRequestModel requestModel)
        {
            return await _calendarUsersService.GetCalendarUsers(requestModel);
        }

        [HttpPost]
        [Route("api/case-management-pn/calendar")]
        public async Task<OperationResult> CreateCalendarUser(CalendarUserModel requestModel)
        {
            return await _calendarUsersService.CreateCalendarUser(requestModel);
        }

        [HttpPost]
        [Route("api/case-management-pn/calendar/update")]
        public async Task<OperationResult> UpdateCalendarUser(CalendarUserModel requestModel)
        {
            return await _calendarUsersService.UpdateCalendarUser(requestModel);
        }

        [HttpGet]
        [Route("api/case-management-pn/calendar/delete/{id}")]
        public async Task<OperationResult> DeleteCalendarUser(int id)
        {
            return await _calendarUsersService.DeleteCalendarUser(id);
        }
    }
}
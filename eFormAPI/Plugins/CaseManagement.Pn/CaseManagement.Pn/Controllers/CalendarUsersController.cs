using System;
using System.IO;
using System.Threading.Tasks;
using CaseManagement.Pn.Abstractions;
using CaseManagement.Pn.Infrastructure.Models.Calendar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
        [AllowAnonymous]
        [DebuggingFilter]
        [Route("api/case-management-pn/calendar")]
        public async Task<OperationResult> CreateCalendarUser([FromBody] CalendarUserModel requestModel)
        {
            return await _calendarUsersService.CreateCalendarUser(requestModel);
        }

        [HttpPost]
        [Route("api/case-management-pn/calendar/update")]
        public async Task<OperationResult> UpdateCalendarUser([FromBody] CalendarUserModel requestModel)
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
    public class DebuggingFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Method != "POST")
            {
                return;
            }
            
            filterContext.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);

            string text = new StreamReader(filterContext.HttpContext.Request.Body).ReadToEndAsync().Result;

            filterContext.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            
            Console.WriteLine(text);

            base.OnActionExecuting(filterContext);
        }
    }
}
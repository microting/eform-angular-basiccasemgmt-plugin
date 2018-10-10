using CaseManagement.Pn.Infrastructure.Models.Calendar;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace CaseManagement.Pn.Abstractions
{
    public interface ICalendarUsersService
    {
        OperationResult CreateCalendarUser(CalendarUserModel requestModel);
        OperationResult DeleteCalendarUser(int id);
        OperationDataResult<CalendarUsersModel> GetCalendarUsers(CalendarUsersRequestModel requestModel);
        OperationResult UpdateCalendarUser(CalendarUserModel requestModel);
    }
}
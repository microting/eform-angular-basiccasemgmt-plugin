using System.Threading.Tasks;
using CaseManagement.Pn.Infrastructure.Models.Calendar;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace CaseManagement.Pn.Abstractions
{
    public interface ICalendarUsersService
    {
        Task<OperationResult> CreateCalendarUser(CalendarUserModel requestModel);
        Task<OperationResult> DeleteCalendarUser(int id);
        Task<OperationDataResult<CalendarUsersModel>> GetCalendarUsers(CalendarUsersRequestModel requestModel);
        Task<OperationResult> UpdateCalendarUser(CalendarUserModel requestModel);
    }
}
using System.Threading.Tasks;
using CaseManagement.Pn.Infrastructure.Models.Calendar;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace CaseManagement.Pn.Abstractions
{
    public interface ICalendarUsersService
    {
        Task<OperationDataResult<CalendarUsersModel>> Index(CalendarUsersRequestModel requestModel);
        Task<OperationResult> Create(CalendarUserModel requestModel);
        Task<OperationResult> Update(CalendarUserModel requestModel);
        Task<OperationResult> Delete(int id);
    }
}
using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using CaseManagement.Pn.Infrastructure.Data;
using CaseManagement.Pn.Infrastructure.Data.Entities;
using CaseManagement.Pn.Infrastructure.Extensions;
using CaseManagement.Pn.Infrastructure.Helpers;
using CaseManagement.Pn.Infrastructure.Models.Calendar;
using Microting.eFormApi.BasePn.Infrastructure;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using NLog;

namespace CaseManagement.Pn.Controllers
{
    [Authorize]
    public class CalendarUsersController : ApiController
    {
        private readonly Logger _logger;
        private readonly CaseManagementPnDbContext _dbContext;
        private readonly EFormCoreHelper _coreHelper = new EFormCoreHelper();

        public CalendarUsersController()
        {
            _dbContext = CaseManagementPnDbContext.Create();
            _logger = LogManager.GetCurrentClassLogger();
        }

        [HttpPost]
        [Route("api/case-management-pn/calendar/get-all")]
        public OperationDataResult<CalendarUsersModel> GetCalendarUsers(CalendarUsersRequestModel requestModel)
        {
            try
            {
                var calendarUsersModel = new CalendarUsersModel();
                var calendarUsersQuery = _dbContext.CalendarUsers.AsQueryable();
                if (!string.IsNullOrEmpty(requestModel.Sort))
                {
                    if (requestModel.IsSortDsc)
                    {
                        calendarUsersQuery = calendarUsersQuery
                            .OrderByDescending(requestModel.Sort);
                    }
                    else
                    {
                        calendarUsersQuery = calendarUsersQuery
                            .OrderBy(requestModel.Sort);
                    }
                }
                else
                {
                    calendarUsersQuery = _dbContext.CalendarUsers
                        .OrderBy(x => x.Id);
                }
                calendarUsersQuery = calendarUsersQuery
                    .Skip(requestModel.Offset)
                    .Take(requestModel.PageSize);

                var calendarUsers = calendarUsersQuery.ToList();
                calendarUsersModel.Total = _dbContext.CalendarUsers.Count();
                var core = _coreHelper.GetCore();
                calendarUsers.ForEach(calendarUser =>
                {
                    var item = new CalendarUserModel
                    {
                        Id = calendarUser.Id,
                        SiteId = calendarUser.SiteId,
                        IsVisibleInCalendar = calendarUser.IsVisibleInCalendar,
                        NameInCalendar = calendarUser.NameInCalendar,
                        Color = calendarUser.Color,
                    };
                    if (item.SiteId > 0)
                    {
                        var site = core.SiteRead(item.SiteId);
                        if (site != null)
                        {
                            item.FirstName = site.FirstName;
                            item.LastName = site.LastName;
                        }
                    }
                    calendarUsersModel.CalendarUsers.Add(item);
                });
                return new OperationDataResult<CalendarUsersModel>(true, calendarUsersModel);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                _logger.Error(e);
                return new OperationDataResult<CalendarUsersModel>(false,
                    CustomersPnLocaleHelper.GetString("ErrorWhileObtainingCalendarUserInfo"));
            }
        }

        [HttpPost]
        [Route("api/case-management-pn/calendar")]
        public OperationResult CreateCalendarUser(CalendarUserModel requestModel)
        {
            try
            {
                var calendarUser = new CalendarUser()
                {
                    SiteId = requestModel.SiteId,
                    NameInCalendar = requestModel.NameInCalendar,
                    Color = requestModel.Color,
                    IsVisibleInCalendar = requestModel.IsVisibleInCalendar,
                };
                _dbContext.CalendarUsers.Add(calendarUser);
                _dbContext.SaveChanges();
                var caseManagementSetting = _dbContext.CaseManagementSettings.FirstOrDefault();
                if (caseManagementSetting?.RelatedEntityGroupId != null)
                {
                    var core = _coreHelper.GetCore();
                    var entityGroup = core.EntityGroupRead(caseManagementSetting.RelatedEntityGroupId.ToString());
                    if (entityGroup == null)
                    {
                        return new OperationResult(false, "Entity group not found");
                    }

                    var nextItemUid = entityGroup.EntityGroupItemLst.Count;
                    var label = calendarUser.NameInCalendar;
                    if (string.IsNullOrEmpty(label))
                    {
                        label = $"Empty company {nextItemUid}";
                    }
                    var item = core.EntitySelectItemCreate(entityGroup.Id, $"{label}", 0,
                        nextItemUid.ToString());
                    if (item != null)
                    {
                        entityGroup = core.EntityGroupRead(caseManagementSetting.RelatedEntityGroupId.ToString());
                        if (entityGroup != null)
                        {
                            foreach (var entityItem in entityGroup.EntityGroupItemLst)
                            {
                                if (entityItem.MicrotingUUID == item.MicrotingUUID)
                                {
                                    calendarUser.RelatedEntityId = entityItem.Id;
                                }
                            }
                        }
                    }
                    _dbContext.SaveChanges();
                }
                return new OperationResult(true,
                    CustomersPnLocaleHelper.GetString("CalendarUserHasBeenCreated"));
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                _logger.Error(e);
                return new OperationResult(false,
                    CustomersPnLocaleHelper.GetString("ErrorWhileCreatingCalendarUser"));
            }
        }

        [HttpPost]
        [Route("api/case-management-pn/calendar/update")]
        public OperationResult UpdateCalendarUser(CalendarUserModel requestModel)
        {
            try
            {
                var calendarUser = _dbContext.CalendarUsers.FirstOrDefault(x => x.Id == requestModel.Id);
                if (calendarUser == null)
                {
                    return new OperationResult(false,
                        CustomersPnLocaleHelper.GetString("CalendarUserNotFound"));
                }

                calendarUser.SiteId = requestModel.SiteId;
                calendarUser.Color = requestModel.Color;
                calendarUser.IsVisibleInCalendar = requestModel.IsVisibleInCalendar;
                calendarUser.NameInCalendar = requestModel.NameInCalendar;
                _dbContext.SaveChanges();
                var caseManagementSetting = _dbContext.CaseManagementSettings.FirstOrDefault();
                if (caseManagementSetting?.RelatedEntityGroupId != null)
                {
                    var core = _coreHelper.GetCore();
                    var entityGroup = core.EntityGroupRead(caseManagementSetting.RelatedEntityGroupId.ToString());
                    if (entityGroup == null)
                    {
                        return new OperationResult(false, "Entity group not found");
                    }

                    var nextItemUid = entityGroup.EntityGroupItemLst.Count;
                    var label = calendarUser.NameInCalendar;
                    if (string.IsNullOrEmpty(label))
                    {
                        label = $"Empty company {nextItemUid}";
                    }
                    core.EntityItemUpdate(entityGroup.Id, $"{label}", "",
                        nextItemUid.ToString(),0);
                    //if (item != null)
                    //{
                    //    entityGroup = core.EntityGroupRead(caseManagementSetting.RelatedEntityGroupId.ToString());
                    //    if (entityGroup != null)
                    //    {
                    //        foreach (var entityItem in entityGroup.EntityGroupItemLst)
                    //        {
                    //            if (entityItem.MicrotingUUID == item.MicrotingUUID)
                    //            {
                    //                calendarUser.RelatedEntityId = entityItem.Id;
                    //            }
                    //        }
                    //    }
                    //}
                    //_dbContext.SaveChanges();
                }

                return new OperationResult(true,
                    CustomersPnLocaleHelper.GetString("CalendarUserUpdatedSuccessfully"));
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                _logger.Error(e);
                return new OperationResult(false,
                    CustomersPnLocaleHelper.GetString("ErrorWhileUpdatingCalendarUser"));
            }
        }


        [HttpGet]
        [Route("api/case-management-pn/calendar/delete/{id}")]
        public OperationResult DeleteCalendarUser(int id)
        {
            try
            {
                var calendarUser = _dbContext.CalendarUsers.FirstOrDefault(x => x.Id == id);
                if (calendarUser == null)
                {
                    return new OperationResult(false,
                        CustomersPnLocaleHelper.GetString("CalendarUserNotFound"));
                }
                _dbContext.CalendarUsers.Remove(calendarUser);
                _dbContext.SaveChanges();
                return new OperationResult(true,
                    CustomersPnLocaleHelper.GetString("CalendarUserDeletedSuccessfully"));
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                _logger.Error(e);
                return new OperationResult(false,
                    CustomersPnLocaleHelper.GetString("ErrorWhileDeletingCalendarUser"));
            }
        }
    }
}
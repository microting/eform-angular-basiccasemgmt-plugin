﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CaseManagement.Pn.Abstractions;
using CaseManagement.Pn.Infrastructure.Data;
using CaseManagement.Pn.Infrastructure.Data.Entities;
using CaseManagement.Pn.Infrastructure.Extensions;
using CaseManagement.Pn.Infrastructure.Models;
using CaseManagement.Pn.Infrastructure.Models.Calendar;
using eFormCore;
using eFormData;
using eFormShared;
using Microsoft.Extensions.Logging;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace CaseManagement.Pn.Services
{
    public class CalendarUsersService : ICalendarUsersService
    {
        private readonly ILogger<CalendarUsersService> _logger;
        private readonly CaseManagementPnDbContext _dbContext;
        private readonly ICaseManagementLocalizationService _caseManagementLocalizationService;
        private readonly IEFormCoreService _coreHelper;

        public CalendarUsersService(ILogger<CalendarUsersService> logger,
            CaseManagementPnDbContext dbContext,
            IEFormCoreService coreHelper,
            ICaseManagementLocalizationService caseManagementLocalizationService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _coreHelper = coreHelper;
            _caseManagementLocalizationService = caseManagementLocalizationService;
        }

        public async Task<OperationDataResult<CalendarUsersModel>> GetCalendarUsers(CalendarUsersRequestModel requestModel)
        {
            try
            {
                CalendarUsersModel calendarUsersModel = new CalendarUsersModel();
                IQueryable<CalendarUser> calendarUsersQuery = _dbContext.CalendarUsers.AsQueryable();
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
                    .Where(x => x.WorkflowState != Constants.WorkflowStates.Removed)
                    .Skip(requestModel.Offset)
                    .Take(requestModel.PageSize);

                List<CalendarUser> calendarUsers = calendarUsersQuery.ToList();
                calendarUsersModel.Total = _dbContext.CalendarUsers.Count();
                Core core = _coreHelper.GetCore();
                calendarUsers.ForEach(calendarUser =>
                {
                    CalendarUserModel item = new CalendarUserModel
                    {
                        Id = calendarUser.Id,
                        SiteId = calendarUser.SiteId,
                        IsVisibleInCalendar = calendarUser.IsVisibleInCalendar,
                        NameInCalendar = calendarUser.NameInCalendar,
                        Color = calendarUser.Color,
                    };
                    if (item.SiteId > 0)
                    {
                        Site_Dto site = core.SiteRead(item.SiteId);
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
                _logger.LogError(e.Message);
                return new OperationDataResult<CalendarUsersModel>(false,
                    _caseManagementLocalizationService.GetString("ErrorWhileObtainingCalendarUserInfo"));
            }
        }

        public async Task<OperationResult> CreateCalendarUser(CalendarUserModel calendarUserModel)
        {
            try
            {
                CalendarUser calendarUser = new CalendarUser()
                {
                    SiteId = calendarUserModel.SiteId,
                    NameInCalendar = calendarUserModel.NameInCalendar,
                    Color = calendarUserModel.Color,
                    IsVisibleInCalendar = calendarUserModel.IsVisibleInCalendar,
                };
                _dbContext.CalendarUsers.Add(calendarUser);
                await _dbContext.SaveChangesAsync();
//                await calendarUserModel.Save(_dbContext); //TODO
                PluginConfigurationValue caseManagementSetting =
                    _dbContext.PluginConfigurationValues.SingleOrDefault(x =>
                        x.Name == "CaseManagementBaseSettings:RelatedEntityGroupId");
                
                if (caseManagementSetting != null)
                {
                    Core core = _coreHelper.GetCore();
                    EntityGroup entityGroup = core.EntityGroupRead(caseManagementSetting.Value);
                    if (entityGroup == null)
                    {
                        return new OperationResult(false, "Entity group not found");
                    }

                    int nextItemUid = entityGroup.EntityGroupItemLst.Count;
                    string label = calendarUser.NameInCalendar;
                    if (string.IsNullOrEmpty(label))
                    {
                        label = $"Empty company {nextItemUid}";
                    }

                    EntityItem item = core.EntitySelectItemCreate(entityGroup.Id, $"{label}", 0,
                        nextItemUid.ToString());
                    if (item != null)
                    {
                        entityGroup = core.EntityGroupRead(caseManagementSetting.Value);
                        if (entityGroup != null)
                        {
                            foreach (EntityItem entityItem in entityGroup.EntityGroupItemLst)
                            {
                                if (entityItem.MicrotingUUID == item.MicrotingUUID)
                                {
                                    calendarUser.RelatedEntityId = entityItem.Id;
                                }
                            }
                        }
                    }

                    await _dbContext.SaveChangesAsync();
                }

                return new OperationResult(true,
                    _caseManagementLocalizationService.GetString("CalendarUserHasBeenCreated"));
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                _logger.LogError(e.Message);
                return new OperationResult(false,
                    _caseManagementLocalizationService.GetString("ErrorWhileCreatingCalendarUser"));
            }
        }

        public async Task<OperationResult> UpdateCalendarUser(CalendarUserModel requestModel)
        {
            try
            {
                CalendarUser calendarUser = _dbContext.CalendarUsers.FirstOrDefault(x => x.Id == requestModel.Id);
                if (calendarUser == null)
                {
                    return new OperationResult(false,
                        _caseManagementLocalizationService.GetString("CalendarUserNotFound"));
                }

                calendarUser.SiteId = requestModel.SiteId;
                calendarUser.Color = requestModel.Color;
                calendarUser.IsVisibleInCalendar = requestModel.IsVisibleInCalendar;
                calendarUser.NameInCalendar = requestModel.NameInCalendar;
//                await requestModel.Update(_dbContext); //TODO 
                await _dbContext.SaveChangesAsync();
                PluginConfigurationValue caseManagementSetting = _dbContext.PluginConfigurationValues.SingleOrDefault(x =>
                    x.Name == "CaseManagementBaseSettings:RelatedEntityGroupId");
                if (caseManagementSetting != null)
                {
                    Core core = _coreHelper.GetCore();
                    EntityGroup entityGroup = core.EntityGroupRead(caseManagementSetting.Value);
                    if (entityGroup == null)
                    {
                        return new OperationResult(false, "Entity group not found");
                    }

                    int nextItemUid = entityGroup.EntityGroupItemLst.Count;
                    string label = requestModel.NameInCalendar;
                    if (string.IsNullOrEmpty(label))
                    {
                        label = $"Empty company {nextItemUid}";
                    }

                    core.EntityItemUpdate(entityGroup.Id, $"{label}", "",
                        nextItemUid.ToString(), 0);
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
                    _caseManagementLocalizationService.GetString("CalendarUserUpdatedSuccessfully"));
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                _logger.LogError(e.Message);
                return new OperationResult(false,
                    _caseManagementLocalizationService.GetString("ErrorWhileUpdatingCalendarUser"));
            }
        }

        public async Task<OperationResult> DeleteCalendarUser(int id)
        {
            try
            {
                CalendarUserModel deleteModel = new CalendarUserModel();
                deleteModel.Id = id;
                deleteModel.Delete(_dbContext);
                return new OperationResult(true);

//                CalendarUser calendarUser = _dbContext.CalendarUsers.FirstOrDefault(x => x.Id == id);
//                if (calendarUser == null)
//                {
//                    return new OperationResult(false,
//                        _caseManagementLocalizationService.GetString("CalendarUserNotFound"));
//                }
//
//                _dbContext.CalendarUsers.Remove(calendarUser);
//                _dbContext.SaveChanges();
//                return new OperationResult(true,
//                    _caseManagementLocalizationService.GetString("CalendarUserDeletedSuccessfully"));
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                _logger.LogError(e.Message);
                return new OperationResult(false,
                    _caseManagementLocalizationService.GetString("ErrorWhileDeletingCalendarUser"));
            }
        }
    }
}
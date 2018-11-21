using System;
using System.Linq;
using CaseManagement.Pn.Infrastructure.Data;
using CaseManagement.Pn.Infrastructure.Data.Entities;

namespace CaseManagement.Pn.Infrastructure.Models.Calendar
{
    public class CalendarUserModel : IModel
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CreatedByUserId { get; set; }
        public int UpdatedByUserId { get; set; }
        public string WorkflowState { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SiteId { get; set; }
        public bool IsVisibleInCalendar { get; set; }
        public string NameInCalendar { get; set; }
        public string Color { get; set; }

        public void Save(CaseManagementPnDbAnySql _dbContext)
        {
            CalendarUser calendarUser = new CalendarUser();
            calendarUser.Color = Color;
            calendarUser.IsVisibleInCalendar = IsVisibleInCalendar;
            calendarUser.NameInCalendar = NameInCalendar;
            calendarUser.SiteId = SiteId;
            _dbContext.CalendarUsers.Add(calendarUser);
            _dbContext.SaveChanges();

            //_dbContext.CalendarUsersVersion.Add(MapCalendarUserVersion(_dbContext, calendarUser));
            _dbContext.SaveChanges();
        }

        public void Update(CaseManagementPnDbAnySql _dbContext)
        {
            CalendarUser calendarUser = _dbContext.CalendarUsers.FirstOrDefault(x => x.Id == Id);

            if (calendarUser == null)
            {
                throw new NullReferenceException($"Could not fint Calendar User with id {Id}");
            }

            calendarUser.Color = Color;
            calendarUser.IsVisibleInCalendar = IsVisibleInCalendar;
            calendarUser.NameInCalendar = NameInCalendar;
            calendarUser.SiteId = SiteId;
            
            if (_dbContext.ChangeTracker.HasChanges())
            {
                calendarUser.Updated_By_User_Id = UpdatedByUserId;
                calendarUser.Updated_at = DateTime.Now;
                calendarUser.Version += 1;

                //_dbContext.CalendarUsersVersion.Add(MapCalendarUserVersion(_dbContext, calendarUser));
                _dbContext.SaveChanges();
            }
        }

        public void Delete(CaseManagementPnDbAnySql _dbContext)
        {
            CalendarUser calendarUser = _dbContext.CalendarUsers.FirstOrDefault(x => x.Id == Id);

            if (calendarUser == null)
            {
                throw new NullReferenceException($"Could not fint Calendar User with id {Id}");
            }

            calendarUser.Workflow_state = eFormShared.Constants.WorkflowStates.Removed;

            if (_dbContext.ChangeTracker.HasChanges())
            {
                calendarUser.Updated_at = DateTime.Now;
                calendarUser.Updated_By_User_Id = UpdatedByUserId;
                calendarUser.Version += 1;

                //_dbContext.CalendarUsersVersion.Add(MapCalendarUserVersion(_dbContext, calendarUser));
                _dbContext.SaveChanges();
            }
        }
    }
}
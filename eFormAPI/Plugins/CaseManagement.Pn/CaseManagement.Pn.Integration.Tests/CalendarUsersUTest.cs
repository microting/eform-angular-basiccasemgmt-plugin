using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaseManagement.Pn.Infrastructure.Data.Entities;
using CaseManagement.Pn.Infrastructure.Models.Calendar;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace CaseManagement.Pn.Integration.Tests
{
    [TestFixture]
    public class CalendarUsersUTest : DbTestFixture
    {

        [Test]
        public async Task CalendarUserModel_Save_DoesSave()
        {
            // Arrange
            Random rnd = new Random();
            CalendarUserModel calendarUserModel = new CalendarUserModel();
            calendarUserModel.Color = Guid.NewGuid().ToString();
            calendarUserModel.NameInCalendar = Guid.NewGuid().ToString();
            calendarUserModel.IsVisibleInCalendar = rnd.Next(100) < 50;
            calendarUserModel.SiteId = rnd.Next(1, 255);
            
            // Act
            await calendarUserModel.Save(DbContext);

            CalendarUser dbCalendarUser = DbContext.CalendarUsers.AsNoTracking().First();
            List<CalendarUser> userList = DbContext.CalendarUsers.AsNoTracking().ToList();
            //List<CalendarUserVersions> versionList = DbContext.CalendarUsersVersions.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(dbCalendarUser);

            Assert.AreEqual(1, userList.Count());

            Assert.AreEqual(calendarUserModel.Color, dbCalendarUser.Color);
            Assert.AreEqual(calendarUserModel.IsVisibleInCalendar, dbCalendarUser.IsVisibleInCalendar);
            Assert.AreEqual(calendarUserModel.NameInCalendar, dbCalendarUser.NameInCalendar);
            Assert.AreEqual(calendarUserModel.SiteId, dbCalendarUser.SiteId);
            
        }

        [Test]
        public async Task CalendarUserModel_Update_DoesUpdate()
        {
            // Arrange
            Random rnd = new Random();

            CalendarUser calendarUser = new CalendarUser();
            calendarUser.Color = Guid.NewGuid().ToString();
            calendarUser.IsVisibleInCalendar = rnd.Next(100) < 50;
            calendarUser.NameInCalendar = Guid.NewGuid().ToString();
            calendarUser.SiteId = rnd.Next(1, 255);

            DbContext.CalendarUsers.Add(calendarUser);
            await DbContext.SaveChangesAsync();
            //CalendarUserVersion calendarUserVer = new CalendarUserVersion();


            // Act
            CalendarUserModel calendarUserModel = new CalendarUserModel();
            calendarUserModel.Color = Guid.NewGuid().ToString();
            calendarUserModel.IsVisibleInCalendar = rnd.Next(100)<50;
            calendarUserModel.NameInCalendar = "Georg Jensen";
            calendarUserModel.SiteId = rnd.Next(1,255);

            calendarUserModel.Id = calendarUser.Id;

            await calendarUserModel.Update(DbContext);

            CalendarUser dbCalendarUser = DbContext.CalendarUsers.AsNoTracking().First();
            List<CalendarUser> userList = DbContext.CalendarUsers.AsNoTracking().ToList();
            //List<CalendarUserVersions> versionList = DbContext.CalendarUsersVersions.AsNoTracking().ToList();          

            // Assert
            Assert.NotNull(dbCalendarUser);

            Assert.AreEqual(1, userList.Count());

            Assert.AreEqual(calendarUserModel.Color, dbCalendarUser.Color);
            Assert.AreEqual(calendarUserModel.IsVisibleInCalendar, dbCalendarUser.IsVisibleInCalendar);
            Assert.AreEqual(calendarUserModel.NameInCalendar, dbCalendarUser.NameInCalendar);
            Assert.AreEqual(calendarUserModel.SiteId, dbCalendarUser.SiteId);

        }

        [Test]
        public async Task CalendarUserModel_Delete_DoesDelete()
        {
            // Arrange
            Random rnd = new Random();

            CalendarUser calendarUser = new CalendarUser();
            calendarUser.Color = Guid.NewGuid().ToString();
            calendarUser.IsVisibleInCalendar = rnd.Next(100) < 50;
            calendarUser.NameInCalendar = Guid.NewGuid().ToString();
            calendarUser.SiteId = rnd.Next(1, 255);
            calendarUser.Workflow_state = eFormShared.Constants.WorkflowStates.Created;
            DbContext.CalendarUsers.Add(calendarUser);
            await DbContext.SaveChangesAsync();
            //CalendarUserVersion calendarUserVer = new CalendarUserVersion();


            // Act
            CalendarUserModel calendarUserModel = new CalendarUserModel();
            calendarUserModel.Color = calendarUser.Color;
            calendarUserModel.IsVisibleInCalendar = calendarUser.IsVisibleInCalendar;
            calendarUserModel.NameInCalendar = calendarUser.NameInCalendar;
            calendarUserModel.SiteId = calendarUser.SiteId;

            calendarUserModel.Id = calendarUser.Id;

            await calendarUserModel.Delete(DbContext);

            CalendarUser dbCalendarUser = DbContext.CalendarUsers.AsNoTracking().First();
            List<CalendarUser> userList = DbContext.CalendarUsers.AsNoTracking().ToList();
            //List<CalendarUserVersions> versionList = DbContext.CalendarUsersVersions.AsNoTracking().ToList();          

            // Assert
            Assert.NotNull(dbCalendarUser);

            Assert.AreEqual(1, userList.Count());

            Assert.AreEqual(calendarUserModel.Color, dbCalendarUser.Color);
            Assert.AreEqual(calendarUserModel.IsVisibleInCalendar, dbCalendarUser.IsVisibleInCalendar);
            Assert.AreEqual(calendarUserModel.NameInCalendar, dbCalendarUser.NameInCalendar);
            Assert.AreEqual(calendarUserModel.SiteId, dbCalendarUser.SiteId);
            Assert.AreEqual(eFormShared.Constants.WorkflowStates.Removed, dbCalendarUser.Workflow_state);

        }
    }
}

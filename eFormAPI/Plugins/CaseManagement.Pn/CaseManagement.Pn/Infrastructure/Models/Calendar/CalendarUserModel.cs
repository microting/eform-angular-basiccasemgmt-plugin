using CaseManagement.Pn.Infrastructure.Data;

namespace CaseManagement.Pn.Infrastructure.Models.Calendar
{
    public class CalendarUserModel : IModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SiteId { get; set; }
        public bool IsVisibleInCalendar { get; set; }
        public string NameInCalendar { get; set; }
        public string Color { get; set; }

        public void Save(CaseManagementPnDbAnySql _dbContext)
        {

        }
        public void Update(CaseManagementPnDbAnySql _dbContext)
        {

        }
        public void Delete(CaseManagementPnDbAnySql _dbContext)
        {

        }
    }
}
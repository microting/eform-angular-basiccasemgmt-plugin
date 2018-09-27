using System.Collections.Generic;

namespace CaseManagement.Pn.Infrastructure.Models.Calendar
{
    public class CalendarUsersModel
    {
        public int Total { get; set; }
        public List<CalendarUserModel> CalendarUsers { get; set; } 
            = new List<CalendarUserModel>();
    }
}
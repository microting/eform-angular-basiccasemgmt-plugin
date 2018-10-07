using System;

namespace CaseManagement.Pn.Infrastructure.Models.Calendar
{
    public class CalendarEventModel
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public CalendarEventMeta Meta { get; set; } = new CalendarEventMeta();
    }
}
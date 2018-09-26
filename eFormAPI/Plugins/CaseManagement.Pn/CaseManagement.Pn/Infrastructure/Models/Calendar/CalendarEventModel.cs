using System;
using System.Collections.Generic;

namespace CaseManagement.Pn.Infrastructure.Models.Calendar
{
    public class CalendarEventModel
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public List<CalendarEventMeta> Meta { get; set; } 
            = new List<CalendarEventMeta>();

    }
}
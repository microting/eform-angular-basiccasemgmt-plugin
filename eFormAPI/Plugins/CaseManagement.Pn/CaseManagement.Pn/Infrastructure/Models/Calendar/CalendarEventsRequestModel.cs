using System;

namespace CaseManagement.Pn.Infrastructure.Models.Calendar
{
    public class CalendarEventsRequestModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TemplateId { get; set; }
    }
}
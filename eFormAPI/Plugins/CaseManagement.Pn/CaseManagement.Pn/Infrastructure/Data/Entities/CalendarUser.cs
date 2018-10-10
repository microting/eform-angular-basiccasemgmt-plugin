using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace CaseManagement.Pn.Infrastructure.Data.Entities
{
    public class CalendarUser : BaseEntity
    {
        public int SiteId { get; set; }
        public bool IsVisibleInCalendar { get; set; }
        public string NameInCalendar { get; set; }
        public string Color { get; set; }
        public int RelatedEntityId { get; set; }
    }
}
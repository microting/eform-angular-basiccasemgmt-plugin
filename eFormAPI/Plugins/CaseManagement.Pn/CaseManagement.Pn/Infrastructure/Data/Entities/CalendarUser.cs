using System;
using System.ComponentModel.DataAnnotations;
using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace CaseManagement.Pn.Infrastructure.Data.Entities
{
    public class CalendarUser : BaseEntity
    {
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public int Created_By_User_Id { get; set; }
        public int Updated_By_User_Id { get; set; }
        public int Version { get; set; }
        [StringLength(255)]
        public string Workflow_state { get; set; }
        public int SiteId { get; set; }
        public bool IsVisibleInCalendar { get; set; }
        public string NameInCalendar { get; set; }
        public string Color { get; set; }
        public int RelatedEntityId { get; set; }
    }
}
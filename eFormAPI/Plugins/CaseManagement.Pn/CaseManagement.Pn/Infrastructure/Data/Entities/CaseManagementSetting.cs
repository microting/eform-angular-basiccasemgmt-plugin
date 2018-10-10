using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace CaseManagement.Pn.Infrastructure.Data.Entities
{
    public class CaseManagementSetting : BaseEntity
    {
        public int? SelectedTemplateId { get; set; }
        public string SelectedTemplateName { get; set; }
        public int? RelatedEntityGroupId { get; set; }
    }
}
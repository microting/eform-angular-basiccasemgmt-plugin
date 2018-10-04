namespace CaseManagement.Pn.Infrastructure.Models
{
    public class CaseManagementPnSettingsModel
    {
        public int? SelectedTemplateId { get; set; }
        public string SelectedTemplateName { get; set; }
        public int? RelatedEntityGroupId { get; set; }
        public string RelatedEntityGroupName { get; set; }

    }
}
using CaseManagement.Pn.Infrastructure.Data;

namespace CaseManagement.Pn.Infrastructure.Models
{
    public class CaseManagementPnSettingsModel : IModel
    {
        public int? SelectedTemplateId { get; set; }
        public string SelectedTemplateName { get; set; }
        public int? RelatedEntityGroupId { get; set; }
        public string RelatedEntityGroupName { get; set; }

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
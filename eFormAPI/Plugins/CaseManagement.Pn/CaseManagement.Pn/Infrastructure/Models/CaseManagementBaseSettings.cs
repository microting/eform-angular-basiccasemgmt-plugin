namespace CaseManagement.Pn.Infrastructure.Models
{
    public class CaseManagementBaseSettings
    {
        public string LogLevel { get; set; }
        public string LogLimit { get; set; }
        public string SdkConnectionString { get; set; }
        public string MaxParallelism { get; set; }
        public int NumberOfWorkers { get; set; }
        public int SelectedTemplateId { get; set; }
        public int RelatedEntityGroupId { get; set; }
    }
}
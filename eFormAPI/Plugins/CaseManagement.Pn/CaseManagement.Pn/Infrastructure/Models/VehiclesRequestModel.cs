namespace CaseManagement.Pn.Infrastructure.Models
{
    public class VehiclesRequestModel
    {
        public string SortColumnName { get; set; }
        public int Offset { get; set; }
        public int PageSize { get; set; }
        public bool IsSortDsc { get; set; }
    }
}

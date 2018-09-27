namespace CaseManagement.Pn.Infrastructure.Models.Calendar
{
    public class CalendarUsersRequestModel
    {
        public string Sort { get; set; } = "Id";
        public string NameFilter { get; set; }
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsSortDsc { get; set; } = true;
        public int Offset { get; set; } = 0;
    }
}
using System.Collections.Generic;

namespace CaseManagement.Pn.Infrastructure.Models
{
    public class VehiclesModel
    {
        public int Total { get; set; }
        public List<VehicleModel> Vehicles { get; set; }

        public VehiclesModel()
        {
            Vehicles = new List<VehicleModel>();
        }
    }
}

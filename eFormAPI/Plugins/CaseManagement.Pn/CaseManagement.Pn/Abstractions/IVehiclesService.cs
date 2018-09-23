using CaseManagement.Pn.Infrastructure.Models;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace CaseManagement.Pn.Abstractions
{
    public interface IVehiclesService
    {
        OperationDataResult<VehiclesModel> GetAllVehicles(VehiclesRequestModel pnRequestModel);
        OperationResult CreateVehicle(VehicleModel vehiclePnCreateModel);
        OperationResult UpdateVehicle(VehicleModel vehiclePnUpdateModel);
    }
}

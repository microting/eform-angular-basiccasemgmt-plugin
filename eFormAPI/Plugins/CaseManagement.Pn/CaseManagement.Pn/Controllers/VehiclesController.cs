using CaseManagement.Pn.Abstractions;
using CaseManagement.Pn.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace CaseManagement.Pn.Controllers
{
    [Authorize]
    public class VehiclesController : Controller
    {
        private readonly IVehiclesService _vehiclesService;

        public VehiclesController(IVehiclesService vehiclesService)
        {
            _vehiclesService = vehiclesService;
        }

        [HttpPost]
        [Route("api/vehicles-pn")]
        public OperationDataResult<VehiclesModel> GetAllVehicles([FromBody] VehiclesRequestModel requestModel)
        {
            return _vehiclesService.GetAllVehicles(requestModel);
        }

        [HttpPost]
        [Route("api/vehicles-pn/create-vehicle")]
        public OperationResult CreateVehicle([FromBody] VehicleModel vehicleCreateModel)
        {
            return _vehiclesService.CreateVehicle(vehicleCreateModel);
        }

        [HttpPost]
        [Route("api/vehicles-pn/update-vehicle")]
        public OperationResult UpdateVehicle([FromBody] VehicleModel vehicleUpdateModel)
        {
            return _vehiclesService.UpdateVehicle(vehicleUpdateModel);
        }
    }
}
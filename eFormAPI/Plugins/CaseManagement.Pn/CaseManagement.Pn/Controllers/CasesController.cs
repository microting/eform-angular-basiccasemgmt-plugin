using System.Threading.Tasks;
using CaseManagement.Pn.Abstractions;
using eFormAPI.Web.Abstractions.Security;
using eFormAPI.Web.Infrastructure;
using eFormAPI.Web.Infrastructure.Models.Cases.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseManagement.Pn.Controllers
{
    [Authorize]
    public class CasesController : Controller
    {
        private readonly ICaseManagementCasesService _caseManagementCasesService;
//        private readonly IEformPermissionsService _permissionsService;

        public CasesController(ICaseManagementCasesService caseManagementCasesService)
        {
            _caseManagementCasesService = caseManagementCasesService;
//            _permissionsService = permissionsService;
        }

        [HttpPost]
        [Route("api/case-management-pn/cases")]
        [Authorize(Policy = AuthConsts.EformPolicies.Cases.CasesRead)]
        public async Task<IActionResult> Index([FromBody] CaseRequestModel requestModel)
        {
//            if (requestModel.TemplateId != null
//                && ! await _permissionsService.CheckEform((int) requestModel.TemplateId,
//                    AuthConsts.EformClaims.CasesClaims.CasesRead))
//            {
//                return Forbid();
//            }

            return Ok(await _caseManagementCasesService.Index(requestModel));
        }
    }
}
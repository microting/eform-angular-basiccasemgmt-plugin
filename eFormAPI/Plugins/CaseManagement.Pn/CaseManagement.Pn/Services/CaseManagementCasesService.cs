/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Threading.Tasks;
using CaseManagement.Pn.Abstractions;
using Microting.eForm.Infrastructure.Constants;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace CaseManagement.Pn.Services
{
    public class CaseManagementCasesService : ICaseManagementCasesService
    {
        private readonly IEFormCoreService _coreHelper;
        private readonly ICaseManagementLocalizationService _localizationService;

        public CaseManagementCasesService(IEFormCoreService coreHelper,
            ICaseManagementLocalizationService localizationService)
        {
            _coreHelper = coreHelper;
            _localizationService = localizationService;
        }
        
//        public async Task<OperationDataResult<CaseListModel>> Index(CaseRequestModel requestModel)
//        {
//            try
//            {
//                var core = await _coreHelper.GetCore();
//                var caseList = await core.CaseReadAll(requestModel.TemplateId, null, null,
//                    Constants.WorkflowStates.NotRemoved, requestModel.NameFilter,
//                    requestModel.IsSortDsc, requestModel.Sort, requestModel.PageIndex, requestModel.PageSize);
//                var model = new CaseListModel()
//                {
//                    NumOfElements = caseList.NumOfElements,
//                    PageNum = caseList.PageNum,
//                    Cases = caseList.Cases
//                };
//
//                return new OperationDataResult<CaseListModel>(true, model);
//            }
//            catch (Exception)
//            {
//                return new OperationDataResult<CaseListModel>(false, _localizationService.GetString("CaseLoadingFailed"));
//            }
//        }
    }
}
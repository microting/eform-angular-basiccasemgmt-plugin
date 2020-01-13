using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CaseManagement.Pn.Infrastructure.Data;
using Microting.eFormBasicCaseManagementBase.Infrastructure.Data;

namespace CaseManagement.Pn.Infrastructure.Models
{
    interface IModel
    {
        Task Create(eFormCaseManagementPnDbContext _dbContext);

        Task Update(eFormCaseManagementPnDbContext _dbContext);

        Task Delete(eFormCaseManagementPnDbContext _dbContext);

    }
}

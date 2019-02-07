using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CaseManagement.Pn.Infrastructure.Data;

namespace CaseManagement.Pn.Infrastructure.Models
{
    interface IModel
    {
        Task Save(CaseManagementPnDbAnySql _dbContext);

        Task Update(CaseManagementPnDbAnySql _dbContext);

        Task Delete(CaseManagementPnDbAnySql _dbContext);

    }
}
